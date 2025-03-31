using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct CellPos
{
    public int x;
    public int y;
}

[System.Serializable]
struct CellContent
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

    public int UnitGridSize => _unitGridSize;
    public Vector3 Origin { get; private set; }
    public Vector2Int GridSize => _gridSize;

    Dictionary<CellPos, CellContent> Res;
    CellContent?[,] grid = new CellContent?[7, 3];

    private void Awake()
    {
        Origin = gridSpawnpoint.transform.position;
        //GenerateGrid();
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

    private bool IsCellEmpty(CellPos pos)
    {
        return (grid[pos.x, pos.y] == null);
    }
}
