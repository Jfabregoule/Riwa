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
    [Header("Debug")]
    [SerializeField] private bool _showDebug = false;

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

    private void Awake()
    {
        Origin = gridSpawnpoint.transform.position;

        solution.Clear();
        foreach (var entry in serializedSolutions)
            solution.Add(entry.position, entry.content);

        grid.Clear();
        foreach (var entry in serializedGrid)
            grid[entry.position] = entry.content;

        foreach (Statue statue in _statues)
        {
            statue.OnStatueMoved += Move;
            statue.OnStatueRotate += UpdateStatueRotation;
            statue.OnStatueEndMoving += Check;
        }
    }

    [ContextMenu("Generate Grid")]
    public void GenerateGrid()
    {
        solution.Clear();
        solution.Add(new CellPos(0, 0), new CellContent(1, 135));    // ID 1 --> Blue Statue
        solution.Add(new CellPos(4, 1), new CellContent(2, 45));     // ID 2 --> Red Statue
        solution.Add(new CellPos(2, 2), new CellContent(3, 270));    // ID 3 --> Yellow Statue
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
                        grid[new CellPos(solution.Key.x, solution.Key.y)] = solution.Value;
                        break;
                    }
                }

                if (grid.ContainsKey(new CellPos(x, y)) && grid[new CellPos(x, y)] != null) continue;
                GameObject tile = Instantiate(defaultTile, position, Quaternion.identity);
                tile.transform.SetParent(gridSpawnpoint.transform);
                grid[new CellPos(x, y)] = null;
            }
        }

        serializedGrid.Clear();
        foreach (var pair in grid)
            serializedGrid.Add(new GridEntry { position = pair.Key, content = pair.Value });

        serializedSolutions.Clear();
        foreach (var pair in solution)
            serializedSolutions.Add(new Solution { position =  pair.Key, content = pair.Value });

        GenerateStatue();
    }

    public void GenerateStatue()
    {
        int index = 0;
        foreach(Statue statues in _statues)
        {
            index++;
            int randX, randY;
            do
            {
                randX = Random.Range(0, _gridSize.x);
                randY = Random.Range(0, _gridSize.y);
            }
            while (randX >= 3 && randY >= 3);
            int randRotation = Random.Range(0, 8);
            Quaternion rot = Quaternion.Euler(0, randRotation * 45, 0);
            Vector3 position = Origin + new Vector3(randX * _unitGridSize, 1, randY * _unitGridSize);
            _statues[index - 1].SetStatuesData(index, randRotation * 45, _unitGridSize, randX, randY);
            GameObject statue = Instantiate(_statues[index - 1].gameObject, position, rot);
            statue.transform.SetParent(gridSpawnpoint.transform);
        }
    }

    public void PlaceStatueData(CellPos pos, CellContent statueData)
    {
        grid[pos] = statueData;
    }

    public void Check()
    {
        bool isGridComplete = true;

        foreach (var pair in solution)
        {
            if (!grid.TryGetValue(pair.Key, out CellContent? content) || !content.HasValue)
            {
                if(_showDebug == true) Debug.Log($"Erreur: Aucun élément trouvé à la position {pair.Key.x}, {pair.Key.y}, Statue attendue: {pair.Value.id}.");
                isGridComplete = false;
                continue;
            }

            if (content.Value.id == pair.Value.id && content.Value.rotation == pair.Value.rotation)
            {
                if (_showDebug == true) Debug.Log($"Statue {pair.Value.id} bien placée en {pair.Key.x}, {pair.Key.y} avec rotation {pair.Value.rotation}.");
            }
            else
            {
                if (_showDebug == true) Debug.Log($"Erreur: Statue {content.Value.id} mal orientée ou mal placée en {pair.Key.x}, {pair.Key.y}. Rotation attendue: {pair.Value.rotation}, actuelle: {content.Value.rotation}.");
                isGridComplete = false;
            }
        }

        if (isGridComplete)
        {
            foreach(Statue statues in _statues)
            {
                statues.Validate = true;
            }
            if (_showDebug == true) Debug.Log("Grille complétée avec succès !");
        }
        else
            if (_showDebug == true) Debug.Log("La grille n'est pas encore correctement remplie.");
    }


    public bool Move(CellPos oldPos, Vector2Int direction, CellContent statueData)
    {
        CellPos newPos = new CellPos(oldPos.x + direction.x, oldPos.y + direction.y);

        if (!grid.ContainsKey(newPos))
        {
            if (_showDebug == true) Debug.Log($"Mouvement impossible : {newPos.x}, {newPos.y} est hors de la grille.");
            return false;
        }
        if (!IsCellEmpty(newPos))
        {
            if (_showDebug == true) Debug.Log($"Mouvement impossible : La case {newPos.x}, {newPos.y} est déjà occupée !");
            return false;
        }

        grid[oldPos] = null;
        grid[newPos] = statueData;

        if (_showDebug == true) Debug.Log($"Statue déplacée en {newPos.x}, {newPos.y}");
        return true;
    }

    public void UpdateStatueRotation(CellPos pos, CellContent statueData)
    {
        grid[pos] = statueData;
    }

    private bool IsCellEmpty(CellPos pos)
    {
        bool isEmpty = !grid.ContainsKey(pos) || !grid[pos].HasValue;
        if (_showDebug == true) Debug.Log($"Vérification case ({pos.x}, {pos.y}) -> Est vide ? {isEmpty} | Contenu : {grid[pos]}");
        return isEmpty;
    }
}
