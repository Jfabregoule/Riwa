using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChangeTime : MonoBehaviour
{
    [SerializeField] private bool _isRight;

    private CanvasGroup _canvasGroup;
    private Control _control;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(Helpers.WaitMonoBeheviour(() => GameManager.Instance.UIManager, WaitUIManager));
        GameManager.Instance.OnResetSave += Reset;
        GameManager.Instance.OnUnlockChangeTime += Display;
    }

    private void OnDisable()
    {
        if (GameManager.Instance)
        {
            GameManager.Instance.OnUnlockChangeTime -= Display;
            GameManager.Instance.OnResetSave -= Reset;
        }
    }

    private void Display()
    {
        if (_control.IsRightHanded == _isRight)
        {
            Helpers.EnabledCanvasGroup(_canvasGroup);
        } 
    }

    private void Reset() 
    {
        Helpers.DisabledCanvasGroup(_canvasGroup);
    }

    private void WaitUIManager(UIManager manager)
    {
        if (manager != null)
        {
            _control = manager.Control;
        }
    }
}
