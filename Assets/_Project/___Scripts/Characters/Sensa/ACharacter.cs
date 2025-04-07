using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class ACharacter : MonoBehaviour
{
    #region Constantes

    //Constantes

    public const string PAWN_OBJECT = "Pawn";
    public const string SOUL_OBJECT = "Soul";
    public const string CAMERA_TARGET_OBJECT= "CameraTarget";
    public const string CAMERA_TARGET_PARENT_OBJECT= "CameraTargetParent";

    #endregion

    #region Fields

    //Field

    private GameObject _pawn;
    private StateMachineCharacter _fsmCharacter;
    private Animator _animator;
    private Rigidbody _rb;
    private CapsuleCollider _capsuleCollider;
    private InputManager _inputManager;
    private GameObject _soul;

    private GameObject _cameraTarget;
    private GameObject _cameraTargetParent;


    private bool _canInteract;
    private bool _canInteractSoul;

    private bool _isInPast = false;
    private ChangeTime _changeTime;

    [SerializeField] private LayerMask _pastLayer;
    [SerializeField] private LayerMask _presentLayer;

    private CameraHandler _cameraHandler;

    [Header("Gameplay Statistics")]

    [SerializeField] private float _speed = 1;
    [SerializeField] private float _joystickRunTreshold = 0.4f;

    [Header("StateMachine values")]

    [SerializeField] private float _timeBeforeWait = 5.0f;
    [SerializeField] private bool _isChangingTime = false;
    [SerializeField] private bool _isInSoul = false;

    [Header("VFX")]

    [SerializeField] private ParticleSystem _soulLinkVFX;

    #endregion

    #region Properties

    //Properties

    public GameObject Pawn { get => _pawn;}
    public StateMachineCharacter FsmCharacter { get => _fsmCharacter;}
    public Animator Animator { get => _animator;}
    public Rigidbody Rb { get => _rb;}
    public CapsuleCollider CapsuleCollider { get => _capsuleCollider; }
    public bool CanInteract { get => _canInteract; set => _canInteract = value; }
    public bool CanInteractSoul { get => _canInteractSoul; set => _canInteractSoul = value; }
    public float Speed { get => _speed; set => _speed = value; }
    public float JoystickRunTreshold { get => _joystickRunTreshold; set => _joystickRunTreshold = value; }
    public float TimeBeforeWait { get => _timeBeforeWait; set => _timeBeforeWait = value; }
    public bool IsChangingTime { get => _isChangingTime; set => _isChangingTime = value; }
    public bool IsInPast { get => _isInPast; set => _isInPast = value; }
    public ChangeTime ChangeTime { get => _changeTime; }
    public bool IsInSoul { get => _isInSoul; set => _isInSoul = value; }
    public GameObject Soul { get => _soul; set => _soul = value; }
    public ParticleSystem SoulLinkVFX { get => _soulLinkVFX; set => _soulLinkVFX = value; }
    public LayerMask PastLayer { get => _pastLayer;}
    public LayerMask PresentLayer { get => _presentLayer;}
    public CameraHandler CameraHandler { get => _cameraHandler;}
    public GameObject CameraTarget { get => _cameraTarget; set => _cameraTarget = value; }
    public GameObject CameraTargetParent { get => _cameraTargetParent; set => _cameraTargetParent = value; }

    public InputManager InputManager { get => _inputManager;}

    #endregion

    #region Methods

    //Methods

    public void OnEnable()
    {
        _pawn = GameObject.Find(PAWN_OBJECT);
        _rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _fsmCharacter = new StateMachineCharacter();
        _changeTime         = GetComponent<ChangeTime>();

        _soul = GameObject.Find(SOUL_OBJECT);
        _soul.SetActive(false);


        _cameraTarget = _cameraTargetParent.transform.Find(CAMERA_TARGET_OBJECT).gameObject;


        _fsmCharacter.InitStateMachine(this);
        _fsmCharacter.InitState(_fsmCharacter.States[EnumStateCharacter.Idle]);


    public void Start()
        _cameraHandler = GameManager.Instance.CameraHandler; //Il faut appeler ça après le load des 3C dans gameManager
        _cameraHandler = GameManager.Instance.CameraHandler; //Il faut appeler ça après le load des 3C dans gameManager

    }

    private void Update()
    {
        _fsmCharacter.StateMachineUpdate();
    }

    private void FixedUpdate()
    {
        _fsmCharacter.StateMachineFixedUpdate();
    }

    #endregion

}
