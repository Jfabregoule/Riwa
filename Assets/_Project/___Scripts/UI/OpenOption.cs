using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenOption : MonoBehaviour
{
    [SerializeField] private CanvasGroup _navBarCanvasGroup;
    [SerializeField] private float _movingTime;
    private InputManager _inputManager;
    private CanvasGroup _optionsButtonCanvasGroup;
    private RectTransform _navbarRectTransform;
    private Vector3 _initialPos;

    void OnEnable()
    {
        StartCoroutine(Helpers.WaitMonoBeheviour(() => InputManager.Instance, SubscribeToInputManager));
        _optionsButtonCanvasGroup = GetComponent<CanvasGroup>();
        _navbarRectTransform = _navBarCanvasGroup.gameObject.GetComponent<RectTransform>();
        _initialPos = _navbarRectTransform.localPosition;
    }

    void OnDisable()
    {
        if (_inputManager != null)
        {
            _inputManager.OnOpenOptions -= OpenOptions;
            _inputManager.OnTouchScreen -= CloseOptions;
        }
    }
    public void OpenOptions()
    {
        Helpers.DisabledCanvasGroup(_optionsButtonCanvasGroup);
        _inputManager.DisableGameplayControls();
        //_inputManager.DisableDialogueControls();
        StartCoroutine(MoveNavbarUp());
    }

    public void CloseOptions()
    {
        //_inputManager.EnableDialogueControls();
        if (_navBarCanvasGroup.alpha != 1) return;
        _inputManager.EnableGameplayControls();
        Helpers.EnabledCanvasGroup(_optionsButtonCanvasGroup);
        StartCoroutine(MoveNavbarDown());
    }
    private void SubscribeToInputManager(InputManager script)
    {
        if (script != null)
        {
            _inputManager = script;
            _inputManager.OnOpenOptions += OpenOptions;
            _inputManager.OnTouchScreen += CloseOptions;
        }
    }
    public void ResetTransform()
    {
        _navbarRectTransform.localPosition = _initialPos;
    }
    public void OpenWithoutAnim()
    {
        _navbarRectTransform.localPosition = new Vector3(0f, _initialPos.y, 0f);
        Helpers.EnabledCanvasGroup(_navBarCanvasGroup);
    }
    private IEnumerator MoveNavbarUp()
    {
        float timer = 0f;
        Vector3 initialPos = _navbarRectTransform.localPosition;
        Vector3 finalPos = new Vector3(0f, _navbarRectTransform.localPosition.y,0f);

        while(timer < _movingTime)
        {
            timer += Time.deltaTime;

            _navBarCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / _movingTime);

            _navbarRectTransform.localPosition = Vector3.Lerp(initialPos, finalPos, timer / _movingTime);

            yield return null;
        }
        _navbarRectTransform.localPosition = finalPos;
        Helpers.EnabledCanvasGroup(_navBarCanvasGroup);
    }

    private IEnumerator MoveNavbarDown()
    {
        float timer = 0f;
        Vector3 _finalPos = _navbarRectTransform.localPosition;

        while (timer < _movingTime)
        {
            timer += Time.deltaTime;

            _navBarCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / _movingTime);

            _navbarRectTransform.localPosition = Vector3.Lerp(_finalPos, _initialPos, timer / _movingTime);

            yield return null;
        }
        _navbarRectTransform.localPosition = _initialPos;
        Helpers.DisabledCanvasGroup(_navBarCanvasGroup);

    }
}
