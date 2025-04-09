using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[System.Serializable]
public struct DamierDatas
{
    public CellPos pos;
    public bool? breakable;
}

[System.Serializable]
public struct CellData
{

    public CellData(bool? breakable, Cell cell)
    {
        this.breakable = breakable;
        this.cell = cell;
    }

    public void SetBreakable(bool? isBreakable) { breakable = isBreakable; }
    public bool? GetIsBreakable() { return breakable; }

    public bool? breakable;
    public Cell cell;
}

public class Damier : MonoBehaviour
{

    [Header("Debug")]
    [SerializeField] private Material _blue;
    [SerializeField] private Material _white;
    [SerializeField] private Material _green;

    [Header("Damier")]
    [SerializeField] private int _damierSize = 6;
    [SerializeField] private GameObject _riwa;

    [HideInInspector][SerializeField] List<DamierDatas> serializedDamier = new List<DamierDatas>(); // Delete when Save is done
    [HideInInspector][SerializeField] List<Cell> serializedCells = new List<Cell>(); // Delete when Save is done too

    List<CellPos> path = new List<CellPos>();

    private float _pathTravelTime = 7.0f;

    Dictionary<CellPos, Cell> _cells = new Dictionary<CellPos, Cell>();
    Dictionary<CellPos, bool?> _damier = new Dictionary<CellPos, bool?>(); // This has to be saved later

    private void Awake()
    {
        _damier.Clear();
        foreach (var data in serializedDamier)
            _damier.Add(data.pos, data.breakable);

        _cells.Clear();
        foreach(var cell in serializedCells)
        {
            _cells.Add(cell.Position, cell);
            cell.OnCellTriggered += OnCellTriggered;
        }
    }

    private void Start()
    {
        GeneratePath();
    }

    // Basic test damier generation (visualisation)
    [ContextMenu("Generate Damier")]
    public void Generate()
    {
        float cellSize = 1f;
        for (int x = 0; x < _damierSize; x++)
        {
            for (int y = 0; y < _damierSize; y++)
            {
                CellPos pos = new CellPos(x, y);
                GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cell.transform.position = transform.position + new Vector3(x * cellSize, 0, y * cellSize);
                cell.transform.localScale = new Vector3(cellSize, 0.1f, cellSize);
                cell.GetComponent<Renderer>().material = (x + y) % 2 == 0 ? _blue : _white;
                cell.transform.parent = transform;
                cell.name = $"Cell {x} {y}";
                BoxCollider collider = cell.AddComponent<BoxCollider>();
                collider.isTrigger = true;
                collider.size = new Vector3(1f, 1f, 1f);
                collider.center = new Vector3(0f, 0.5f, 0f);
                Cell cellScript = cell.AddComponent<Cell>();
                cellScript.Init(pos);
                _cells[pos] = cellScript;
                _damier[pos] = true;
            }
        }

        serializedDamier.Clear();
        foreach(var pair in _damier)
            serializedDamier.Add(new DamierDatas() { pos = pair.Key, breakable = pair.Value });

        serializedCells.Clear();
        foreach(var pair in _cells)
            serializedCells.Add(pair.Value);
    }

