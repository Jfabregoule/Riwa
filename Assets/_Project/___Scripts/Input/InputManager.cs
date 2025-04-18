using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    private Controls _controls;

    private bool[] _touchMove;

    private bool _gameplayEnabled;
    private bool _dialogueEnabled;

    #region Events

    public delegate void PressEvent();
    public event PressEvent OnInteract;
    public event PressEvent OnInteractEnd;
    public event PressEvent OnMove;
    public event PressEvent OnMoveEnd;
    public event PressEvent OnOpenOptions;
    public event PressEvent OnAdvanceDialogue;

    #endregion

    protected override void Awake()
    {
        base.Awake();
        _controls = new Controls();

        _touchMove = new bool[2] { false, false };

        _gameplayEnabled = true;
        _dialogueEnabled = false;
    }

    #region Enabled Methods

    private void OnEnable()
    {
        _controls.Enable();

        if (_gameplayEnabled)
            BindGameplayEvents();
        if (_dialogueEnabled)
            BindDialogueEvents();
    }

    private void OnDisable()
    {
        _controls.Disable();

        if (_gameplayEnabled)
            UnbindGameplayEvents();
        if (_dialogueEnabled)
            UnbindDialogueEvents();
    }

    public void EnableGameplayControls()
    {
        if (_gameplayEnabled) return;
        _gameplayEnabled = true;
        BindGameplayEvents();

        _controls.Gameplay.Enable();
    }

    public void DisableGameplayControls()
    {
        if (!_gameplayEnabled) return;
        _gameplayEnabled = false;  
        UnbindGameplayEvents();

        _controls.Gameplay.Disable();
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

    public void EnableGameplayWithoutInteractControls()
    {
        _toggleJoystick = true;
    }

    public void DisableGameplayWithoutInteractControls()
    {
        _toggleJoystick = false;
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
        _controls.Gameplay.Touch.performed += ctx => TouchPerfomed();
        _controls.Gameplay.Touch.canceled += ctx => TouchCanceled();

        //_controls.Gameplay.SecondTouch.performed += ctx => TouchPerfomed(1);
        //_controls.Gameplay.SecondTouch.canceled += ctx => TouchCanceled(1);

        _controls.Gameplay.Options.performed += ctx => OptionsPerfomed();

        //Pour PC
        _controls.Gameplay.Interact.performed += ctx => InteractPerfomed();
        _controls.Gameplay.Interact.canceled += ctx => InteractCanceled();
    }

    private void UnbindGameplayEvents()
    {
        _controls.Gameplay.Touch.performed -= ctx => TouchPerfomed();
        _controls.Gameplay.Touch.canceled -= ctx => TouchCanceled();

        //_controls.Gameplay.SecondTouch.performed -= ctx => TouchPerfomed();
        //_controls.Gameplay.SecondTouch.canceled -= ctx => TouchCanceled();

        _controls.Gameplay.Options.performed -= ctx => OptionsPerfomed();

        //Pour PC
        _controls.Gameplay.Interact.performed -= ctx => InteractPerfomed();
        _controls.Gameplay.Interact.canceled -= ctx => InteractCanceled();
    }

    private void BindDialogueEvents()
    {
        _controls.Dialogue.Advance.performed += ctx => AdvanceDialoguePerfomed();
    }

    private void UnbindDialogueEvents()
    {
        _controls.Dialogue.Advance.performed -= ctx => AdvanceDialoguePerfomed();
    }
    #endregion

    #region Events Methods

    private void TouchPerfomed() 
    {
        var touches = Touchscreen.current.touches;

        for (int i = 0; i < touches.Count; i++)
        {
            if (i > 2) return;
            var touch = touches[i];
            if (touch.press.isPressed)
            {
                Vector2 pos = touch.position.ReadValue();
                if (pos.x > Screen.width / 2)
                {
                    _touchMove[i] = false;
                    OnInteract?.Invoke();
                }
                else
                {
                    _touchMove[i] = true;
                    OnMove?.Invoke();
                }
            }
        }
        
    }

    private void TouchCanceled()
    {
        var touches = Touchscreen.current.touches;

        for (int i = 0; i < touches.Count; i++)
        {
            if (i > 2) return;
            var touch = touches[i];
            if (!touch.press.isPressed)
            {
                if (!_touchMove[i])
                {
                    OnInteractEnd?.Invoke();
                }
                else
                {
                    OnMoveEnd?.Invoke();
                }
            }
        }
    }

    private void InteractPerfomed() => OnInteract?.Invoke();
    private void InteractCanceled() => OnInteractEnd?.Invoke();
    private void AdvanceDialoguePerfomed() => OnAdvanceDialogue?.Invoke();

    public void InteractTrue() => OnInteract?.Invoke();
    public void InteractFalse() => OnInteractEnd?.Invoke();

    public void OptionsPerfomed() => OnOpenOptions?.Invoke();

    #endregion
}
