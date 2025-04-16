using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CellState
{
    NotBreakable,
    Breakable,
    Broken
}

[System.Serializable]
public struct DamierDatas
{
    public DamierDatas(CellPos pos, GameObject cell, CellState state)
    {
        this.cellPos = pos;
        this.cell = cell;
        this.cellState = state;
    }

    public void SetCellState(CellState state) { cellState = state; }

    public CellPos cellPos;
    public GameObject cell;
    public CellState cellState;
}

public class Damier : MonoBehaviour
{

    [Header("Debug")]
    [SerializeField] private Material _blue;
    [SerializeField] private Material _white;
    [SerializeField] private Material _green;

    [Header("Damier")]
    [SerializeField] private int _damierSize = 6;
    [SerializeField] private float _cellSize = 1.5f;
    [SerializeField] private float _lerpTime = 1.0f;
    [SerializeField] private Vector3 _rotation = Vector3.zero;
    [SerializeField] private GameObject _riwa;

    [HideInInspector][SerializeField] List<DamierDatas> serializedDamier = new List<DamierDatas>(); // Delete when Save is done

    List<CellPos> path = new List<CellPos>();

    Dictionary<CellPos, DamierDatas> _damier = new Dictionary<CellPos, DamierDatas>(); // This has to be saved later

    private void Awake()
    {
        _damier.Clear();
        foreach (var data in serializedDamier)
        {
            Cell script = data.cell.GetComponent<Cell>();
            script.Init(data.cellPos);
            _damier.Add(data.cellPos, new DamierDatas(data.cellPos, data.cell, data.cellState));
            script.OnCellTriggered += OnCellTriggered;
        }

        GameManager.Instance.Character.OnRespawn += RespawnBrokenTile;
    }

    private void Start()
    {
        GeneratePath();
    }

    [ContextMenu("Generate Damier")]
    public void Generate()
    {

        Quaternion rotation = Quaternion.Euler(_rotation);
        Vector3 centerOffset = new Vector3((_damierSize - 1) * _cellSize, 0f, (_damierSize - 1) * _cellSize);

        for (int x = 0; x < _damierSize; x++)
        {
            for (int y = 0; y < _damierSize; y++)
            {
                CellPos pos = new CellPos(x, y);
                GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Vector3 localPos = new Vector3(x * (_cellSize + 0.05f), 0, y * (_cellSize + 0.05f)) - centerOffset;
                Vector3 rotatedPos = rotation * localPos;
                cell.transform.position = transform.position + rotatedPos;
                cell.transform.localScale = new Vector3(_cellSize, 0.1f, _cellSize);
                cell.GetComponent<Renderer>().material = (x + y) % 2 == 0 ? _blue : _white;
                cell.transform.parent = transform;
                cell.name = $"Cell {x} {y}";
                BoxCollider collider = cell.GetComponent<BoxCollider>();
                collider.isTrigger = true;
                collider.size = new Vector3(1f, 1f, 1f);
                collider.center = new Vector3(0f, 0.5f, 0f);
                Cell cellScript = cell.AddComponent<Cell>();
                cellScript.Init(pos);
                Rigidbody rb = cell.AddComponent<Rigidbody>();
                rb.isKinematic = true;
                cell.layer = 6;
                _damier[pos] = new DamierDatas(pos, cell, CellState.Breakable);
            }
        }

        serializedDamier.Clear();
        foreach(var pair in _damier)
            serializedDamier.Add(new DamierDatas() { cellPos = pair.Key, cellState = pair.Value.cellState, cell = pair.Value.cell });
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

        ChangeCellState(end, CellState.NotBreakable);

        int pathStart = Random.Range(0, options.Count);
        CellPos current = options[pathStart];
        ChangeCellState(current, CellState.NotBreakable);
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
                ChangeCellState(current, CellState.NotBreakable);
                break;
            }

            if (CheckDirection(currentRight) && rightCounter != 2) existingNeighbors.Add(currentRight);
            if (CheckDirection(currentDown) && downCounter != 2 && current.x < 4) existingNeighbors.Add(currentDown);
            if (CheckDirection(currentUp) && upCounter != 2) existingNeighbors.Add(currentUp);
            if (CheckDirection(currentLeft) && leftCounter != 2) existingNeighbors.Add(currentLeft);

            if (existingNeighbors.Contains(currentUp))
            {
                CellPos leftOfUp = currentUp.GetCellAtDirection(CellPos.Direction.Left);
                if (_damier.ContainsKey(leftOfUp) && _damier[leftOfUp].cellState == CellState.NotBreakable)
                {
                    existingNeighbors.Remove(currentUp);
                }
            }

