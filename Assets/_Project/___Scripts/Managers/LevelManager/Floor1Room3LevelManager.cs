using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Floor1Room3LevelManager : BaseLevelManager
{

    #region Fields

    [Header("Damier")]
    [SerializeField] private CinemachineVirtualCamera _damierCamera;

    private bool _isDamierCompleted = false;

    [Header("Liana")]
    [SerializeField] private TreeStumpTest _treeStumpTest;

    [Header("Tutorial Camera")]
    [SerializeField] private List<CinemachineVirtualCamera> _riwaSensaCamera;
    [SerializeField] private List<CinemachineVirtualCamera> _vinesCameras;

    [Header("Chawa")]
    [SerializeField] private GameObject _chawa;
    [SerializeField] private ParticleSystem _chawaTrail;
    [SerializeField] private Transform _chawaLianaPosition;
    [SerializeField] private BoxCollider _chawaPathTriggerZone;
    
    private RiwaShowingPathTriggerZone _riwaShowingPathTriggerZone;
    private bool _playerHasChangedTemporality;

    [Header("Dialogue Manager")]
    [SerializeField] private TutorialRoom3Manager _tutorialRoom3Manager;

    private List<Cell> _brokenCells = new List<Cell>();

    #endregion

    #region Properties

    public List<Cell> BrokenCells { get => _brokenCells; set => _brokenCells = value; }
    public CinemachineVirtualCamera DamierCamera { get => _damierCamera; }
    public List<CinemachineVirtualCamera> RiwaSensaCamera { get => _riwaSensaCamera; }
    public List<CinemachineVirtualCamera> VinesCameras { get => _vinesCameras; }
    public GameObject Chawa { get => _chawa; set => _chawa = value; }
    public ParticleSystem ChawaTrail { get => _chawaTrail; set => _chawaTrail = value; }
    public TutorialRoom3Manager TutorialRoom3Manager { get => _tutorialRoom3Manager; }
    public bool IsDamierCompleted { get => _isDamierCompleted; set => _isDamierCompleted = value; }
    public Transform ChawaLianaPosition { get => _chawaLianaPosition; set => _chawaLianaPosition = value; }
    public BoxCollider ChawaPathTriggerZone { get => _chawaPathTriggerZone; set => _chawaPathTriggerZone = value; }
    public TreeStumpTest TreeStumpTest { get => _treeStumpTest; set => _treeStumpTest = value; }
    public RiwaShowingPathTriggerZone RiwaShowingPathTriggerZone { get => _riwaShowingPathTriggerZone; set => _riwaShowingPathTriggerZone = value; }

    #endregion

    #region Events

    public delegate void RiwaShowingPath();
    public event RiwaShowingPath OnRiwaShowingPath;

    public delegate void PlayerCompletedDamier();
    public event PlayerCompletedDamier OnPlayerCompletedDamier;

    #endregion

    public void InvokeOnRiwaShowingPath() { OnRiwaShowingPath?.Invoke(); }
    public void InvokeOnPlayerCompletedDamier() 
    {
        if(IsDamierCompleted == false)
        {
            IsDamierCompleted = true;
            _chawaPathTriggerZone.enabled = false;
            OnPlayerCompletedDamier?.Invoke(); 
        }
    }

    public override void Start()
    {
        base.Start();
        GameManager.Instance.OnTimeChangeStarted += PlayerGoesInPast;
        _treeStumpTest.enabled = false;
        _treeStumpTest.CanInteract = false;
        _riwaShowingPathTriggerZone = _chawa.GetComponentInChildren<RiwaShowingPathTriggerZone>();
        GameManager.Instance.UnlockChangeTime();
    }

    private void OnDisable()
    {
        if(GameManager.Instance != null)
            GameManager.Instance.OnTimeChangeStarted -= PlayerGoesInPast;
    }

    private void PlayerGoesInPast(EnumTemporality temporality)
    {
        if (temporality == EnumTemporality.Past && _playerHasChangedTemporality == false)
        {
            _playerHasChangedTemporality = true;
            GameManager.Instance.UIManager.StopPulse(UIElementEnum.ChangeTime);
        }
    }

}
