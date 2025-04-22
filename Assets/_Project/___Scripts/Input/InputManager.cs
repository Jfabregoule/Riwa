using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class InputManager : Singleton<InputManager>
{
    private Controls _controls;

    private int? _joystickFingerId = null;

    private bool _gameplayEnabled;
    private bool _dialogueEnabled;
    private bool _optionsEnabled;

    private bool _interactEnabled;
    private bool _moveEnabled;

    #region Events

    public delegate void PressEvent();
    public delegate void MoveEvent(Vector2 position);
    public event PressEvent OnInteract;
    public event MoveEvent OnMove;
    public event PressEvent OnMoveEnd;
    public event PressEvent OnChangeTime;
    public event PressEvent OnOpenOptions;
    public event PressEvent OnAdvanceDialogue;

    #endregion

    protected override void Awake()
    {
        base.Awake();
        _controls = new Controls();

        EnhancedTouchSupport.Enable();

        _gameplayEnabled = true;
        _dialogueEnabled = false;
        _optionsEnabled = true;

        _interactEnabled = true;
        _moveEnabled = true;
    }

    #region Enabled Methods

    private void OnEnable()
    {
        _controls.Enable();

        if (_gameplayEnabled)
            BindGameplayEvents();
        if (_dialogueEnabled)
            BindDialogueEvents();
        if(_optionsEnabled)
            BindOptionsEvents();
    }

    private void OnDisable()
    {
        _controls.Disable();

        if (_gameplayEnabled)
            UnbindGameplayEvents();
        if (_dialogueEnabled)
            UnbindDialogueEvents();
        if (_optionsEnabled)
            UnbindOptionsEvents();
    }

    public void EnableGameplayControls()
    {
        if (_gameplayEnabled) return;
        _gameplayEnabled = true;
        _interactEnabled = true;
        _moveEnabled = true;
        BindGameplayEvents();

        _controls.Gameplay.Enable();
    }

    public void DisableGameplayControls()
    {
        if (!_gameplayEnabled) return;
        _gameplayEnabled = false;
        _interactEnabled = false;
        _moveEnabled = false;
        UnbindGameplayEvents();

        _controls.Gameplay.Disable();
    }

    public void EnableGameplayMoveControls()
    {
        _moveEnabled = true;
        Touch.onFingerDown += OnFingerDown;
        Touch.onFingerUp += OnFingerUp;
        _controls.Gameplay.Move.Enable();
    }

    public void DisableGameplayMoveControls()
    {
        _moveEnabled = false;
        Touch.onFingerDown -= OnFingerDown;
        Touch.onFingerUp -= OnFingerUp;
        _joystickFingerId = null;
        OnMoveEnd?.Invoke();
        _controls.Gameplay.Move.Disable();
    }

    public void EnableGameplayChangeTimeControls()
    {
        _controls.Gameplay.ChangeTime.Enable();
    }

    public void DisableGameplayChangeTimeControls()
    {
        _controls.Gameplay.ChangeTime.Disable();
    }

    public void EnableGameplayInteractControls()
    {
        _interactEnabled = true;
        Touch.onFingerDown += OnFingerDown;
        Touch.onFingerUp += OnFingerUp;
        _controls.Gameplay.Interact.Enable();
    }

    public void DisableGameplayInteractControls()
    {
        _interactEnabled = false;
        Touch.onFingerDown -= OnFingerDown;
        Touch.onFingerUp -= OnFingerUp;
        _controls.Gameplay.Interact.Disable();
    }

    public void EnableDialogueControls()
    {
        if (_dialogueEnabled) return;
        _dialogueEnabled = true;
        BindDialogueEvents();

        _controls.Dialogue.Enable();
    }

    public void DisableDialogueControls()
    {
        if (!_dialogueEnabled) return;
        _dialogueEnabled = false;
        UnbindDialogueEvents();

        _controls.Dialogue.Disable();
    }

    public void EnableOptionsControls()
    {
        if (_optionsEnabled) return;
        _optionsEnabled = true;
        BindOptionsEvents();

        _controls.Options.Enable();
    }

    public void DisableOptionsControls()
    {
        if (!_optionsEnabled) return;
        _optionsEnabled = false;
        UnbindOptionsEvents();

        _controls.Options.Disable();
    }

    #endregion

    #region Public Readers

    public Vector2 GetMoveDirection()
    {
        return _controls.Gameplay.Move.ReadValue<Vector2>();
    }

    public Vector2 GetPressPosition()
    {
        return _controls.Gameplay.PressPosition.ReadValue<Vector2>();
    }
    #endregion


    #region Bind / Unbind Methods
    private void BindGameplayEvents()
    {
        Touch.onFingerDown += OnFingerDown;
        Touch.onFingerUp += OnFingerUp;

        _controls.Gameplay.ChangeTime.canceled += ctx => ChangeTimePerfomed();

        //Pour PC
        _controls.Gameplay.Interact.performed += ctx => InteractPerfomed();
    }

    private void UnbindGameplayEvents()
    {
        Touch.onFingerDown -= OnFingerDown;
        Touch.onFingerUp -= OnFingerUp;

        _controls.Gameplay.ChangeTime.canceled -= ctx => ChangeTimePerfomed();

        //Pour PC
        _controls.Gameplay.Interact.performed -= ctx => InteractPerfomed();
    }

    private void BindDialogueEvents()
    {
        _controls.Dialogue.Advance.performed += ctx => AdvanceDialoguePerfomed();
    }

    private void UnbindDialogueEvents()
    {
        _controls.Dialogue.Advance.performed -= ctx => AdvanceDialoguePerfomed();
    }

    private void BindOptionsEvents()
    {
        _controls.Options.Open.performed += ctx => OptionsPerfomed();
    }

    private void UnbindOptionsEvents()
    {
        _controls.Options.Open.performed -= ctx => OptionsPerfomed();
    }
    #endregion

    #region Events Methods

    private void OnFingerDown(Finger finger)
    {
        Vector2 pos = finger.currentTouch.screenPosition;

        if (pos.x < Screen.width / 2 && _joystickFingerId == null && _moveEnabled)
        {
            _joystickFingerId = finger.index;
            OnMove?.Invoke(pos);
        }

        else if (pos.x >= Screen.width / 2 && _interactEnabled)
        {
            OnInteract?.Invoke();
        }
    }

    private void OnFingerUp(Finger finger)
    {
        if (_joystickFingerId == finger.index)
        {
            _joystickFingerId = null;
            OnMoveEnd?.Invoke();
        }
    }

    private void InteractPerfomed() => OnInteract?.Invoke();
    private void AdvanceDialoguePerfomed() => OnAdvanceDialogue?.Invoke();
    private void ChangeTimePerfomed() => OnChangeTime?.Invoke();
    private void OptionsPerfomed() => OnOpenOptions?.Invoke();

    #endregion
}
