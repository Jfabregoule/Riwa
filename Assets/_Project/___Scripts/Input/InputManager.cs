using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : Singleton<InputManager>
{
    private Controls _controls;

    private bool _gameplayEnabled;
    private bool _dialogueEnabled;
    private bool _optionsEnabled;

    private bool _rotateEnabled;
    private bool _pushEnabled;
    private bool _pullEnabled;

    private bool _controlsInverted = false;

    #region Events

    public delegate void PressEvent();
    public delegate void MoveEvent(Vector2 position);
    public delegate void Lock(bool isRight);
    public event PressEvent OnInteract;
    public event MoveEvent OnMove;
    public event PressEvent OnMoveEnd;
    public event PressEvent OnChangeTime;
    public event PressEvent OnOpenOptions;
    public event PressEvent OnAdvanceDialogue;
    public event Lock OnLockJoystick;
    public event Lock OnUnlockJoystick;
    public event PressEvent OnTouchScreen;
    public event PressEvent OnPush;
    public event PressEvent OnPull;
    public event PressEvent OnRotateRight;
    public event PressEvent OnRotateLeft;

    #endregion

    protected override void Awake()
    {
        base.Awake();
        _controls = new Controls();


        _gameplayEnabled = true;
        _dialogueEnabled = false;
        _optionsEnabled = true;

        _rotateEnabled = true;
        _pushEnabled = true;
        _pullEnabled = true;
}

    private bool IsTouchOverUI(Vector2 touchPosition, int threshold = 0)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = touchPosition
        };

        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        return raycastResults.Count > threshold;
    }

    public void ToggleControlInversion(bool value)
    {
        _controlsInverted = value;
    }


    #region Enabled Methods

    private void OnEnable()
    {
        _controls.Enable();
        _controls.Global.Enable();

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
        _controls.Global.Disable();

        if (_gameplayEnabled)
            UnbindGameplayEvents();
        if (_dialogueEnabled)
            UnbindDialogueEvents();
        if (_optionsEnabled)
            UnbindOptionsEvents();
    }

    public void EnableAllControls()
    {
        EnableGameplayControls();
        EnableDialogueControls();
        EnableOptionsControls();
    }

    public void EnableGameplayControls()
    {
        if (_gameplayEnabled) return;
        _gameplayEnabled = true;

        _rotateEnabled = true;
        _pushEnabled = true;
        _pullEnabled = true;
        BindGameplayEvents();

        _controls.Gameplay.Enable();
    }

    public void DisableGameplayControls()
    {
        if (!_gameplayEnabled) return;
        _gameplayEnabled = false;

        _rotateEnabled = false;
        _pushEnabled = false;
        _pullEnabled = false;
        OnMoveEnd?.Invoke();
        UnbindGameplayEvents();

        _controls.Gameplay.Disable();
    }

    public void EnableGameplayMoveControls()
    {
        _controls.Gameplay.Touch.Enable();
        _controls.Gameplay.Move.Enable();
    }

    public void DisableGameplayMoveControls()
    {
        OnMoveEnd?.Invoke();
        _controls.Gameplay.Touch.Disable();
        _controls.Gameplay.Move.Disable();
    }

    public void LockJoystick()
    {
        OnLockJoystick?.Invoke(!_controlsInverted);
    }

    public void UnlockJoystick()
    {
        OnUnlockJoystick?.Invoke(!_controlsInverted);
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
        _controls.Gameplay.Interact.Enable();
    }

    public void DisableGameplayInteractControls()
    {
        _controls.Gameplay.Interact.Disable();
    }

    public void EnableGameplayRotateControls()
    {
        _rotateEnabled = true;
    }
    public void DisableGameplayRotateControls()
    {
        _rotateEnabled = false;
    }
    public void EnableGameplayPushControls()
    {
        _pushEnabled = true;
    }
    public void DisableGameplayPushControls()
    {
        _pushEnabled = false;
    }
    public void EnableGameplayPullControls()
    {
        _pullEnabled = true;
    }
    public void DisableGameplayPullControls()
    {
        _pullEnabled = false;
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
        return _controls.Global.PressPosition.ReadValue<Vector2>();
    }
    #endregion


    #region Bind / Unbind Methods
    private void BindGameplayEvents()
    {
        _controls.Gameplay.Touch.performed += ctx => TouchPerfomed();
        _controls.Gameplay.Touch.canceled += ctx => TouchCanceled();

        _controls.Gameplay.ChangeTime.performed += ctx => ChangeTimePerfomed();
        _controls.Gameplay.Interact.performed += ctx => InteractPerfomed();

        _controls.Gameplay.Displacement.performed += ctx => DisplacementPerfomed();
    }

    private void UnbindGameplayEvents()
    {
        _controls.Gameplay.Touch.performed -= ctx => TouchPerfomed();
        _controls.Gameplay.Touch.canceled -= ctx => TouchCanceled();

        _controls.Gameplay.ChangeTime.performed -= ctx => ChangeTimePerfomed();
        _controls.Gameplay.Interact.performed -= ctx => InteractPerfomed();

        _controls.Gameplay.Displacement.performed -= ctx => DisplacementPerfomed();
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
        _controls.Options.Touch.performed += ctx => TouchScreenPerformed();
    }

    private void UnbindOptionsEvents()
    {
        _controls.Options.Open.performed -= ctx => OptionsPerfomed();
        _controls.Options.Touch.performed -= ctx => TouchScreenPerformed();

    }
    #endregion

    #region Events Methods

    private void TouchPerfomed() {
        Vector2 pos = GetPressPosition();
        bool isLeftSide = pos.x < Screen.width / 2;
        if (_controlsInverted) isLeftSide = !isLeftSide;

        if (isLeftSide)
        {
            if (IsTouchOverUI(pos, 0)) return;

            OnMove?.Invoke(pos);
        }
    }

    private void TouchCanceled()
    {
        OnMoveEnd?.Invoke();
    }

    private void InteractPerfomed()
    {
        System.Delegate[] deletages = OnInteract.GetInvocationList();
        OnInteract?.Invoke();
    }
    private void AdvanceDialoguePerfomed() 
    {
        if (IsTouchOverUI(GetPressPosition(), 0)) return;

        OnAdvanceDialogue?.Invoke();
    }
    private void ChangeTimePerfomed() => OnChangeTime?.Invoke();
    private void OptionsPerfomed() => OnOpenOptions?.Invoke();

    private void TouchScreenPerformed()
    {
        if (IsTouchOverUI(GetPressPosition(), 0)) return;

        OnTouchScreen?.Invoke();
    }

    private void DisplacementPerfomed()
    {
        Vector2 action = _controls.Gameplay.Displacement.ReadValue<Vector2>();

        if (action == Vector2.up)
            OnPush?.Invoke();
        else if (action == Vector2.down)
            OnPull?.Invoke();
        else if (action == Vector2.left)
            OnRotateLeft?.Invoke();
        else if (action == Vector2.right)
            OnRotateRight?.Invoke();
    }

    #endregion
}
