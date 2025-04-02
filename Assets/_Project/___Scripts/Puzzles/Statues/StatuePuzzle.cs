using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CellPos
{

    public CellPos(int posX, int posY) { x = posX; y = posY; }

    public void Init(int posX, int posY)
    {
        x = posX;
        y = posY;
    }

    public int x;
    public int y;
}

[System.Serializable]
public struct CellContent
{
    public CellContent(int newID, int newRotation) { id = newID; rotation = newRotation; }

    public void Init(int newID, int newRotation)
    {
        id = newID;
        rotation = newRotation;
    }

    public int id;
    public int rotation;
}

[System.Serializable]
public struct GridEntry
{
    public CellPos position;
    public CellContent? content;
}

public class StatuePuzzle : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] private Vector2Int _gridSize = new Vector2Int(7, 7);
    [SerializeField] private int _unitGridSize = 7;
    [SerializeField] private GameObject tiles;
    [SerializeField] private GameObject gridSpawnpoint;
    [SerializeField] private GameObject blueTileGO;
    [SerializeField] private List<Statue> _statues;

    [SerializeField] private List<GridEntry> serializedGrid = new List<GridEntry>();

    public int UnitGridSize => _unitGridSize;
    public Vector3 Origin { get; private set; }
    public Vector2Int GridSize => _gridSize;

    Dictionary<CellPos, CellContent> Res = new Dictionary<CellPos, CellContent>();
    CellContent?[,] gridBis = new CellContent?[7, 7];
    Dictionary<CellPos, CellContent?> grid = new Dictionary<CellPos, CellContent?>();

    private void Start()
    {
        Origin = gridSpawnpoint.transform.position;
        foreach (Statue statue in _statues)
        {
            statue.OnStatueMoved += Move;
        }

        Res.Add(new CellPos(0, 0), new CellContent(1, 135));

        grid.Clear();
        foreach (var entry in serializedGrid)
        {
            grid[entry.position] = entry.content;
        }
    }

    [ContextMenu("Generate Grid")]
    public void GenerateGrid()
    {
        Origin = gridSpawnpoint.transform.position;
        int blueTile = Random.Range(0, _gridSize.x * _gridSize.y);
        for (int y = 0; y < _gridSize.y; y++)
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                if (y >= 3 && x >= 4)
                {
                    continue;
                }
                int index = y * _gridSize.x + x;
                Vector3 position = Origin + new Vector3(x * _unitGridSize, 0f, y * _unitGridSize);
                if (index == blueTile)
                {
                    Debug.Log("RESULT POS: " + index);
                    GameObject bt = Instantiate(blueTileGO, position, Quaternion.identity);
                    bt.transform.SetParent(gridSpawnpoint.transform);
                    grid[new CellPos(x, y)] = new CellContent(1, 135);
                }
                else
                {
                    GameObject tile = Instantiate(tiles, position, Quaternion.identity);
                    tile.transform.SetParent(gridSpawnpoint.transform);
                    //grid[new CellPos(x, y)] = null;
                    grid.Add(new CellPos(x, y), null);
                }
            }
        }

        serializedGrid.Clear();
        foreach (var pair in grid)
        {
            serializedGrid.Add(new GridEntry { position = pair.Key, content = pair.Value });
        }
    }

    public void Check()
    {
        foreach(var pair in Res)
        {
            if (grid[pair.Key] != null)
            {
                CellContent value = pair.Value;
                if (grid[pair.Key].Equals(value))
                    Debug.Log("Grid completed");
                else
                    Debug.Log("Grid not completed");
            }
            
        }
    }

    public bool Move(CellPos oldPos, Vector2Int direction, CellContent statueData)
    {
        CellPos newPos = new CellPos(oldPos.x + direction.x, oldPos.y + direction.y);

        bool test = grid.ContainsKey(newPos);
        Debug.Log(test);
        if (!grid.ContainsKey(newPos)) return false;
        if (!IsCellEmpty(newPos)) return false;

        grid[oldPos] = null;
        grid[newPos] = statueData;

        return true;
    }

    public void UpdateStatueRotation(CellPos pos, CellContent statueData)
    {
        grid[pos] = statueData;
    }

    private bool IsCellEmpty(CellPos pos)
    {
        return (grid[pos] == null);
    }
}
