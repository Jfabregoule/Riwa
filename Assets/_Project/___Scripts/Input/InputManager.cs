using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private Controls _controls;

    private VariableJoystick _joystick;

    private bool _gameplayEnabled;
    private bool _dialogueEnabled;

    #region Events

    public delegate void PressEvent();
    public event PressEvent OnInteract;
    public event PressEvent OnInteractEnd;
    public event PressEvent OnAdvanceDialogue;

    #endregion

    protected override void Awake()
    {
        base.Awake();
        _controls = new Controls();

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

    #endregion

    #region Public Readers

    public Vector2 GetMoveDirection()
    {
        return _joystick != null ? _joystick.Direction : Vector2.zero;
    }

    public Vector2 GetPressPosition()
    {
        return _controls.Gameplay.PressPosition.ReadValue<Vector2>();
    }
    #endregion


    #region Bind / Unbind Methods
    private void BindGameplayEvents()
    {
        _controls.Gameplay.Interact.performed += ctx => PressPerfomed();
        _controls.Gameplay.Interact.canceled += ctx => InteractCanceled();
    }

    private void UnbindGameplayEvents()
    {
        _controls.Gameplay.Interact.performed -= ctx => PressPerfomed();
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

    private void PressPerfomed() {
        if(GetPressPosition().x > Screen.width / 2)
        {
            OnInteract?.Invoke();
        }
    }
    private void InteractPerfomed() => OnInteract?.Invoke();
    private void InteractCanceled() => OnInteractEnd?.Invoke();
    private void AdvanceDialoguePerfomed() => OnAdvanceDialogue?.Invoke();

    #endregion

    public void RegisterJoystick(VariableJoystick joystick)
    {
        _joystick = joystick;
    }
}
