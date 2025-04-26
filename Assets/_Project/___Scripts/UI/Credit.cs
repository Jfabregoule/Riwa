using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credit : MonoBehaviour
{
    [SerializeField] private float _scrollSpeed;
    [SerializeField] private float _maxTreshold;
    private CanvasGroup _creditCanvasGroup;
    private RectTransform _rectTransform;


    private bool _isEnable = false;
    private void Start()
    {
        _creditCanvasGroup = transform.parent.GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (!_isEnable) return;

        _rectTransform.anchoredPosition += new Vector2(0, _scrollSpeed * Time.deltaTime);

        if (_rectTransform.anchoredPosition.y < _maxTreshold) return;

        ToggleOffCredit();
    }
    public void ToggleOffCredit()
    {
        _isEnable = false;
        _creditCanvasGroup.alpha = 0;
        _creditCanvasGroup.blocksRaycasts = false;
        _creditCanvasGroup.interactable = false;
        _rectTransform.anchoredPosition = Vector2.zero;
    }

    public void ToggleCredit()
    {
        _isEnable = true;
        _creditCanvasGroup.alpha = 1;
        _creditCanvasGroup.blocksRaycasts = true;
        _creditCanvasGroup.interactable = true;
    }
}
