using UnityEngine;

public class HighlightEffect : MonoBehaviour
{
    private BlackScreen _blackScreen;
    [SerializeField] private float _size;
    [SerializeField] private float _speed = 0.3f;
    [SerializeField] private float _transparancy = 0.95f;
    void Start()
    {
        StartCoroutine(Helpers.WaitMonoBeheviour(() => GameManager.Instance.UIManager, WaitUIManager));
    }

    public void StartHighlight()
    {
        RectTransform transform = gameObject.GetComponent<RectTransform>();
        Vector3 transformWorldPivotPos = transform.TransformPoint(new Vector3(
            (0.5f - transform.pivot.x) * transform.rect.width,
            (0.5f - transform.pivot.y) * transform.rect.height,
            0f
        ));
        _blackScreen.SetCercle(RectTransformUtility.WorldToScreenPoint(null, transformWorldPivotPos), _size);
        _blackScreen.FadeIn(_transparancy, _speed);
    }

    public void StopHighlight()
    {
        _blackScreen.FadeOut(_speed);

        _blackScreen.ResetCercle();
    }

    private void WaitUIManager(UIManager manager)
    {
        if (manager != null)
        {
            _blackScreen = manager.BlackScreen;
        }
    }
}
