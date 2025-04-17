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
    //private DialogueSystem _dialogueSystem;
    void OnEnable()
    {
        //StartCoroutine(Helpers.WaitMonoBeheviour(() => DialogueSystem.Instance, SubcribeToDialogueEvent));
    }

    private void OnDisable() 
    { 
        //if (_dialogueSystem != null)
        //{
        //    //_dialogueSystem.OnSentenceChanged -= OnSentenceChange;
        //    _dialogueSystem.OnCanvasGroupChanged -= OnCanvasGroupChange;
        //    _dialogueSystem.OnCanvasGroupAlphaChanged -= OnCanvasGroupAlphaChange;
        //} 
    }

    private void Start()
    {
        GetComponentInParent<DialogueUIDispacher>().RegisterUI(_type, this);
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

    private void OnSentenceChange(string sentence)
    {
        _text.SetText(sentence);
    }

    private void OnCanvasGroupChange(bool isActive)
    {
        Helpers.ToggleCanvasGroup(isActive, _canvasGroup);
    }

    private void OnCanvasGroupAlphaChange(float alpha)
    {
        _canvasGroup.alpha = alpha;
    }

    public void OnSkip()
    {
        DialogueSystem.Instance.SkipAll();
    }

    //private void SubcribeToDialogueEvent(DialogueSystem dialogueSystem)
    //{
    //    if (dialogueSystem != null)
    //    {
    //        _dialogueSystem = dialogueSystem;
    //        //dialogueSystem.OnSentenceChanged += OnSentenceChange;
    //        //dialogueSystem.OnCanvasGroupChanged += OnCanvasGroupChange;
    //        //dialogueSystem.OnCanvasGroupAlphaChanged += OnCanvasGroupAlphaChange;
    //        Debug.Log("Script is ready!");
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Script was still null after timeout.");
    //    }
    //}
}
