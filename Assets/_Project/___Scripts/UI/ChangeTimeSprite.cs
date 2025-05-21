using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpriteTransition : MonoBehaviour
{
    [SerializeField] private Sprite _pastSprite;
    [SerializeField] private Sprite _presentSprite;

    [SerializeField] private float _transitionDuration = 1f;

    private Image _mainImage;
    private Image _transitionImage; // Image temporaire pour fondu
    private bool _isPresent;
    private Coroutine _spriteCoroutine;

    private void OnEnable()
    {
        GameManager.Instance.OnTimeChangeStarted += SetSprite;
    }

    private void Start()
    {
        _isPresent = true;
        _mainImage = GetComponent<Image>();

        GameObject tempObj = new GameObject("TransitionImage");
        tempObj.transform.SetParent(transform, false);

        _transitionImage = tempObj.AddComponent<Image>();
        _transitionImage.rectTransform.sizeDelta = _mainImage.rectTransform.sizeDelta;
        _transitionImage.sprite = null;
        _transitionImage.color = new Color(1, 1, 1, 0);
    }

    private void SetSprite(EnumTemporality temporality)
    {
        _isPresent = !_isPresent;

        if (_spriteCoroutine != null)
            StopCoroutine(_spriteCoroutine);

        Sprite fromSprite = _mainImage.sprite;
        Sprite toSprite = _isPresent ? _presentSprite : _pastSprite;

        _spriteCoroutine = StartCoroutine(FadeToSprite(toSprite, _transitionDuration));
    }

    private IEnumerator FadeToSprite(Sprite newSprite, float duration)
    {
        _transitionImage.sprite = newSprite;
        _transitionImage.color = new Color(1, 1, 1, 0);
        _transitionImage.enabled = true;

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            _transitionImage.color = new Color(1, 1, 1, t);
            _mainImage.color = new Color(1, 1, 1, 1 - t);
            yield return null;
        }

        _mainImage.sprite = newSprite;
        _mainImage.color = Color.white;
        _transitionImage.color = new Color(1, 1, 1, 0);
        _transitionImage.enabled = false;
    }
}
