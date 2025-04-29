using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[System.Serializable]
public struct MuralPieceData
{
    public MuralPiece MuralPiece;
    public EnumTemporality Temporality;
}

public class Floor1Room4LevelManager : BaseLevelManager
{
    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera _elevatorCamera;
    [SerializeField] private CinemachineVirtualCamera _sensaRiwaDiscussing;
    [SerializeField] private List<CinemachineVirtualCamera> _fresqueCameras;

    [Header("GameObjects")]
    [SerializeField] private GameObject _chawa;
    [SerializeField] private MuralPiece _muralPiece;

    [Header("Values")]
    [SerializeField] private Transform _sensaLandingTransform;
    [SerializeField] private Transform _riwaLandingTransform;

    [Header("Managers")]
    [SerializeField] private TutorialDialogRoom4Manager _dialogManager;

    [Header("Puzzle")]
    [SerializeField] private VineRetraction _rootCollider;
    [SerializeField] TreeStumpTest _treeStumpTest;
    [SerializeField] private List<MuralPiece> _muralPieces;
    [SerializeField] private CinemachineVirtualCamera _completedFresqueCamera;
    
    private Dictionary<MuralPieceData, bool> _fresqueCompletion = new Dictionary<MuralPieceData, bool>();

    private bool _isTutorialDone = false;

    public CinemachineVirtualCamera ElevatorCamera { get => _elevatorCamera; }
    public CinemachineVirtualCamera SensaRiwaDiscussing { get => _sensaRiwaDiscussing; }
    public List<CinemachineVirtualCamera> FresqueCameras { get => _fresqueCameras; }
    public GameObject Chawa { get => _chawa; }
    public TutorialDialogRoom4Manager DialogManager { get => _dialogManager; }
    public bool IsTutorialDone { get => _isTutorialDone; set => _isTutorialDone = value; }
    public MuralPiece MuralPiece { get => _muralPiece; }
    public Transform SensaLandingTransform { get => _sensaLandingTransform; }
    public Transform RiwaLandingTransform { get => _riwaLandingTransform; }
    public Dictionary<MuralPieceData, bool> FresqueCompletionData { get => _fresqueCompletion; set => _fresqueCompletion = value; }

    public override void OnEnable()
    {
        base.OnEnable();
        LoadData();
        SaveSystem.Instance.OnLoadProgress += LoadData;
        _rootCollider.OnGrowthPercentageReached += PlayerCanInteractWithSocle;
        _rootCollider.OnGrowthPercentageUnreached += PlayerCannotInteractWithSocle;
    }

    private void OnDisable()
    {
        SaveSystem.Instance.OnLoadProgress -= LoadData;
        SaveSystem.Instance.SaveElement<bool>("Room4TutorialDone", _isTutorialDone);
        _rootCollider.OnGrowthPercentageReached -= PlayerCanInteractWithSocle;
        _rootCollider.OnGrowthPercentageUnreached -= PlayerCannotInteractWithSocle;
    }

    private void LoadData()
    {
        _isTutorialDone = SaveSystem.Instance.LoadElement<bool>("Room4TutorialDone");
    }

    private void Start()
    {
        GameManager.Instance.UnlockChangeTime();
    }

    public void FillMuralPieceDictionary()
    {
        foreach (MuralPiece piece in _muralPieces)
        {
            _fresqueCompletion.Add(new MuralPieceData() { MuralPiece = piece, Temporality = piece.PieceTemporality }, piece.IsPiecePlaced);
            piece.OnPickUp += CheckFresqueTemporalityCompletion;
        }
    }

    private void PlayerCanInteractWithSocle()
    {
        _treeStumpTest.enabled = true;
    }    
    
    private void PlayerCannotInteractWithSocle()
    {
        _treeStumpTest.enabled = false;
    }

    public void CheckFresqueTemporalityCompletion(MuralPiece piece)
    {
        int pastPiecesPlaced = 0;
        int presentPiecesPlaced = 0;

        if(piece.PieceTemporality == EnumTemporality.Past)
        {
            MuralPiece presentPiece = piece.gameObject.GetComponent<TemporalItem>().PresentItem.GetComponent<MuralPiece>();
            StartCoroutine(presentPiece.PlacePieceOnFresque());
            ChangeFresqueCompletionData(presentPiece, EnumTemporality.Present);
        }
        
        foreach(var entry in _fresqueCompletion)
        {
            if(entry.Value == true)
            {
                if(entry.Key.Temporality == EnumTemporality.Past)
                    pastPiecesPlaced++;
                else if(entry.Key.Temporality == EnumTemporality.Present)
                    presentPiecesPlaced++;
            }
        }

        if (pastPiecesPlaced == 4 || presentPiecesPlaced == 4) StartCoroutine(SeeCompletedFresque());

    }

    private IEnumerator SeeCompletedFresque()
    {
        GameManager.Instance.Character.InputManager.DisableGameplayControls();
        _completedFresqueCamera.Priority = 50;
        yield return new WaitForSeconds(4f);
        GameManager.Instance.Character.InputManager.EnableGameplayControls();
        _completedFresqueCamera.Priority = 0;
    }

    public void ChangeFresqueCompletionData(MuralPiece piece, EnumTemporality temporality)
    {
        MuralPieceData key = new MuralPieceData() { MuralPiece = piece, Temporality = temporality };

        if(_fresqueCompletion.ContainsKey(key))
            _fresqueCompletion[key] = true;
    }
}
