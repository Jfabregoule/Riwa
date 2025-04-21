using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{

    private Image _panel;

    public delegate void NoArgVoidReturn();
    public event NoArgVoidReturn OnFinishFade;


    public void Awake()
    {
        _panel = GetComponentInChildren<Image>();
    }

    public void FadeIn(float fadeSpeed = 1f)
    {
        StartCoroutine(Fade(0,1,fadeSpeed));
    }

    public void FadeOut(float fadeSpeed = 1f)
    {
        StartCoroutine(Fade(1,0,fadeSpeed));
    }

    public IEnumerator Fade(float start, float end, float speed)
    {
        float distance = Mathf.Abs(end - start);
        float duration = distance * speed;
        float clock = 0f;

        while (clock < duration)
        {
            
            clock += Time.deltaTime * 10;
            _panel.color = new Color(0,0,0,Mathf.Lerp(start,end,clock / duration));
            yield return null;
        }

        OnFinishFade?.Invoke();

    }

}
