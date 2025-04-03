using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ACharacter : MonoBehaviour
{
    #region Constantes
    
    //Constantes

    public const string PAWN_OBJECT = "Pawn";

    #endregion

    #region Fields

    //Field

    private GameObject _pawn;
    private StateMachineCharacter _fsmCharacter;
    private Animator _animator;
    private Rigidbody _rb;
    private CapsuleCollider _capsuleCollider;

    private VariableJoystick _joystick; //TEMPORAIRE EN ATTENDANT L'INPUT SYSTEM

    private bool _canInteract;
    private bool _canInteractSoul;

    private bool _isInPast = false;
    private ChangeTime _changeTime;

    [SerializeField] private LayerMask _pastLayer;
    [SerializeField] private LayerMask _presentLayer;

    [Header("Gameplay Statistics")]

    [SerializeField] private float _speed = 1;
    [SerializeField] private float _joystickRunTreshold = 0.4f;

    [Header("StateMachine values")]

    [SerializeField] private float _timeBeforeWait = 2.0f;
    [SerializeField] private bool _isChangingTime = false;

    #endregion

    #region Properties

    //Properties

    public GameObject Pawn { get => _pawn;}
    public StateMachineCharacter FsmCharacter { get => _fsmCharacter;}
    public Animator Animator { get => _animator;}
    public VariableJoystick Joystick { get => _joystick;}
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
    public LayerMask PastLayer { get => _pastLayer;}
    public LayerMask PresentLayer { get => _presentLayer;}

    #endregion

    #region Methods

    //Methods

    public void Start()
    {
        _pawn = GameObject.Find(PAWN_OBJECT);
        _rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _fsmCharacter = new StateMachineCharacter();
        _fsmCharacter.InitStateMachine(this);

        _animator = GetComponent<Animator>();

        _joystick = GameObject.Find("Variable Joystick").GetComponent<VariableJoystick>(); //A modifier plus tard 

        _fsmCharacter.InitState(_fsmCharacter.States[EnumStateCharacter.Idle]);

        _changeTime = GameObject.Find("Sphere").GetComponent<ChangeTime>();

    }

    private void Update()
    {
        float dt = Time.deltaTime;

        _fsmCharacter.StateMachineUpdate(dt);
    }

    public void OnChangeTempo() //A DEGAGER QUAND Y'AURA L'INPUT SYSTEM
    {
        IsChangingTime = true;
    }

    #endregion

}
