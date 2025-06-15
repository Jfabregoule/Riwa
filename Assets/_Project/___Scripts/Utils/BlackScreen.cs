using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{

    private CanvasGroup _canvasGroup;
    private Coroutine _currentCoroutine;

    public void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        //GameManager.Instance.OnRoomExit += FadeIn;
    }

    public void FadeIn(float time)
    {
        StartCoroutine(Fade(0, 1, time, true));
    }

    public void FadeOut(float time)
    {
        StartCoroutine(Fade(1, 0, time, false));
    }

    public void GrayIn() {
        if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
        _currentCoroutine = StartCoroutine(Fade(0, 0.8f, 0.3f, true));
    }
    public void GrayOut()
    {
        if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
        _currentCoroutine = StartCoroutine(Fade(0.8f, 0, 0.3f, false));
    }

    public void SetAlpha(float alpha)
    {
        _canvasGroup.alpha = alpha;
    }

    public IEnumerator Fade(float start, float end, float duration,bool isEnable)
    {

        float clock = 0f;

        if (isEnable)
            Helpers.EnabledCanvasGroup(_canvasGroup);

        while (clock < duration)
        {
            clock += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(start, end, clock / duration);
            yield return null;
        }

        if (!isEnable)
            Helpers.DisabledCanvasGroup(_canvasGroup);
    }
}
