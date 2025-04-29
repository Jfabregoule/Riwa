using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[System.Serializable]
public struct CellPos
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public CellPos(int posX, int posY) { x = posX; y = posY; }

    public void Init(int posX, int posY)
    {
        x = posX;
        y = posY;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is CellPos)) return false;
        CellPos other = (CellPos)obj;
        return x == other.x && y == other.y;
    }

    public override int GetHashCode()
    {
        return x * 397 ^ y;
    }

    public static CellPos operator +(CellPos a, CellPos b)
    {
        return new CellPos(a.x + b.x, a.y + b.y);
    }

    public static CellPos operator -(CellPos a, CellPos b)
    {
        return new CellPos(a.x - b.x, a.y - b.y);
    }

    public static bool operator ==(CellPos a, CellPos b)
    {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(CellPos a, CellPos b)
    {
        return !(a == b);
    }

    public CellPos GetCellAtDirection(Direction direction)
    {
        switch(direction)
        {
            case Direction.Up:
                return new CellPos(x, y + 1);
            case Direction.Down:
                return new CellPos(x, y - 1);
            case Direction.Left:
                return new CellPos(x - 1, y);
            case Direction.Right:
                return new CellPos(x + 1, y);
            default:
                return this;
        }
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

[System.Serializable]
public struct StatueData
{
    public int id;
    public int rotation;
    public float unitGridSize;
    public int posX;
    public int posY;
}

[System.Serializable]
public struct Datas
{
    public Statue statue;
    public StatueData data;
}

public class Grid : MonoBehaviour, IActivable
{
    [Header("Grid")]
    [SerializeField] private Vector2Int _gridSize = new Vector2Int(5, 5);
    [SerializeField] private float _unitGridSize = 2.5f;
    [SerializeField] private GameObject _defaultTile;
    [SerializeField] private GameObject _stair;
    [SerializeField] private MovingPlatform _finalMP;
    [Header("Debug")]
    [SerializeField] private bool _showDebug = false;

    /// <summary>
    /// Specify the statue prefab before Generating grid then specify the new generated ones
    /// </summary>
    [Header("Statues")]
    [SerializeField] private List<GameObject> _statuesPrefab;

    /// <summary>
    /// Save system without save system o_o
    /// Remove them when the save system is up
    /// </summary>
    [HideInInspector][SerializeField] private List<GridEntry> serializedGrid = new List<GridEntry>();
    [HideInInspector][SerializeField] private List<Solution> serializedSolutions = new List<Solution>();
    [HideInInspector][SerializeField] private List<Datas> serializedStatues = new List<Datas>();

    [SerializeField] private List<Statue> _statues = new List<Statue>();

    Dictionary<CellPos, CellContent> solution = new Dictionary<CellPos, CellContent>();
    Dictionary<CellPos, CellContent?> grid = new Dictionary<CellPos, CellContent?>();

    Dictionary<Statue, StatueData> statueData = new Dictionary<Statue, StatueData>();

    private Floor1Room4LevelManager _room4LevelManager;
    private bool _isGridActivated = false;

    public event IActivable.ActivateEvent OnActivated;
    public event IActivable.ActivateEvent OnDesactivated;

    public float UnitGridSize => _unitGridSize;
    public Vector3 Origin { get; private set; }
    public Vector2Int GridSize => _gridSize;
    public Dictionary<CellPos, CellContent> Solution { get => solution; set => solution = value; }
    public bool IsGridActivated { get => _isGridActivated; set => _isGridActivated = value; }

    public void OnEnable()
    {
        LoadData();
        SaveSystem.Instance.OnLoadProgress += LoadData;
    }

    private void OnDisable()
    {
        SaveSystem.Instance.OnLoadProgress -= LoadData;

        SerializableVector3 statuePosition;
        SerializableVector3 statueRotation;

        for (int i = 0; i < _statues.Count; i++)
        {
            if (_statues[i].IsMoving) continue;
            statuePosition = new SerializableVector3(_statues[i].transform.position);
            SaveSystem.Instance.SaveElement<SerializableVector3>($"StatuePosition{i}", statuePosition);
            statueRotation = new SerializableVector3(_statues[i].transform.rotation.eulerAngles);
            SaveSystem.Instance.SaveElement<SerializableVector3>($"StatueRotation{i}", statueRotation);
        }
    }

    private void LoadData()
    {
        statueData.Clear();
        for (int i = 0; i < _statues.Count; i++)
        {
            if (SaveSystem.Instance.ContainsElements($"StatuePosition{i}"))
                _statues[i].transform.position = SaveSystem.Instance.LoadElement<SerializableVector3>($"StatuePosition{i}").ToVector3();
            if (SaveSystem.Instance.ContainsElements($"StatueRotation{i}"))
                _statues[i].transform.rotation = Quaternion.Euler(SaveSystem.Instance.LoadElement<SerializableVector3>($"StatueRotation{i}").ToVector3());
            if (SaveSystem.Instance.ContainsElements($"StatueDatas{i}"))
            {
                statueData.Add(_statues[i], SaveSystem.Instance.LoadElement<StatueData>($"StatueDatas{i}"));
                StatueData data = statueData[_statues[i]];
                _statues[i].SetStatuesData(data);
                _statues[i].OnStatueMoved += Move;
                _statues[i].OnStatueRotate += UpdateStatueRotation;
                _statues[i].OnStatueEndMoving += Check;
                CellPos pos = new CellPos(data.posX, data.posY);
                grid[pos] = new CellContent(data.id, data.rotation);
            }
        }
    }

    private void Awake()
    {
        Origin = transform.position;

        solution.Clear();
        foreach (var entry in serializedSolutions)
            solution.Add(entry.position, entry.content);

        grid.Clear();
        foreach (var entry in serializedGrid)
            grid[entry.position] = entry.content;

        statueData.Clear();
        foreach (var entry in serializedStatues)
            statueData.Add(entry.statue, entry.data);

        foreach (var pair in statueData)
        {
            pair.Key.SetStatuesData(pair.Value);
            pair.Key.OnStatueMoved += Move;
            pair.Key.OnStatueRotate += UpdateStatueRotation;
            pair.Key.OnStatueEndMoving += Check;
            CellPos pos = new CellPos(pair.Value.posX, pair.Value.posY);
            grid[pos] = new CellContent(pair.Value.id, pair.Value.rotation);
        }

    }

    private void Start()
    {
        //if(_isGridActivated == false) SaveSystem.Instance.SaveElement<bool>("GridActivated", false);
        //if (_isGridActivated == true) _finalMP.StartMoving();
        _room4LevelManager = (Floor1Room4LevelManager)Floor1Room4LevelManager.Instance;
    }

    [ContextMenu("Generate Grid")]
    public void GenerateGrid()
    {
        solution.Clear();
        solution.Add(new CellPos(0, 4), new CellContent(1, 225));     // ID 1 --> Blue Statue
        solution.Add(new CellPos(1, 0), new CellContent(2, 135));     // ID 2 --> Orange Statue
        solution.Add(new CellPos(2, 2), new CellContent(3, 0));       // ID 3 --> Purple Statue
        solution.Add(new CellPos(4, 3), new CellContent(4, 90));      // ID 4 --> Green Statue

        Origin = transform.position;

        for (int y = 0; y < _gridSize.y; y++)
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                //if (y >= 3 && x >= 3)
                //    continue;

                Vector3 position = Origin + new Vector3(x * _unitGridSize, 0f, y * _unitGridSize);

                foreach(var solution in solution)
                {
                    if(solution.Key.Equals(new CellPos(x, y)))
                    {
                        GameObject st = Instantiate(_defaultTile, position, Quaternion.identity);
                        st.name = "Tile Statue: " + solution.Value.id;
                        st.transform.SetParent(transform);
                        grid[new CellPos(solution.Key.x, solution.Key.y)] = solution.Value;
                        break;
                    }
                }

                if (grid.ContainsKey(new CellPos(x, y)) && grid[new CellPos(x, y)] != null) continue;
                GameObject tile = Instantiate(_defaultTile, position, Quaternion.identity);
                tile.name = "Tile " + x + ", " + y;
                tile.transform.SetParent(transform);
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
        _statues.Clear();
        serializedStatues.Clear();
        int index = 0;
        for (int i = 0; i < _statuesPrefab.Count; i++)
        {
            index++;
            int randX, randY;
            do
            {
                randX = Random.Range(0, _gridSize.x);
                randY = Random.Range(0, _gridSize.y);
            }
            while (randX >= 3 && randY >= 3);
            int randRotation = Random.Range(0, 8) * 45;
            Quaternion rot = Quaternion.Euler(0, randRotation, 0);
            Vector3 position = Origin + new Vector3(randX * _unitGridSize, 0.09f, randY * _unitGridSize);
            GameObject statueGO = Instantiate(_statuesPrefab[index - 1], position, rot);
            statueGO.transform.SetParent(transform);

            Statue statue = statueGO.GetComponent<Statue>();
            _statues.Add(statue);

            serializedStatues.Add(new Datas { statue = statue, data = new StatueData { id = index, rotation = (int)randRotation, unitGridSize = _unitGridSize, posX = randX, posY = randY } });
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
            //SaveSystem.Instance.SaveElement<bool>("GridActivated", true);
            _isGridActivated = true;
            foreach (Statue statues in _statues)
            {
                statues.Validate = true;
            }
            GameManager.Instance.Character.StateMachine.GoToIdle();
            StartCoroutine(ActiveElevator());
            if (_showDebug == true) Debug.Log("Grille complétée avec succès !");
        }
        else
        {
            _isGridActivated = false;
            if (_showDebug == true) Debug.Log("La grille n'est pas encore correctement remplie.");
        }
    }

    private IEnumerator ActiveElevator()
    {

        yield return new WaitForSeconds(2.5f);

        _room4LevelManager.ElevatorCamera.Priority = 20;
        InputManager.Instance.DisableGameplayControls();
        Activate();

        yield return new WaitForSeconds(3f);

        _room4LevelManager.ElevatorCamera.Priority = 0;
        InputManager.Instance.EnableGameplayControls();
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

    public void Activate()
    {
        OnActivated?.Invoke();
    }

    public void Deactivate()
    {
        OnDesactivated?.Invoke();
    }
}
