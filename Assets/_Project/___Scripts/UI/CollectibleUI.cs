using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectibleUI : MonoBehaviour
{
    [SerializeField] CanvasGroup _canvas;
    [SerializeField] TextMeshProUGUI _textAddCollectible;
    [SerializeField] TextMeshProUGUI _textAllCollectible;
    [SerializeField] float _timeToShow;
    private void OnEnable()
    {
        StartCoroutine(Helpers.WaitMonoBeheviour(() => GameManager.Instance.CollectibleManager, SubscribeToCollectibleManager));
    }
    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CollectibleManager.OnCollect -= DisplayCollectibleCanvas;
            GameManager.Instance.CollectibleManager.OnCollectAdd -= UpdateTextAdd;
            GameManager.Instance.CollectibleManager.OnCollectAll -= UpdateTextAll;
        }
    }
    void Start()
    {
        
    }

    private void DisplayCollectibleCanvas()
    {
        StartCoroutine(ShowCollectible());
    }

    private void UpdateTextAdd(int nb)
    {
        _textAddCollectible.text = nb.ToString();
    }

    private void UpdateTextAll(int nb)
    {
        _textAllCollectible.text = nb.ToString();
    }

    private void SubscribeToCollectibleManager(CollectibleManager script)
    {
        if (script != null)
        {
            script.OnCollect += DisplayCollectibleCanvas;
            script.OnCollectAdd += UpdateTextAdd;
            script.OnCollectAll += UpdateTextAll;
        }
    }

    private IEnumerator ShowCollectible()
    {
        yield return StartCoroutine(FadeShowCollectible(1f));
        Helpers.GetWait(_timeToShow);
        StartCoroutine(FadeHideCollectible(1f));
    }

    private IEnumerator FadeShowCollectible(float duration)
    {
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            _canvas.alpha = Mathf.Lerp(0f, 1f, timer / duration);
            yield return null;
        }
        Helpers.EnabledCanvasGroup(_canvas);
    }

    private IEnumerator FadeHideCollectible(float duration)
    {
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            _canvas.alpha = Mathf.Lerp(1f, 0f, timer / duration);
            yield return null;
        }
        Helpers.DisabledCanvasGroup(_canvas);
    }
}
