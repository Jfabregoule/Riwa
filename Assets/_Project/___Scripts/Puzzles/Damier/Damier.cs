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
    /// <summary>
    /// TO DO :
    /// Each cell should know if its breakable or not
    /// Also when the player is on it, should break if breakable
    /// Generate a good path each Start (or Awake)
    /// </summary>
    ///

    [Header("Debug")]
    [SerializeField] private Material _blue;
    [SerializeField] private Material _white;
    [SerializeField] private Material _green;

    [Header("Damier")]
    [SerializeField] private int _damierSize = 6;

    [HideInInspector][SerializeField] List<DamierDatas> serializedDamier = new List<DamierDatas>(); // Delete when Save is done
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
        CellPos start = new CellPos(0, 0);
        CellPos end = new CellPos(_damierSize - 1, _damierSize - 1);

        List<CellPos> path = new List<CellPos>();
        List<CellPos> possibleMoves = new List<CellPos>();
        CellPos current = start;
        path.Add(current);

        System.Random random = new System.Random();

        while (!current.Equals(end))
        {
            possibleMoves.Clear();
            if (_damier[current] == _damier[start])
            {
                path.Add(current);
                _damier[current] = false;
            }

            CellPos right = new CellPos(current.x + 1, current.y);
            CellPos left = new CellPos(current.x - 1, current.y);
            CellPos up = new CellPos(current.x, current.y + 1);
            CellPos down = new CellPos(current.x, current.y - 1);

            if (_damier.ContainsKey(right) && _damier[current] != _damier[right])
                possibleMoves.Add(right);

            if (_damier.ContainsKey(left) && _damier[current] != _damier[left])
                possibleMoves.Add(left);

            if (_damier.ContainsKey(up) && _damier[current] != _damier[up])
                possibleMoves.Add(up);

            if (_damier.ContainsKey(down) && _damier[current] != _damier[down])
                possibleMoves.Add(down);

            if (possibleMoves.Count == 0)
            {
                Debug.Log("No more possible moves");
                break;
            }

            int randomIndex = random.Next(0, possibleMoves.Count);
            path.Add(possibleMoves[randomIndex]);
            current = possibleMoves[randomIndex];
            _damier[current] = false;

        }

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
}
