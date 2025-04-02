using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region Constantes
    
    //Constantes

    public const string PAWN_OBJECT = "Pawn";

    #endregion

    #region Fields

    //Field

    private GameObject _pawn;
    private FSMCharacter _fsmCharacter;
    private Animator _animator;
    private Rigidbody _rb;

    private VariableJoystick _joystick; //TEMPORAIRE EN ATTENDANT L'INPUT SYSTEM

    private bool _canInteract;
    private bool _canInteractSoul;

    [Header("Gameplay Statistics")]

    [SerializeField] private float _speed = 1;
    [SerializeField] private float _joystickRunTreshold = 0.4f;

    [Header("StateMachine values")]

    [SerializeField] private float _timeBeforeWait = 2.0f;

    #endregion

    #region Properties

    //Properties

    public GameObject Pawn { get => _pawn;}
    public FSMCharacter FsmCharacter { get => _fsmCharacter;}
    public Animator Animator { get => _animator;}
    public VariableJoystick Joystick { get => _joystick;}
    public Rigidbody Rb { get => _rb;}
    public bool CanInteract { get => _canInteract; set => _canInteract = value; }
    public bool CanInteractSoul { get => _canInteractSoul; set => _canInteractSoul = value; }
    public float Speed { get => _speed; set => _speed = value; }
    public float JoystickRunTreshold { get => _joystickRunTreshold; set => _joystickRunTreshold = value; }
    public float TimeBeforeWait { get => _timeBeforeWait; set => _timeBeforeWait = value; }

    #endregion

    #region Methods

    //Methods

    public void Start()
    {
        _pawn = GameObject.Find(PAWN_OBJECT);
        _rb = _pawn.GetComponent<Rigidbody>();
        _fsmCharacter = new FSMCharacter();
        _fsmCharacter.InitStateMachine(this);

        _animator = GetComponent<Animator>();

        _joystick = GameObject.Find("Variable Joystick").GetComponent<VariableJoystick>(); //A modifier plus tard 

        _fsmCharacter.InitState(_fsmCharacter.States[EnumStateCharacter.Idle]);

    }

    private void Update()
    {
        float dt = Time.deltaTime;

        _fsmCharacter.StateMachineUpdate(dt);
    }

    public void OnChangeTempo() //A DEGAGER QUAND Y'AURA L'INPUT SYSTEM
    {
       _fsmCharacter.ChangeState(_fsmCharacter.States[EnumStateCharacter.ChangeTempo]);
    }

    #endregion

}
