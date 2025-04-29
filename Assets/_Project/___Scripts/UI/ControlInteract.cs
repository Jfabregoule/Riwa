using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlInteract : MonoBehaviour
{

    private CanvasGroup _canvasGroup;
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    private void OnEnable()
    {
        GameManager.Instance.OnShowMoveInputEvent += ShowInteract;
    }

    public void ShowInteract()
    {
        InputManager.Instance.DisableGameplayControls();
        StartCoroutine(FadeInCanvas(1f));
    }

    private IEnumerator FadeInCanvas(float duration)
    {
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;

            _canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / duration);
            yield return null;
        }
        Helpers.EnabledCanvasGroup(_canvasGroup);
    }

    public void FadeOut()
    {
        InputManager.Instance.EnableGameplayControls();
        StartCoroutine(FadeOutCanvas(0.5f));
    }
    private IEnumerator FadeOutCanvas(float duration)
    {
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;

            _canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / duration);
            yield return null;
        }
        Helpers.DisabledCanvasGroup(_canvasGroup);
    }
}
