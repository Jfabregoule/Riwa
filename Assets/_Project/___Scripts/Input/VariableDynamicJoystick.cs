using UnityEngine;
using UnityEngine.TextCore.Text;

public class VariableDynamicJoystick : MonoBehaviour
{
    private InputManager _inputManager;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private CanvasGroup _arrowCanvasGroup;
    [SerializeField] private RectTransform _background;
    [SerializeField] private RectTransform _handle;   
    
    private ACharacter _character;

    void OnEnable()
    {
        StartCoroutine(Helpers.WaitMonoBeheviour(() => InputManager.Instance, SubscribeToInputManager));
        StartCoroutine(Helpers.WaitMonoBeheviour(() => GameManager.Instance.Character, SubscribeToCharacter));
    }

    void OnDisable()
    {
        if(_inputManager != null)
        {
            _inputManager.OnMove -= OnTouchStarted;
            _inputManager.OnMoveEnd -= OnTouchEnded;
        }
        if(_character != null)
        {
            _character.OnHoldingStart -= SetCanvasGroup;
            _character.OnHoldingEnd -= DisableCanvasGroup;
        }
    }

    private void Start()
    {
        
    }

    private void OnTouchStarted(Vector2 position)
    {
        Helpers.EnabledCanvasGroup(_canvasGroup);
        _background.position = position;
        _handle.anchoredPosition = Vector2.zero;
    }

    private void OnTouchEnded()
    {
        Helpers.DisabledCanvasGroup(_canvasGroup);
        _handle.anchoredPosition = Vector2.zero;
    }

    private void SubscribeToInputManager(InputManager script)
    {
        if (script != null)
        {
            _inputManager = script;
            script.OnMove += OnTouchStarted;
            script.OnMoveEnd += OnTouchEnded;
        }
    }

    private void SubscribeToCharacter(ACharacter script)
    {
        if(script != null)
        {
            _character = script;
            script.OnHoldingStart += SetCanvasGroup;
            script.OnHoldingEnd += DisableCanvasGroup;
        }
    }
    private void SetCanvasGroup()
    {
        Helpers.EnabledCanvasGroup(_arrowCanvasGroup);

        Vector2 joystickDir = _character.InputManager.GetMoveDirection();

        if (joystickDir == Vector2.zero) return;

        //Pour calculer avec l'orientation de la caméra

        Vector3 camForward = Vector3.ProjectOnPlane(GameManager.Instance.CameraHandler.transform.forward, Vector3.up).normalized;
        Vector3 camRight = Vector3.Cross(Vector3.up, camForward);

        /////////

        Vector3 inputDir = (camForward * joystickDir.y + camRight * joystickDir.x).normalized;

        Vector3 worldForward = _character.transform.forward;
        Vector3 worldRight = _character.transform.right;

        float dotForward = Vector3.Dot(worldForward, inputDir);
        float dotRight = Vector3.Dot(worldRight, inputDir);

        if (Mathf.Abs(dotForward) > Mathf.Abs(dotRight))
        {
            if (!_character.HoldingObject.TryGetComponent(out IMovable movable)) return;
            if (dotForward > 0.5f)
            {
                //Pull

            }
            else
            {
                //Push
            }
        }
        else
        {
            if (!_character.HoldingObject.TryGetComponent(out IRotatable rotatable)) return;
            if (dotRight > 0.5f)
            {
                //Rotate Droite
            }
            else
            {
                //Rotate Gauche

            }
        }
    }

    private void DisableCanvasGroup()
    {
        Helpers.DisabledCanvasGroup(_arrowCanvasGroup);
    }
}
