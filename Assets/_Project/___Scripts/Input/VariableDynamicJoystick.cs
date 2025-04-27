using UnityEngine;
using UnityEngine.UIElements;

public class VariableDynamicJoystick : MonoBehaviour
{
    private InputManager _inputManager;
    private CanvasGroup _canvasGroup;
    private RectTransform _background;
    private RectTransform _handle;

    private Vector2 _startPos;

    private bool _isLocked = false;

    void OnEnable()
    {
        StartCoroutine(Helpers.WaitMonoBeheviour(() => InputManager.Instance, SubscribeToInputManager));
    }

    void OnDisable()
    {
        if(_inputManager != null)
        {
            _inputManager.OnMove -= OnTouchStarted;
            _inputManager.OnMoveEnd -= OnTouchEnded;
            _inputManager.OnLockJoystick -= LockJoystick;
            _inputManager.OnUnlockJoystick -= UnlockJoystick;
        }
    }

    private void Start()
    {
        _canvasGroup = GetComponentInChildren<CanvasGroup>();
        RectTransform[] rects = _canvasGroup.GetComponentsInChildren<RectTransform>();
        _background = rects[0];
        _startPos = _background.position;
        _handle = rects[1];
    }

    private void OnTouchStarted(Vector2 position)
    {
        if (!_isLocked)
        {
            Helpers.EnabledCanvasGroup(_canvasGroup);
            _background.position = position;
        }
        _handle.anchoredPosition = Vector2.zero;
    }

    private void OnTouchEnded()
    {
        if (!_isLocked)
        {
            Helpers.DisabledCanvasGroup(_canvasGroup);
            _handle.anchoredPosition = Vector2.zero;
        }
    }

    private void LockJoystick()
    {
        _isLocked = true;
        _background.position = _startPos;
        Helpers.EnabledCanvasGroup(_canvasGroup);
    }

    private void UnlockJoystick()
    {
        _isLocked = false;
        Helpers.DisabledCanvasGroup(_canvasGroup);
    }

    private void SubscribeToInputManager(InputManager script)
    {
        if (script != null)
        {
            _inputManager = script;
            script.OnMove += OnTouchStarted;
            script.OnMoveEnd += OnTouchEnded;
            script.OnLockJoystick += LockJoystick;
            script.OnUnlockJoystick += UnlockJoystick;
        }
    }
}
