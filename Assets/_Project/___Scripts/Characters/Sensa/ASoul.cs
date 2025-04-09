using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private StateMachineSoul _fsmSoul;
    private Animator _animator;
    private GameObject _character;

    private VariableJoystick _joystick; //TEMPORAIRE EN ATTENDANT L'INPUT SYSTEM

    private bool _canInteract;
    private bool _canInteractSoul;

    private CameraHandler _cameraHandler;

    [Header("Gameplay Statistics")]

    [SerializeField] private float _joystickRunTreshold = 0.4f;
    [SerializeField] private float _linkMaxDistance = 2.5f;
    [SerializeField] private float _linkElasticity = 500f;

    [Header("StateMachine values")]

    [SerializeField] private float _timeBeforeWait = 2.0f;

    #endregion

    #region Properties

    //Properties

    public GameObject SoulPawn { get => _soulPawn;}
    public StateMachineSoul FsmSoul { get => _fsmSoul;}
    public Animator Animator { get => _animator;}
    public VariableJoystick Joystick { get => _joystick;}
    public bool CanInteract { get => _canInteract; set => _canInteract = value; }
    public bool CanInteractSoul { get => _canInteractSoul; set => _canInteractSoul = value; }
    public float JoystickRunTreshold { get => _joystickRunTreshold; set => _joystickRunTreshold = value; }
    public float TimeBeforeWait { get => _timeBeforeWait; set => _timeBeforeWait = value; }
    public CameraHandler CameraHandler { get => _cameraHandler;}
    public GameObject Character { get => _character; set => _character = value; }
    public float LinkMaxDistance { get => _linkMaxDistance; set => _linkMaxDistance = value; }
    public float LinkElasticity { get => _linkElasticity; set => _linkElasticity = value; }

    #endregion

    #region Methods

    //Methods

    public void Start()
    {
        Character = GameObject.Find(CHARACTER_OBJECT);
        _soulPawn = GameObject.Find(SOULPAWN_OBJECT);
        _rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _fsmSoul = new StateMachineSoul();
        _fsmSoul.InitStateMachine(this);

        _animator = GetComponent<Animator>();

        _joystick = GameObject.Find("Variable Joystick").GetComponent<VariableJoystick>(); //A modifier plus tard 

        _fsmSoul.InitState(_fsmSoul.States[EnumStateSoul.Idle]);

        _cameraHandler = GameManager.Instance.CameraHandler; //Il faut appeler ça après le load des 3C dans gameManager

    }

    private void Update()
    {
        _fsmSoul.StateMachineUpdate();
    }

    private void FixedUpdate()
    {
        _fsmSoul.StateMachineFixedUpdate();
    }

    #endregion

}
