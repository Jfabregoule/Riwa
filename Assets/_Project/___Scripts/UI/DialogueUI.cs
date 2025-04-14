using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private CanvasGroup _canvasGroup;
    void OnEnable()
    {
        StartCoroutine(Helpers.WaitMonoBeheviour(() => DialogueSystem.Instance, SubcribeToDialogueEvent));
    }

    private void OnDisable() 
    { 
        if (DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.OnSentenceChanged -= OnSentenceChange;
            DialogueSystem.Instance.OnCanvasGroupChanged -= OnCanvasGroupChange;
        } 
    }

    private void OnSentenceChange(string sentence)
    {
        _text.SetText(sentence);
    }

    private void OnCanvasGroupChange(bool isActive)
    {
        Helpers.ToggleCanvasGroup(isActive, _canvasGroup);
    }

    private void SubcribeToDialogueEvent(DialogueSystem dialogueSystem)
    {
        if (dialogueSystem != null)
        {
            dialogueSystem.OnSentenceChanged += OnSentenceChange;
            dialogueSystem.OnCanvasGroupChanged += OnCanvasGroupChange;
            Debug.Log("Script is ready!");
        }
        else
        {
            Debug.LogWarning("Script was still null after timeout.");
        }
    }
}
