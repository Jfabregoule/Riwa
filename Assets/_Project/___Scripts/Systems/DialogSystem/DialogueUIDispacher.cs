using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUIDispacher : MonoBehaviour
{
    private Dictionary<DialogueUIType, DialogueUI> _registeredUIs;
    private DialogueSystem _dialogueSystem;

    private void OnEnable()
    {
        StartCoroutine(Helpers.WaitMonoBeheviour(() => DialogueSystem.Instance, SubcribeToDialogueEvent));
        //DialogueSystem.Instance.OnSentenceChanged += OnSentenceChanged;
    }

    private void OnDisable()
    {
        if (_dialogueSystem != null)
            _dialogueSystem.OnSentenceChanged -= OnSentenceChanged;
    }
    void Awake()
    {
        _registeredUIs = new Dictionary<DialogueUIType, DialogueUI>();
    }

    public void RegisterUI(DialogueUIType uiEnum, DialogueUI ui)
    {
        if (!_registeredUIs.ContainsKey(uiEnum))
        {
            _registeredUIs.Add(uiEnum, ui);
        }
    }

    public void UnregisterUI(DialogueUIType uiEnum)
    {
        _registeredUIs.Remove(uiEnum);
    }

    private void OnSentenceChanged(DialogueUIType targetUI, string sentence)
    {
        if (_registeredUIs.TryGetValue(targetUI, out var ui))
        {
            ui.DisplayText(sentence);
        }
    }

    private void OnCanvasGroupChange(DialogueUIType targetUI, bool isActive)
    {
        if (_registeredUIs.TryGetValue(targetUI, out var ui))
        {
            ui.DisplayCanvasGroup(isActive);
        }
    }

    private void OnCanvasGroupAlphaChanged(DialogueUIType targetUI, float alpha)
    {
        if (_registeredUIs.TryGetValue(targetUI, out var ui))
        {
            ui.AlphaCanvasGroup(alpha);
        }
    }

    private void SubcribeToDialogueEvent(DialogueSystem dialogueSystem)
    {
        if (dialogueSystem != null)
        {
            _dialogueSystem = dialogueSystem;
            dialogueSystem.OnSentenceChanged += OnSentenceChanged;
            dialogueSystem.OnCanvasGroupChanged += OnCanvasGroupChange;
            dialogueSystem.OnCanvasGroupAlphaChanged += OnCanvasGroupAlphaChanged;
        }
        else
        {
            Debug.LogWarning("Script was still null after timeout.");
        }
    }
}

public enum DialogueUIType
{
    Basis,
    Other,
    BlackScreen
}
