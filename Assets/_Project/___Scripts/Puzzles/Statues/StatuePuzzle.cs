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

[System.Serializable]
public struct Solution
{
    public CellPos position;
    public CellContent content;
}

public class StatuePuzzle : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] private Vector2Int _gridSize = new Vector2Int(7, 7);
    [SerializeField] private int _unitGridSize = 7;
    [SerializeField] private GameObject defaultTile;
    [SerializeField] private GameObject gridSpawnpoint;

    [Header("Tiles")]
    [SerializeField] private List<GameObject> tiles;

    [Header("Statues")]
    [SerializeField] private List<Statue> _statues;

    [Header("Debug")]
    [SerializeField] private List<GridEntry> serializedGrid = new List<GridEntry>();
    [SerializeField] private List<Solution> serializedSolutions = new List<Solution>();

    public int UnitGridSize => _unitGridSize;
    public Vector3 Origin { get; private set; }
    public Vector2Int GridSize => _gridSize;

    Dictionary<CellPos, CellContent> solution = new Dictionary<CellPos, CellContent>();
    Dictionary<CellPos, CellContent?> grid = new Dictionary<CellPos, CellContent?>();

    private void Start()
    {
        Origin = gridSpawnpoint.transform.position;
        foreach (Statue statue in _statues)
        {
            statue.OnStatueMoved += Move;
        }

        grid.Clear();
        foreach (var entry in serializedGrid)
        {
            grid[entry.position] = entry.content;
        }
        solution.Clear();
        foreach (var entry in serializedSolutions)
            solution.Add(entry.position, entry.content);
    }

    [ContextMenu("Generate Grid")]
    public void GenerateGrid()
    {

        solution.Add(new CellPos(0, 0), new CellContent(1, 135));    // ID 1 --> Blue Statue
        solution.Add(new CellPos(4, 1), new CellContent(2, 45));     // ID 2 --> Red Statue
        solution.Add(new CellPos(2, 2), new CellContent(3, -90));    // ID 3 --> Yellow Statue
        solution.Add(new CellPos(1, 4), new CellContent(4, 0));      // ID 4 --> Green Statue

        Origin = gridSpawnpoint.transform.position;

        for (int y = 0; y < _gridSize.y; y++)
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                if (y >= 3 && x >= 3)
                    continue;

                Vector3 position = Origin + new Vector3(x * _unitGridSize, 0f, y * _unitGridSize);

                foreach(var solution in solution)
                {
                    if(solution.Key.Equals(new CellPos(x, y)))
                    {
                        GameObject associatedStatueTile = tiles[solution.Value.id - 1];
                        GameObject st = Instantiate(associatedStatueTile, position, Quaternion.identity);
                        st.transform.SetParent(gridSpawnpoint.transform);
                        grid[new CellPos(x, y)] = solution.Value;
                        break;
                    }
                }

                GameObject tile = Instantiate(defaultTile, position, Quaternion.identity);
                tile.transform.SetParent(gridSpawnpoint.transform);
                grid[new CellPos(x, y)] = null;
            }
        }

        serializedGrid.Clear();
        foreach (var pair in grid)
        {
            serializedGrid.Add(new GridEntry { position = pair.Key, content = pair.Value });
        }

        serializedSolutions.Clear();
        foreach (var pair in solution)
            serializedSolutions.Add(new Solution { position =  pair.Key, content = pair.Value });
    }

    public void Check()
    {
        foreach(var pair in solution)
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
