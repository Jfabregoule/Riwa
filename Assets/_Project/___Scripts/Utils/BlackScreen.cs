using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{

    private CanvasGroup _canvasGroup;

    public void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        GameManager.Instance.OnRoomChange += FadeIn;

    }
    public void FadeIn()
    {
        StartCoroutine(Fade(0,1,1f,true));
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(1,0,1f,false));
    }

    public IEnumerator Fade(float start, float end, float duration,bool isEnable)
    {

        float clock = 0f;

        while (clock < duration)
        {
            clock += Time.deltaTime * 10;
            _canvasGroup.alpha = Mathf.Lerp(start, end, clock / duration);
            yield return null;
        }

        if (isEnable)
        {
            Helpers.EnabledCanvasGroup(_canvasGroup);
        }
        else
        {
            Helpers.DisabledCanvasGroup(_canvasGroup);
        }
    }

}
