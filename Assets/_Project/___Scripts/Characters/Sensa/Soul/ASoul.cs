using UnityEngine;

public class ASoul : APawn<EnumStateSoul>
{
    #region Constantes

    public const string CHARACTER_OBJECT = "Character";
    public const string SOULPAWN_OBJECT = "SoulPawn";

    #endregion

    #region Fields

    //Field

    private GameObject _soulPawn;
    private GameObject _character;

    private VariableJoystick _joystick; //TEMPORAIRE EN ATTENDANT L'INPUT SYSTEM

    private bool _canInteract;
    private bool _canInteractSoul;

    private CameraHandler _cameraHandler;

    //new private StateMachineSoul _stateMachine;

    [Header("Gameplay Statistics")]

    [SerializeField] private float _joystickRunTreshold = 0.4f;
    [SerializeField] private float _linkMaxDistance = 2.5f;
    [SerializeField] private float _linkElasticity = 500f;

    [Header("StateMachine values")]

    [SerializeField] private float _timeBeforeWait = 2.0f;

    [Header("VFX")]
    [SerializeField] private GameObject _soulLinkVFX;
    [SerializeField] private ParticleSystem _auraVFX;

    #endregion

    #region Properties

    //Properties

    public GameObject SoulPawn { get => _soulPawn;}
    public bool CanInteract { get => _canInteract; set => _canInteract = value; }
    public bool CanInteractSoul { get => _canInteractSoul; set => _canInteractSoul = value; }
    public float JoystickRunTreshold { get => _joystickRunTreshold; set => _joystickRunTreshold = value; }
    public float TimeBeforeWait { get => _timeBeforeWait; set => _timeBeforeWait = value; }
    public CameraHandler CameraHandler { get => _cameraHandler;}
    public GameObject Character { get => _character; set => _character = value; }
    public float LinkMaxDistance { get => _linkMaxDistance; set => _linkMaxDistance = value; }
    public float LinkElasticity { get => _linkElasticity; set => _linkElasticity = value; }
    new public StateMachineSoul StateMachine { get => (StateMachineSoul)_stateMachine; set => _stateMachine = value; }

    #endregion

    #region Methods

    //Methods

    public void Awake()
    {
        _soulPawn = transform.Find(SOULPAWN_OBJECT).gameObject;
        _rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _inputManager = InputManager.Instance;
        Animator = GetComponent<Animator>();
        StateMachine = new StateMachineSoul();


        _cameraHandler = GameManager.Instance.CameraHandler; //Il faut appeler ça après le load des 3C dans gameManager
    }

    private void OnEnable()
    {
        _soulLinkVFX.gameObject.SetActive(true);
        _auraVFX.Play();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        _soulLinkVFX.gameObject.SetActive(false);
        _auraVFX.Stop();
    }

    private void Start()
    {
        _character = GameManager.Instance.Character.gameObject;
        StateMachine.InitStateMachine(this);
        StateMachine.InitState(_stateMachine.States[EnumStateSoul.Disable]);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        StateMachine.StateMachineUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.StateMachineFixedUpdate();
    }

    #endregion

}
