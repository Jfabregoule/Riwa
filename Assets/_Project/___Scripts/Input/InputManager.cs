using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class InputManager : Singleton<InputManager>
{
    private Controls _controls;

    private int? _joystickFingerId = null;
    private int? _interactFingerId = null;

    private bool _gameplayEnabled;
    private bool _dialogueEnabled;

    #region Events

    public delegate void PressEvent();
    public delegate void MoveEvent(Vector2 position);
    public event PressEvent OnInteract;
    public event PressEvent OnInteractEnd;
    public event MoveEvent OnMove;
    public event PressEvent OnMoveEnd;
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
        _controls.Gameplay.Move.Enable();
    }

    public void DisableGameplayWithoutInteractControls()
    {
        _controls.Gameplay.Move.Disable();
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
        //_controls.Gameplay.Touch.performed += ctx => ReadTouches("TOUCH START");
        //_controls.Gameplay.Touch.canceled += ctx => ReadTouches("TOUCH END");

        Touch.onFingerDown += OnFingerDown;
        Touch.onFingerMove += OnFingerMove;
        Touch.onFingerUp += OnFingerUp;

        //_controls.Gameplay.SecondTouch.performed += ctx => TouchPerfomed(1);
        //_controls.Gameplay.SecondTouch.canceled += ctx => TouchCanceled(1);

        _controls.Gameplay.Options.performed += ctx => OptionsPerfomed();

        //Pour PC
        _controls.Gameplay.Interact.performed += ctx => InteractPerfomed();
        _controls.Gameplay.Interact.canceled += ctx => InteractCanceled();
    }

    private void UnbindGameplayEvents()
    {
        //_controls.Gameplay.Touch.performed -= ctx => ReadTouches("TOUCH START");
        //_controls.Gameplay.Touch.canceled -= ctx => ReadTouches("TOUCH END");

        Touch.onFingerDown -= OnFingerDown;
        Touch.onFingerMove -= OnFingerMove;
        Touch.onFingerUp -= OnFingerUp;

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

    //private void ReadTouches(string context)
    //{
    //    foreach (var touch in Touchscreen.current.touches)
    //    {
    //        if (!touch.press.isPressed && context == "TOUCH END") // cas du doigt lev�
    //        {
    //            Vector2 pos = touch.position.ReadValue();
    //            int id = touch.touchId.ReadValue();

    //            if (pos.x < Screen.width / 2)
    //                OnMoveEnd?.Invoke();
    //            else
    //                OnInteractEnd?.Invoke();
    //        }
    //        else if (touch.press.isPressed && context == "TOUCH START") // cas du doigt pos�
    //        {
    //            Vector2 pos = touch.position.ReadValue();
    //            int id = touch.touchId.ReadValue();

    //            if (pos.x < Screen.width / 2)
    //                OnMove?.Invoke();
    //            else
    //                OnInteract?.Invoke();
    //        }
    //    }
    //}

    private void OnFingerDown(Finger finger)
    {
        Vector2 pos = finger.currentTouch.screenPosition;

        if (pos.x < Screen.width / 2 && _joystickFingerId == null)
        {
            if (_interactFingerId != finger.index)
            {
                _joystickFingerId = finger.index;
                OnMove?.Invoke(pos);
            }
        }
        else if (pos.x >= Screen.width / 2 && _interactFingerId == null)
        {
            if (_joystickFingerId != finger.index)
            {
                _interactFingerId = finger.index;
                OnInteract?.Invoke();
            }
        }
    }

    private void OnFingerMove(Finger finger)
    {
        if (_joystickFingerId == finger.index)
        {
            // Tu peux ajouter ici des donn�es au joystick si besoin
            // var pos = finger.currentTouch.screenPosition;
        }
    }

    private void OnFingerUp(Finger finger)
    {
        if (_joystickFingerId == finger.index)
        {
            _joystickFingerId = null;
            OnMoveEnd?.Invoke();
        }

        if (_interactFingerId == finger.index)
        {
            _interactFingerId = null;
            OnInteractEnd?.Invoke();
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
