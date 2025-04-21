using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Floor1Room4LevelManager : BaseLevelManager
{
    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera _stairCamera;
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

    private bool _isTutorialDone = false;

    public CinemachineVirtualCamera StairCamera { get => _stairCamera; }
    public CinemachineVirtualCamera SensaRiwaDiscussing { get => _sensaRiwaDiscussing; }
    public List<CinemachineVirtualCamera> FresqueCameras { get => _fresqueCameras; }
    public GameObject Chawa { get => _chawa; }
    public TutorialDialogRoom4Manager DialogManager { get => _dialogManager; }
    public bool IsTutorialDone { get => _isTutorialDone; set => _isTutorialDone = value; }
    public MuralPiece MuralPiece { get => _muralPiece; }
    public Transform SensaLandingTransform { get => _sensaLandingTransform; }
    public Transform RiwaLandingTransform { get => _riwaLandingTransform; }

    private void OnEnable()
    {
        base.OnEnable();
        _rootCollider.OnGrowthPercentageReached += PlayerCanInteractWithSocle;
        _rootCollider.OnGrowthPercentageUnreached += PlayerCannotInteractWithSocle;
    }

    private void OnDisable()
    {
        _rootCollider.OnGrowthPercentageReached -= PlayerCanInteractWithSocle;
        _rootCollider.OnGrowthPercentageUnreached -= PlayerCannotInteractWithSocle;
    }

    private void PlayerCanInteractWithSocle()
    {
        _treeStumpTest.enabled = true;
    }    
    
    private void PlayerCannotInteractWithSocle()
    {
        _treeStumpTest.enabled = false;
    }
}
