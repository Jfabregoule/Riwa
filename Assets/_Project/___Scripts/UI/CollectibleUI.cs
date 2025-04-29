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
        Helpers.EnabledCanvasGroup(_canvas);
        yield return Helpers.GetWait(_timeToShow);
        Helpers.DisabledCanvasGroup(_canvas);
    }
}
