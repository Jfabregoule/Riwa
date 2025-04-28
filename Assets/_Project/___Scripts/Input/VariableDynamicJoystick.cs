using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class VariableDynamicJoystick : MonoBehaviour
{
    private InputManager _inputManager;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private CanvasGroup _arrowCanvasGroup;
    [SerializeField] private RectTransform _background;
    [SerializeField] private RectTransform _handle;
    [SerializeField] private RectTransform[] _arrowsRect;

    private Vector2[] _anchors;

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
        _anchors = new Vector2[4];
        _anchors[0] = new Vector2(0.5f, 1f);
        _anchors[1] = new Vector2(0.5f, 0f);
        _anchors[2] = new Vector2(0f, 0.5f);
        _anchors[3] = new Vector2(1f, 0.5f);
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
        Vector3 caissePosition = _character.HoldingObject.transform.position;
        Vector3 playerPosition = _character.transform.position;

        // Calcul de la direction entre le joueur et la caisse
        Vector3 directionToCaisse = (caissePosition - playerPosition).normalized;

        // Récupère la direction de la caméra
        Vector3 camForward = Vector3.ProjectOnPlane(GameManager.Instance.CameraHandler.transform.forward, Vector3.up).normalized;
        Vector3 camRight = Vector3.Cross(Vector3.up, camForward);

        // Direction actuelle du joueur
        Vector3 playerForward = _character.transform.forward;

        // Calcul des valeurs de dot pour la direction du joueur et la caisse
        float dotForward = Vector3.Dot(directionToCaisse, playerForward); // Direction relative à la vue du joueur
        float dotRight = Vector3.Dot(directionToCaisse, camRight);

        if (Mathf.Abs(dotForward) > Mathf.Abs(dotRight))
        {
            if (dotForward > 0.5f)
            {
                // Pull en haut
                SetAnchor(1, 0);

                // Push en bas
                SetAnchor(0, 1);

                // Rotation (Droite à Gauche)
                SetAnchor(3, 2);

                SetAnchor(2, 3);
            }
            else
            {
                // Push en haut
                SetAnchor(0, 0);

                // Pull en bas
                SetAnchor(1, 1);

                // Rotation (Gauche à Droite)
                SetAnchor(2, 2);

                SetAnchor(3, 3);
            }
        }
        else
        {
            if (dotRight > 0.5f)
            {
                // Pull à droite
                SetAnchor(1, 3);

                // Push à gauche
                SetAnchor(0, 2);

                // Rotation (Gauche en bas)
                SetAnchor(2, 1);

                // Rotation (Droite en haut)
                SetAnchor(3, 0);
            }
            else
            {
                // Pull à gauche
                SetAnchor(1, 2);

                // Push à droite
                SetAnchor(0, 3);

                // Rotation (Gauche en haut)
                SetAnchor(2, 0);

                // Rotation (Droite en bas)
                SetAnchor(3, 1);
            }
        }

        Helpers.EnabledCanvasGroup(_arrowCanvasGroup);
    }

    private void SetAnchor(int idRect, int Direction)
    {
        _arrowsRect[idRect].anchorMax = _anchors[Direction];
        _arrowsRect[idRect].anchorMin = _anchors[Direction];
        _arrowsRect[idRect].anchoredPosition = _anchors[Direction];
    }

    private void DisableCanvasGroup()
    {
        Helpers.DisabledCanvasGroup(_arrowCanvasGroup);
    }
}
