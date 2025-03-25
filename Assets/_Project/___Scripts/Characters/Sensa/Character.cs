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



    [Header("Gameplay Statistics")]

    [SerializeField] private float _speed = 1;

    #endregion

    #region Properties

    //Properties

    public GameObject Pawn { get => _pawn;}
    public FSMCharacter FsmCharacter { get => _fsmCharacter;}
    public Animator Animator { get => _animator;}
    public VariableJoystick Joystick { get => _joystick;}
    public float Speed { get => _speed; set => _speed = value; }
    public Rigidbody Rb { get => _rb;}

    #endregion

    #region Methods

    //Methods

    public void Start()
    {
        _pawn = GameObject.Find(PAWN_OBJECT);
        _rb = _pawn.GetComponent<Rigidbody>();
        _fsmCharacter = new FSMCharacter();
        _fsmCharacter.InitStateMachine(this);

        _joystick = GameObject.Find("Variable Joystick").GetComponent<VariableJoystick>();

    }

    #endregion

}
