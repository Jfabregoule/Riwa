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

public class StatuePuzzle : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] private Vector2Int _gridSize = new Vector2Int(7, 3);
    [SerializeField] private int _unitGridSize = 7;
    [SerializeField] private GameObject tiles;
    [SerializeField] private GameObject gridSpawnpoint;
    [SerializeField] private GameObject blueTileGO;
    [SerializeField] private List<Statue> _statues;

    public int UnitGridSize => _unitGridSize;
    public Vector3 Origin { get; private set; }
    public Vector2Int GridSize => _gridSize;

    Dictionary<CellPos, CellContent> Res = new Dictionary<CellPos, CellContent>();
    CellContent?[,] grid = new CellContent?[7, 3];

    private void Start()
    {
        Origin = gridSpawnpoint.transform.position;
        foreach (Statue statue in _statues)
        {
            statue.OnStatueMoved += Move;
        }

        Res.Add(new CellPos(0, 0), new CellContent(1, 135));
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
                int index = y * _gridSize.x + x;
                Vector3 position = Origin + new Vector3(x * _unitGridSize, 0f, y * _unitGridSize);
                if (index == blueTile)
                {
                    Debug.Log("RESULT POS: " + index);
                    GameObject bt = Instantiate(blueTileGO, position, Quaternion.identity);
                    bt.transform.SetParent(gridSpawnpoint.transform);
                    grid[x, y] = new CellContent(1, 135);
                }
                else
                {
                    GameObject tile = Instantiate(tiles, position, Quaternion.identity);
                    tile.transform.SetParent(gridSpawnpoint.transform);
                    grid[x, y] = null;
                }
            }
        }
    }

    public void Check()
    {
        foreach(var pair in Res)
        {
            if (grid[pair.Key.x, pair.Key.y] != null)
            {
                CellContent value = pair.Value;
                if (value.Equals(grid[pair.Key.x, pair.Key.y]))
                    Debug.Log("Grid completed");
                else
                    Debug.Log("Grid not completed");
            }
            
        }
    }

    public bool Move(CellPos oldPos, Vector2Int direction, CellContent statueData)
    {
        CellPos newPos = new CellPos(oldPos.x + direction.x, oldPos.y + direction.y);
        if (newPos.x < 0
            || newPos.x > _gridSize.x - 1
            || newPos.y < 0
            || newPos.y > _gridSize.y - 1)
            return false;

        if (!IsCellEmpty(newPos)) return false;


        grid[oldPos.x, oldPos.y] = null;
        grid[newPos.x, newPos.y] = statueData;

        return true;
    }

    public void UpdateStatueRotation(CellPos pos, CellContent statueData)
    {
        grid[pos.x, pos.y] = statueData;
    }

    private bool IsCellEmpty(CellPos pos)
    {
        return (grid[pos.x, pos.y] == null);
    }

}
