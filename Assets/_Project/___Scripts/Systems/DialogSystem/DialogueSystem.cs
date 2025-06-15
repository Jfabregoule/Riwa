using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueSystem : Singleton<DialogueSystem>
{
    [SerializeField] private Sequencer _beginSequencer;
    [SerializeField] private Sequencer _transiUISequencer;
    [SerializeField] private Sequencer _endSequencer;
    public DialogueEventRegistry EventRegistery { get; private set; }

    public delegate void DialogueText(DialogueUIType uIType, string text);
    public event DialogueText OnSentenceChanged;

    public delegate void DialogueCanvasGroup(DialogueUIType uIType, bool isActive);
    public event DialogueCanvasGroup OnCanvasGroupChanged;

    public delegate void DialogueCanvasGroupAlpha(DialogueUIType uIType, float alpha);
    public event DialogueCanvasGroupAlpha OnCanvasGroupAlphaChanged;

    public DialogueAsset ProcessingDialogue { get; private set; }

    public bool IsInDialogue { get; private set; }

    private int _sectionIndex;
    private int _sentenceIndex;
    private int _characterIndex;

    private bool _isWriting = false;
    private bool _eventWaitwasTrigger = false;
    private DialogueUIType _currentDialogueUI;

    public delegate void DialogueEvent(DialogueEventType eventType);
    public event DialogueEvent OnDialogueEvent;

    public void Start()
    {
        ProcessingDialogue = null;
        IsInDialogue = false;
        EventRegistery = new DialogueEventRegistry();

        _beginSequencer.Init();
        _transiUISequencer.Init();
        _endSequencer.Init();
    }

    //private void OnEnable()
    //{
    //    StartCoroutine(Helpers.WaitMonoBeheviour(() => InputManager.Instance, SubscribeToDialogueInputManager));
    //}

    private void OnDisable() 
    { 
        if (InputManager.Instance != null) 
            InputManager.Instance.OnAdvanceDialogue -= AdvanceDialogue;

        if (GameManager.Instance != null)
            GameManager.Instance.TranslateSystem.OnLanguageChanged -= UpdateSentenceAll;
    }

    public void BeginDialogue(DialogueAsset asset)
    {
        if (asset == null)
        {
            Debug.LogWarning("Dialogue started with unspecified or invalid Dialogue Asset !");
            return;
        }
        ProcessingDialogue = asset;
        _sectionIndex = 0;
        _sentenceIndex = 0;
        _characterIndex = 0;

        _currentDialogueUI = GetSection().UIType;
        GameManager.Instance.TranslateSystem.OnLanguageChanged += UpdateSentenceAll;
        InputManager.Instance.OnAdvanceDialogue += AdvanceDialogue;


        if (ProcessingDialogue.OpeningTriggerEvent)
            OnDialogueEvent?.Invoke(ProcessingDialogue.OpeningEventType);

        if (ProcessingDialogue.DisablePlayerInputsOnOpening)
        {
            InputManager.Instance?.DisableGameplayControls();
            //Disable EventSystem
        }
        IsInDialogue = true;
        _beginSequencer.InitializeSequence();
    }

    public void EndDialogue()
    {
        GameManager.Instance.TranslateSystem.OnLanguageChanged -= UpdateSentenceAll;
        InputManager.Instance.OnAdvanceDialogue -= AdvanceDialogue;

        if (!ProcessingDialogue) return;

        if (ProcessingDialogue.ClosureTriggerEvent)
            OnDialogueEvent?.Invoke(ProcessingDialogue.ClosureEventType);

        IsInDialogue = false;
    }

    public void AdvanceDialogue()
    {
        EventRegistery.Unregister(GetSentence().WaitEventType, AdvanceDialogue);
        if (_isWriting)
        {
            _isWriting = false;
            UpdateSentenceAll();
            return;
        }

        _sentenceIndex++;
        if (_sentenceIndex >= GetSentenceCount())
        {
            EndSection();
            UpdateSection();
        }
        else
        {
            UpdateSentence();
        }
    }

    public void FirstSection()
    {
        _sectionIndex = 0;
        UpdateSection();
    }

    private void EndSection()
    {
        DialogueSection section = GetSection();
        if (section.TriggerEvent)
            OnDialogueEvent?.Invoke(section.EventType);
        _sectionIndex++;
    }

    private void UpdateSection()
    {
        if (_sectionIndex >= GetSectionCount())
        {
            _endSequencer.InitializeSequence();
            StopAllCoroutines();
            return;
        }

        _sentenceIndex = 0;
        DialogueSection section = GetSection();

        if (_currentDialogueUI != section.UIType)
        {
            _transiUISequencer.InitializeSequence();
            return;
        }

        UpdateSentence();
    }

    public void UpdateSentence()
    {
        DialogueSentence sentence = GetSentence();

        if ((sentence.Options & DialogueOptions.DisableDialogueInputs) != 0)
        {
            InputManager.Instance?.DisableDialogueControls();
        }
        else
        {
            InputManager.Instance?.EnableDialogueControls();
        }

        if ((sentence.Options & DialogueOptions.WaitEvent) != 0)
        {
            EventRegistery.Register(sentence.WaitEventType, WaitEventTrigger);
        }

        if ((sentence.Options & DialogueOptions.UseWriting) != 0)
        {
            StartCoroutine(UpdateSentenceWriting());
        }
        else
        {
            UpdateSentenceAll();
        }
    }

    private void UpdateSentenceAll()
    {
        DialogueSentence sentence = GetSentence();
        OnSentenceChanged?.Invoke(_currentDialogueUI, GetTranslateText());
        StopAllCoroutines();
        CheckToPassSentence(sentence);
    }

    private void WaitEventTrigger()
    {
        _eventWaitwasTrigger = true;
        EventRegistery.Unregister(GetSentence().WaitEventType, WaitEventTrigger);
    }

    private void UpdateSentenceTranslate()
    {
        OnSentenceChanged?.Invoke(_currentDialogueUI, GetTranslateText());
    }

    private IEnumerator UpdateSentenceWriting()
    {
        _characterIndex = 0;
        _isWriting = true;
        DialogueSentence sentence = GetSentence();
        string text = "";
        for (int i = 0; i < GetTranslateText().Length; i++)
        {
            text += GetCharacter();
            OnSentenceChanged?.Invoke(_currentDialogueUI, text);
            _characterIndex++;
            yield return Helpers.GetWait(sentence.SpeedWriting);
        }
        _isWriting = false;

        StopAllCoroutines();
        CheckToPassSentence(sentence);
    }

    private void CheckToPassSentence(DialogueSentence sentence)
    {
        if ((sentence.Options & DialogueOptions.UseTime) != 0)
        {
            StartCoroutine(WaitTimeAndAdvanceDialogue(sentence.TimeToPass));
        }

        if (_eventWaitwasTrigger)
        {
            _eventWaitwasTrigger = false;
            AdvanceDialogue();
        }
        else if ((sentence.Options & DialogueOptions.WaitEvent) != 0)
        {
            _eventWaitwasTrigger = false;
            EventRegistery.Unregister(GetSentence().WaitEventType, WaitEventTrigger);
            EventRegistery.Register(sentence.WaitEventType, AdvanceDialogue);
        }

        //if ((sentence.Options & DialogueOptions.WaitEvent) != 0)
        //{
        //    EventRegistery.Register(sentence.WaitEventType, AdvanceDialogue);
        //}
    }

    private IEnumerator WaitTimeAndAdvanceDialogue(float time)
    {
        yield return Helpers.GetWait(time);

        AdvanceDialogue();
    }

    //private void SubscribeToDialogueInputManager(InputManager script)
    //{
    //    if (script != null)
    //    {
    //        script.OnAdvanceDialogue += AdvanceDialogue;
    //        Debug.Log("Script is ready!");
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Script was still null after timeout.");
    //    }
    //}

    public void SkipAll()
    {
        if (ProcessingDialogue.IsAllSkipable)
        {
            FinishDialogue();
        }
    }

    public void FinishDialogue()
    {
        _endSequencer.InitializeSequence();
        StopAllCoroutines();
    }

    public void Reset()
    {
        OnSentenceChanged?.Invoke(_currentDialogueUI, "");
        OnCanvasGroupChanged?.Invoke(_currentDialogueUI, false);
    }

    public void ResetText()
    {
        OnSentenceChanged?.Invoke(_currentDialogueUI, "");
    }

    public void ChangeCanvasGroupAlpha(float alpha)
    {
        OnCanvasGroupAlphaChanged?.Invoke(_currentDialogueUI, alpha);
    }

    public void ChangeUI()
    {
        _currentDialogueUI = GetSection().UIType;
    }

    private int GetSectionCount() => ProcessingDialogue.Sections.Length;
    private int GetSentenceCount() => GetSection().Sentences.Length;
    private DialogueSection GetSection() => ProcessingDialogue.Sections[_sectionIndex];
    private DialogueSentence GetSentence() => GetSection().Sentences[_sentenceIndex];
    private char GetCharacter() => GetTranslateText()[_characterIndex];

    private string GetTranslateText() => GetSentence().TextTranslate.GetText(GameManager.Instance.TranslateSystem.GetCurrentLanguage());
}
