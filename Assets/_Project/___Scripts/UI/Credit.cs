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
    private void OnEnable()
    {
        StartCoroutine(Helpers.WaitMonoBeheviour(() => GameManager.Instance, SubscribeToGameManager));
    }

    private void OnDisable()
    {
        if(GameManager.Instance)
            GameManager.Instance.OnCredit += ToggleCredit;
    }
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
        _rectTransform.anchoredPosition = Vector2.zero;
        string currentRoomName = RiwaLoadSceneSystem.Instance.GetCurrentRoomSceneName();
        StartCoroutine(RiwaLoadSceneSystem.Instance.ChangeScene(new[] { new SceneData(currentRoomName) }, new[] { new SceneData("MainMenu") }));
        Helpers.DisabledCanvasGroup(_creditCanvasGroup);
    }

    public void ToggleCredit()
    {
        _isEnable = true;
        Helpers.EnabledCanvasGroup(_creditCanvasGroup);
    }

    private void SubscribeToGameManager(GameManager manager)
    {
        if (manager == null) return;
        manager.OnCredit += ToggleCredit;
    }
}