            if (existingNeighbors.Contains(currentDown))
            {
                CellPos leftOfDown = currentDown.GetCellAtDirection(CellPos.Direction.Left);
                if (_damier.ContainsKey(leftOfDown) && _damier[leftOfDown].cellState == CellState.NotBreakable)
                    if ((_damier.ContainsKey(leftOfDown) && _damier[leftOfDown].cellState == CellState.NotBreakable) || current.x <= 1)
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
                if (_damier[current].cellState != _damier[index].cellState)
                    possibleMoves.Add(index);
            }

            for (int j = possibleMoves.Count - 1; j >= 0; j--)
            {
                CellPos index = possibleMoves[j];

                if (_damier.ContainsKey(index) && _damier[index].cellState == CellState.NotBreakable)
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
                    ChangeCellState(current, CellState.NotBreakable);

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
                ChangeCellState(current, CellState.NotBreakable);
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

        foreach (Transform cell in transform)
        {
            Cell cellScript = cell.GetComponent<Cell>();
            if (cellScript == null) continue;

            CellPos pos = cellScript.Position;

            if (_damier.TryGetValue(pos, out DamierDatas datas))
            {
                Renderer rend = datas.cell.GetComponent<Renderer>();
                if (rend != null && datas.cellState == CellState.NotBreakable)
                    rend.material = _green;
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

        if (path.Count < 2)
            yield break;

        Vector3 startPosRiwa = Vector3.zero;
        if(_damier.TryGetValue(path[0], out DamierDatas startData))
        {
            startPosRiwa = startData.cell.transform.position;
            startPosRiwa.y = _riwa.transform.position.y + 0.5f;
        }

        _riwa = Instantiate(_riwa, startPosRiwa, Quaternion.identity);
        _riwa.transform.localScale = new Vector3(2f, 2f, 2f);

        List<Vector3> worldPoints = new List<Vector3>();
        foreach (var cell in path)
        {
            if (_damier.TryGetValue(cell, out DamierDatas data))
            {
                Vector3 pos = data.cell.transform.position;
                pos.y = _riwa.transform.position.y;
                worldPoints.Add(pos);
            }
        }

        _riwa.transform.position = worldPoints[0];
        Vector3 initialDir = (worldPoints[1] - worldPoints[0]).normalized;
        _riwa.transform.rotation = Quaternion.LookRotation(new Vector3(initialDir.x, 0, initialDir.z));

        for (int i = 0; i < worldPoints.Count - 1; i++)
        {
            Vector3 p0 = i == 0 ? worldPoints[i] : worldPoints[i - 1];
            Vector3 p1 = worldPoints[i];
            Vector3 p2 = worldPoints[i + 1];
            Vector3 p3 = (i + 2 < worldPoints.Count) ? worldPoints[i + 2] : worldPoints[i + 1];

            float elapsedTime = 0f;

            while (elapsedTime < _lerpTime)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / _lerpTime);

                Vector3 pos = InterpolationHermite(p0, p1, p2, p3, t);
                _riwa.transform.position = pos;

                Vector3 dir = (InterpolationHermite(p0, p1, p2, p3, t + 0.05f) - pos).normalized;
                if (dir.sqrMagnitude > 0.001f)
                    _riwa.transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));

                yield return null;
            }
        }

        _riwa.transform.position = worldPoints[worldPoints.Count - 1];
    }

    private Vector3 InterpolationHermite(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return 0.5f * (
            2f * p1 +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t
        );
    }

    private bool CheckDirection(CellPos pos)
    {
        if (_damier.ContainsKey(pos)) return true;
        else return false;
    }

    private bool IsTargetedCellTaken(CellPos target)
    {
        return _damier.ContainsKey(target) && _damier[target].cellState == CellState.NotBreakable;
    }

    bool IsAdjacentToEnd(CellPos a, CellPos b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        return (dx + dy) == 1;
    }

    private void OnCellTriggered(CellPos pos, Cell cell)
    {
        if(_damier.ContainsKey(pos) && _damier[pos].cellState == CellState.Breakable)
        {
            _damier[pos].cell.GetComponent<Rigidbody>().isKinematic = false;
            ChangeCellState(pos, CellState.Broken);
        }
    }

    private void ChangeCellState(CellPos pos, CellState state)
    {
        DamierDatas data = _damier[pos];
        data.SetCellState(state);
        _damier[pos] = data;
    }

    public void RespawnBrokenTile()
    {
        Floor1Room3LevelManager instance = (Floor1Room3LevelManager)Floor1Room3LevelManager.Instance;
        foreach (var cell in instance.BrokenCells)
        {
            if (_damier[cell.Position].cellState == CellState.Broken)
            {
                Vector3 respawnPosition = _damier[cell.Position].cell.GetComponent<Cell>().RespawnPosition;
                ChangeCellState(cell.Position, CellState.Breakable);
                _damier[cell.Position].cell.GetComponent<Rigidbody>().isKinematic = true;
                _damier[cell.Position].cell.transform.position = respawnPosition;
            }
        }

    }
}