    public void GeneratePath()
    {
        path.Clear();

        CellPos end = new CellPos(_damierSize - 1, _damierSize - 1);

        List<CellPos> options = new List<CellPos>()
         {
             new CellPos(0, 0),
             new CellPos(0, 1)
         };
        List<CellPos> possibleMoves = new List<CellPos>();
        List<CellPos> existingNeighbors = new List<CellPos>();

        _damier[end] = false;

        int pathStart = Random.Range(0, options.Count);
        CellPos current = options[pathStart];
        _damier[current] = false;
        path.Add(current);

        System.Random random = new System.Random();

        bool stopPathGeneration = false;
        int upCounter = 0;
        int downCounter = 0;
        int rightCounter = 0;
        int leftCounter = 0;

        while (current != end)
        {
            existingNeighbors.Clear();
            possibleMoves.Clear();

            CellPos currentLeft = current.GetCellAtDirection(CellPos.Direction.Left);
            CellPos currentRight = current.GetCellAtDirection(CellPos.Direction.Right);
            CellPos currentUp = current.GetCellAtDirection(CellPos.Direction.Up);
            CellPos currentDown = current.GetCellAtDirection(CellPos.Direction.Down);

            if ((_damier.ContainsKey(currentUp) && currentUp == end) ||
                (_damier.ContainsKey(currentRight) && currentRight == end))
            {
                path.Add(current);
                _damier[current] = false;
                break;
            }

            if (CheckDirection(currentRight) && rightCounter != 2) existingNeighbors.Add(currentRight);
            if (CheckDirection(currentDown) && downCounter != 2 && current.x < 4) existingNeighbors.Add(currentDown);
            if (CheckDirection(currentUp) && upCounter != 2) existingNeighbors.Add(currentUp);
            if (CheckDirection(currentLeft) && leftCounter != 2) existingNeighbors.Add(currentLeft);

            if (existingNeighbors.Contains(currentUp))
            {
                CellPos leftOfUp = currentUp.GetCellAtDirection(CellPos.Direction.Left);
                if (_damier.ContainsKey(leftOfUp) && _damier[leftOfUp] == false)
                {
                    existingNeighbors.Remove(currentUp);
                }
            }

            if (existingNeighbors.Contains(currentDown))
            {
                CellPos leftOfDown = currentDown.GetCellAtDirection(CellPos.Direction.Left);
                if (_damier.ContainsKey(leftOfDown) && _damier[leftOfDown] == false)
                    if ((_damier.ContainsKey(leftOfDown) && _damier[leftOfDown] == false) || current.x <= 1)
                    {
                        existingNeighbors.Remove(currentDown);
                    }
            }

            if (existingNeighbors.Contains(currentLeft))
            {
                if (currentLeft.y >= 4 || currentLeft.y <= 1) existingNeighbors.Remove(currentLeft);
            }

            for (int i = 0; i < existingNeighbors.Count; i++)
            {
                CellPos index = existingNeighbors[i];
                if (_damier[current] != _damier[index])
                    possibleMoves.Add(index);
            }

            for (int j = possibleMoves.Count - 1; j >= 0; j--)
            {
                CellPos index = possibleMoves[j];

                if (_damier.ContainsKey(index) && _damier[index] == false)
                {
                    possibleMoves.RemoveAt(j);
                    continue;
                }

                int blockedNeighborCount = 0;

                CellPos rightOfSelectedCell = index.GetCellAtDirection(CellPos.Direction.Right);
                CellPos leftOfSelectedCell = index.GetCellAtDirection(CellPos.Direction.Left);
                CellPos upOfSelectedCell = index.GetCellAtDirection(CellPos.Direction.Up);
                CellPos downOfSelectedCell = index.GetCellAtDirection(CellPos.Direction.Down);

                if (rightOfSelectedCell != end && IsTargetedCellTaken(rightOfSelectedCell)) blockedNeighborCount++;
                if (upOfSelectedCell != end && IsTargetedCellTaken(upOfSelectedCell)) blockedNeighborCount++;
                if (downOfSelectedCell != end && IsTargetedCellTaken(downOfSelectedCell)) blockedNeighborCount++;
                if (leftOfSelectedCell != end && IsTargetedCellTaken(leftOfSelectedCell)) blockedNeighborCount++;

                if (blockedNeighborCount >= 2)
                    possibleMoves.RemoveAt(j);
            }

            for (int i = 0; i < possibleMoves.Count; i++)
            {
                CellPos move = possibleMoves[i];

                if (path.Contains(move)) continue;

                if (move == end || IsAdjacentToEnd(move, end))
                {
                    current = move;
                    _damier[current] = false;

                    if ((current.x == 5 && current.y == 4) || (current.x == 4 && current.y == 5))
                    {
                        stopPathGeneration = true;
                        break;
                    }

                    continue;
                }
            }

            if (possibleMoves.Count == 0)
            {
                Debug.LogWarning("No more possible moves from: " + current.x + ", " + current.y);
                break;
            }

            int moveIndex = random.Next(0, possibleMoves.Count);
            CellPos nextMove = possibleMoves[moveIndex];

            if (!path.Contains(nextMove) && stopPathGeneration == false)
            {
                path.Add(nextMove);
                current = nextMove;
                _damier[current] = false;
            }

            if (current == nextMove.GetCellAtDirection(CellPos.Direction.Up)) upCounter++;
            if (current == nextMove.GetCellAtDirection(CellPos.Direction.Right)) rightCounter++;
            if (current == nextMove.GetCellAtDirection(CellPos.Direction.Down)) downCounter++;
            if (current == nextMove.GetCellAtDirection(CellPos.Direction.Left)) leftCounter++;

            if (upCounter == 2) upCounter = 0;
            if (downCounter == 2) downCounter = 0;
            if (leftCounter == 2) leftCounter = 0;
            if (rightCounter == 2) rightCounter = 0;
        }


        path.Add(end);

        List<CellPos> keys = new List<CellPos>(_damier.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            CellPos key = keys[i];
            if (_damier[key] == null)
                _damier[key] = true;
        }
        //List<CellPos> keys = new List<CellPos>(_damier.Keys);
        //for (int i = 0; i < keys.Count; i++)
        //{
        //    CellPos key = keys[i];
        //    if (_damier[key] == null)
        //        _damier[key] = true;
        //}

        foreach (Transform cell in transform)
        {
            CellPos pos = new CellPos((int)cell.position.x - (int)transform.position.x, (int)cell.position.z - (int)transform.position.z);

            if (_damier.TryGetValue(pos, out bool? breakable))
            {
                Renderer rend = cell.GetComponent<Renderer>();
                if (rend != null)
                {
                    if (breakable == false)
                    {
                        Debug.Log(cell.name);
                        rend.material = _green;
                    }
                }
            }
        }

        RiwaFollowPath();

    }

