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
                cell.transform.position = new Vector3(x * cellSize, 0, y * cellSize);
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

        Vector2Int up = new Vector2Int(0, 1);
        Vector2Int down = new Vector2Int(0, -1);
        Vector2Int right = new Vector2Int(1, 0);
        Vector2Int left = new Vector2Int(-1, 0);

        List<CellPos> possibleMoves = new List<CellPos>();
        path.Add(start);
        path.Add(new CellPos(0, 1));

        _damier[start] = false;
        _damier[new CellPos(0, 1)] = false;
        _damier[end] = false;

        int pathStart = Random.Range(0, path.Count);
        CellPos current = path[pathStart];
        //CellPos current = new CellPos(0, 1);

        Debug.Log("BASE - X: " + current.x + ", " + current.y);

        List<CellPos> existingNeighbors = new List<CellPos>();

        System.Random random = new System.Random();

        int upCounter = 0;
        int downCounter = 0;
        int rightCounter = 0;
        //int leftCounter = 0;

        while (current != end)
        {
            existingNeighbors.Clear();
            possibleMoves.Clear();

            CellPos upFPos = GetCellPosition(current, up);
            CellPos rightFPos = GetCellPosition(current, right);

            if ((_damier.ContainsKey(upFPos) && upFPos == end) ||
                (_damier.ContainsKey(rightFPos) && rightFPos == end))
            {
                path.Add(current);
                _damier[current] = false;
                break;
            }

            if (CheckDirection(current, right) && rightCounter != 2) existingNeighbors.Add(GetCellPosition(current, right));
            if (CheckDirection(current, down) && downCounter != 2) existingNeighbors.Add(GetCellPosition(current, down));
            if (CheckDirection(current, up) && upCounter != 2) existingNeighbors.Add(GetCellPosition(current, up));
            //if (CheckDirection(current, left) && leftCounter != 2) existingNeighbors.Add(GetCellPosition(current, left));

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

                CellPos rightPos = GetCellPosition(index, right);
                CellPos upPos = GetCellPosition(index, up);
                CellPos downPos = GetCellPosition(index, down);

                if (rightPos != end && IsDirectionBreakable(index, right)) blockedNeighborCount++;
                if (upPos != end && IsDirectionBreakable(index, up)) blockedNeighborCount++;
                if (downPos != end && IsDirectionBreakable(index, down)) blockedNeighborCount++;

                if (blockedNeighborCount >= 2)
                {
                    possibleMoves.RemoveAt(j);
                }
            }

            for (int i = 0; i < possibleMoves.Count; i++)
            {
                CellPos move = possibleMoves[i];

                // Vérifie si la position a déjà été ajoutée
                if (path.Contains(move)) continue;

                if (move == end || IsAdjacentToEnd(move, end))
                {
                    path.Add(move);
                    current = move;
                    _damier[current] = false;
                    Debug.Log("Path completed near end at: " + move.x + ", " + move.y);
                    break;
                }
            }

            if (possibleMoves.Count == 0)
            {
                Debug.LogWarning("No more possible moves from: " + current.x + ", " + current.y);
                break;
            }

            int moveIndex = random.Next(0, possibleMoves.Count);
            CellPos nextMove = possibleMoves[moveIndex];

            if (!path.Contains(nextMove))
            {
                path.Add(nextMove);
                current = nextMove;
                _damier[current] = false;
            }

            if (current == GetCellPosition(nextMove, up)) upCounter++;
            if (current == GetCellPosition(nextMove, right)) rightCounter++;
            if (current == GetCellPosition(nextMove, down)) downCounter++;
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
            CellPos pos = new CellPos((int)cell.position.x, (int)cell.position.z);

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

    private bool CheckDirection(CellPos pos, Vector2Int direction)
    {
        CellPos checkPos = new CellPos(pos.x + direction.x, pos.y + direction.y);
        if (_damier.ContainsKey(checkPos)) return true;
        else return false;
    }

    private CellPos GetCellPosition(CellPos pos, Vector2Int direction)
    {
        CellPos newCellPos = new CellPos(pos.x + direction.x, pos.y + direction.y);
        return newCellPos;
    }

    private bool IsDirectionBreakable(CellPos origin, Vector2Int dir)
    {
        CellPos target = GetCellPosition(origin, dir);
        return _damier.ContainsKey(target) && _damier[target] == false;
    }

    private bool IsAdjacentToEnd(CellPos move, CellPos end)
    {
        return (Mathf.Abs(move.x - end.x) + Mathf.Abs(move.y - end.y)) == 1;
    }
}