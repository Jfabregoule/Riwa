using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{

    private CanvasGroup _canvasGroup;

    private GameObject _tempoButtonParent;
    private GameObject _tempoButton;

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

    private void GrayIn() {
        if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
        _currentCoroutine = StartCoroutine(Fade(0, 0.8f, 0.3f, true));
    }
    private void GrayOut()
    {
        if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
        _currentCoroutine = StartCoroutine(Fade(0.8f, 0, 0.3f, false));
    }

    public IEnumerator Fade(float start, float end, float duration,bool isEnable)
    {

        float clock = 0f;

        if (isEnable)
        {
            Helpers.EnabledCanvasGroup(_canvasGroup);
        }
        else
        {
            Helpers.DisabledCanvasGroup(_canvasGroup);
        }

        while (clock < duration)
        {
            clock += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(start, end, clock / duration);
            yield return null;
        }
    }

    public void HighlightButton(IPulsable button)
    {
        GrayIn();

        MonoBehaviour obj = (MonoBehaviour)button;

        _tempoButton = obj.gameObject;
        _tempoButtonParent = obj.gameObject.transform.parent.gameObject;
        _tempoButton.transform.parent = transform.parent.transform;
        //_tempoButton.transform.SetSiblingIndex(0);
    }

    public void ResetHighlighButton()
    {
        GrayOut();

        if (_tempoButton == null) return;

        if( _tempoButtonParent != null )
        {
            _tempoButton.transform.parent = _tempoButtonParent.transform;
            _tempoButtonParent = null;
            _tempoButton = null;
        } 

    }


}
