using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{

    private CanvasGroup _canvasGroup;
    private Coroutine _currentCoroutine;

    private RectTransform _fondNoirRect;
    private CutoutMaskUi _blackScreenImage;

    private RectTransform _circleRect;
    private RectTransform _canvasRect;

    public void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        _circleRect = GetComponent<RectTransform>();
        _fondNoirRect = _circleRect.GetChild(0).GetComponent<RectTransform>();
        _blackScreenImage = _circleRect.GetComponentInChildren<CutoutMaskUi>();
        _canvasRect = _circleRect.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    private void Start()
    {
        ResetCercle();

        _fondNoirRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _canvasRect.rect.size.x);
        _fondNoirRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _canvasRect.rect.size.y);
    }

    public void FadeIn(float transparancy, float time)
    {
        if (_currentCoroutine != null) 
            StopCoroutine(_currentCoroutine);
        _currentCoroutine = StartCoroutine(Fade(0, transparancy, time, true));
    }

    public void FadeOut(float time)
    {
        if (_currentCoroutine != null) 
            StopCoroutine(_currentCoroutine);
        _currentCoroutine = StartCoroutine(Fade(_canvasGroup.alpha, 0, time, false));
    }

    public void SetAlpha(float alpha)
    {
        _canvasGroup.alpha = alpha;
    }

    public IEnumerator Fade(float start, float end, float duration,bool isEnable)
    {

        float clock = 0f;

        if (isEnable)
            Helpers.EnabledCanvasGroup(_canvasGroup, false);

        while (clock < duration)
        {
            clock += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(start, end, clock / duration);
            yield return null;
        }

        if (!isEnable)
            Helpers.DisabledCanvasGroup(_canvasGroup);
    }

    public void SetCercle(Vector3 position, float size)
    {
        Vector2 localPoint;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(_circleRect.parent as RectTransform,position,null,out localPoint))
        {
            localPoint.x += (_circleRect.pivot.x - 0.5f) * _circleRect.rect.width;
            localPoint.y += (_circleRect.pivot.y - 0.5f) * _circleRect.rect.height;

            _circleRect.localPosition = new Vector3(localPoint.x, localPoint.y, _circleRect.localPosition.z);
        }

        _circleRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
        _circleRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);

        UpdateFondNoir();
    }

    public void ResetCercle()
    {
        _circleRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
        _circleRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

        UpdateFondNoir();
    }

    private void UpdateFondNoir()
    {
        _fondNoirRect.localPosition = -_circleRect.localPosition;

        _blackScreenImage.RefreshMaterial();
        _blackScreenImage.SetMaterialDirty();
    }
}
