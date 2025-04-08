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

public class Damier : MonoBehaviour
{

    [Header("Debug")]
    [SerializeField] private Material _blue;
    [SerializeField] private Material _white;
    [SerializeField] private Material _green;

    [Header("Damier")]
    [SerializeField] private int _damierSize = 6;

    [HideInInspector][SerializeField] List<DamierDatas> serializedDamier = new List<DamierDatas>(); // Delete when Save is done
    [SerializeField] List<CellPos> path = new List<CellPos>();
    Dictionary<CellPos, bool?> _damier = new Dictionary<CellPos, bool?>(); // This has to be saved later

    private void Awake()
    {
        _damier.Clear();
        foreach (var data in serializedDamier)
            _damier.Add(data.pos, data.breakable);
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
                GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cell.transform.position = transform.position + new Vector3(x * cellSize, 0, y * cellSize);
                cell.transform.localScale = new Vector3(cellSize, 0.1f, cellSize);
                cell.GetComponent<Renderer>().material = (x + y) % 2 == 0 ? _blue : _white;
                cell.transform.parent = transform;
                cell.name = $"Cell {x} {y}";
                _damier[new CellPos(x, y)] = null;
            }
        }

        serializedDamier.Clear();
        foreach(var pair in _damier)
            serializedDamier.Add(new DamierDatas() { pos = pair.Key, breakable = pair.Value });
    }

    public void GeneratePath()
    {
        path.Clear();

        CellPos start = new CellPos(0, 0);
        CellPos end = new CellPos(_damierSize - 1, _damierSize - 1);

        List<CellPos> possibleMoves = new List<CellPos>();
        List<CellPos> existingNeighbors = new List<CellPos>();

        path.Add(start);
        path.Add(new CellPos(0, 1));

        _damier[start] = false;
        _damier[new CellPos(0, 1)] = false;
        _damier[end] = false;

        int pathStart = Random.Range(0, path.Count);
        CellPos current = path[pathStart];

        System.Random random = new System.Random();

        bool stopPathGeneration = false;
        int upCounter = 0;
        int downCounter = 0;
        int rightCounter = 0;
        //int leftCounter = 0;

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
            //if (CheckDirection(current, left) && leftCounter != 2) existingNeighbors.Add(GetCellPosition(current, left));

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
                {
                    existingNeighbors.Remove(currentDown);
                }
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
            //if(current == GetCellPosition(nextMove, left)) leftCounter++;

            if (upCounter == 2) upCounter = 0;
            if (downCounter == 2) downCounter = 0;
            //if(leftCounter == 2) leftCounter = 0;
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
}