    private void RiwaFollowPath()
    {
        StartCoroutine(FollowPathCoroutine());
    }

    private IEnumerator FollowPathCoroutine()
    {
        List<Vector3> controlPoints = new List<Vector3>();
        foreach (var cell in path)
        {
            controlPoints.Add(new Vector3(cell.x + transform.position.x, _riwa.transform.localPosition.y, cell.y + transform.position.z));
        }

        if (controlPoints.Count < 2)
            yield break;

        float curveStrength = 1f;
        for (int i = 1; i < controlPoints.Count - 1; i++)
        {
            Vector3 prev = controlPoints[i - 1];
            Vector3 current = controlPoints[i];
            Vector3 next = controlPoints[i + 1];

            Vector3 dirPrev = (current - prev).normalized;
            Vector3 dirNext = (next - current).normalized;

            float angle = Vector3.Angle(dirPrev, dirNext);

            if (angle > 150.0f)
            {
                Vector3 perpendicular = Vector3.Cross(dirPrev, dirNext).normalized;

                controlPoints[i] += perpendicular * curveStrength;
            }
        }

        float elapsedTime = 0.0f;

        Vector3 startPos = controlPoints[0];
        Vector3 endPos = controlPoints[controlPoints.Count - 1];
        Vector3 prevPoint = startPos;

        while (elapsedTime < _pathTravelTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / _pathTravelTime);

            Vector3 currentPos = GetBezierPoint(t, controlPoints);

            _riwa.transform.position = currentPos;

            Vector3 direction = (currentPos - prevPoint).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);
            _riwa.transform.rotation = rotation;

            prevPoint = currentPos;

            yield return null;
        }

        _riwa.transform.position = endPos;
        _riwa.transform.rotation = Quaternion.LookRotation(endPos - prevPoint);
    }

    private Vector3 GetBezierPoint(float t, List<Vector3> controlPoints)
    {
        if (controlPoints.Count == 2)
            return Vector3.Lerp(controlPoints[0], controlPoints[1], t);
        else
        {
            List<Vector3> nextPoints = new List<Vector3>();

            for (int i = 0; i < controlPoints.Count - 1; i++)
                nextPoints.Add(Vector3.Lerp(controlPoints[i], controlPoints[i + 1], t));

            return GetBezierPoint(t, nextPoints);
        }
    }

    private bool CheckDirection(CellPos pos)
    {
        if (_damier.ContainsKey(pos)) return true;
        else return false;
    }

    private bool IsTargetedCellTaken(CellPos target)
    {
        return _damier.ContainsKey(target) && _damier[target] == false;
    }

    bool IsAdjacentToEnd(CellPos a, CellPos b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        return (dx + dy) == 1;
    }

    private void OnCellTriggered(CellPos pos, Cell cell)
    {
        if(_cells.TryGetValue(pos, out Cell triggeredCell) && _damier.ContainsKey(pos))
        {
            Destroy(cell.gameObject);
            _damier[pos] = null;
        }
    }
}