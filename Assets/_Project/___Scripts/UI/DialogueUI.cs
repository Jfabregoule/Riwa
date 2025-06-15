using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private CanvasGroup _canvasGroup;
    [SerializeField] private DialogueUIType _type;

    private void WaitUIManager(UIManager manager)
    {
        if (manager != null)
        {
            manager.DialogueUIDispacher.RegisterUI(_type, this);
        }
    }

    private void Start()
    {
        StartCoroutine(Helpers.WaitMonoBeheviour(() => GameManager.Instance.UIManager, WaitUIManager));
        _canvasGroup = GetComponent<CanvasGroup>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void DisplayText(string sentence)
    {
        _text.SetText(sentence);
    }

    public void DisplayCanvasGroup(bool isActive)
    {
        Helpers.ToggleCanvasGroup(isActive, _canvasGroup);
    }

    public void AlphaCanvasGroup(float alpha)
    {
        _canvasGroup.alpha = alpha;
    }

    public void OnSkip()
    {
        DialogueSystem.Instance.SkipAll();
    }
}
