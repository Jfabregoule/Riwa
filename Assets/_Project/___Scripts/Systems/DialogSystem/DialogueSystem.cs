using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using static Unity.VisualScripting.Icons;

public class DialogueSystem : Singleton<DialogueSystem>
{
    [SerializeField] private Sequencer _beginSequencer;
    [SerializeField] private Sequencer _endSequencer;

    [SerializeField] private DialogueAsset _test;

    public delegate void DialogueText(string text);
    public event DialogueText OnSentenceChanged;

    public delegate void DialogueCanvasGroup(bool isActive);
    public event DialogueCanvasGroup OnCanvasGroupChanged;

    public delegate void DialogueCanvasGroupAlpha(float alpha);
    public event DialogueCanvasGroupAlpha OnCanvasGroupAlphaChanged;

    public DialogueAsset ProcessingDialogue { get; private set; }

    private int _sectionIndex;
    private int _sentenceIndex;
    private int _characterIndex;

    private bool _isWriting = false;

    public delegate void DialogueEvent(DialogueEventType eventType);
    public event DialogueEvent OnDialogueEvent;

    public void Start()
    {
        ProcessingDialogue = null;

        BeginDialogue(_test);

        _beginSequencer.Init();
        _endSequencer.Init();
    }

    private void OnEnable()
    {
        StartCoroutine(Helpers.WaitMonoBeheviour(() => InputManager.Instance, SubscribeToDialogueInputManager));
        StartCoroutine(Helpers.WaitMonoBeheviour(() => GameManager.Instance.TranslateSystem, SubscribeToTranslateSystem));
    }

    private void OnDisable() 
    { 
        if (InputManager.Instance != null) 
            InputManager.Instance.OnAdvanceDialogue -= AdvanceDialogue;

        if (GameManager.Instance != null)
            GameManager.Instance.TranslateSystem.OnLanguageChanged -= UpdateSentenceTranslate;
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

        if (ProcessingDialogue.OpeningTriggerEvent)
            OnDialogueEvent?.Invoke(ProcessingDialogue.OpeningEventType);

        if (ProcessingDialogue.DisablePlayerInputsOnOpening)
        {
            InputManager.Instance?.DisableGameplayControls();
            //Disable EventSystem
        }

        _beginSequencer.InitializeSequence();
    }

    public void EndDialogue()
    {
        if (ProcessingDialogue.ClosureTriggerEvent)
            OnDialogueEvent?.Invoke(ProcessingDialogue.ClosureEventType);
    }

    private void AdvanceDialogue()
    {
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
            if (GetSentence().UseWriting)
            {
                StartCoroutine(UpdateSentence());
            }
            else
            {
                UpdateSentenceAll();
            }
        }
            
    }

    public void StartSection()
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

        if (section.DisableDialogueInputs)
        {
            InputManager.Instance?.DisableDialogueControls();
        }
        else
        {
            InputManager.Instance?.EnableDialogueControls();
        }

        if (GetSentence().UseWriting)
        {
            StartCoroutine(UpdateSentence());
        }
        else
        {
            UpdateSentenceAll();
        }
    }

    private void UpdateSentenceAll()
    {
        DialogueSentence sentence = GetSentence();
        OnSentenceChanged?.Invoke(GetTranslateText());
        StopAllCoroutines();
        if (sentence.UseTime)
        {
            StartCoroutine(WaitTimeAndAdvanceDialogue(sentence.TimeToPass));
        }
    }

    private void UpdateSentenceTranslate()
    {
        OnSentenceChanged?.Invoke(GetTranslateText());
    }

    private IEnumerator UpdateSentence()
    {
        _characterIndex = 0;
        _isWriting = true;
        DialogueSentence sentence = GetSentence();
        string text = "";
        for (int i = 0; i < GetTranslateText().Length; i++)
        {
            text += GetCharacter();
            OnSentenceChanged?.Invoke(text);
            _characterIndex++;
            yield return Helpers.GetWait(sentence.SpeedWriting);
        }
        _isWriting = false;

        StopAllCoroutines();
        if (sentence.UseTime)
        {
            StartCoroutine(WaitTimeAndAdvanceDialogue(sentence.TimeToPass));
        }
    }

    private IEnumerator WaitTimeAndAdvanceDialogue(float time)
    {
        yield return Helpers.GetWait(time);

        AdvanceDialogue();
    }

    private void SubscribeToDialogueInputManager(InputManager script)
    {
        if (script != null)
        {
            script.OnAdvanceDialogue += AdvanceDialogue;
            Debug.Log("Script is ready!");
        }
        else
        {
            Debug.LogWarning("Script was still null after timeout.");
        }
    }

    private void SubscribeToTranslateSystem(TranslateSystem script)
    {
        if (script != null)
        {
            script.OnLanguageChanged += UpdateSentenceTranslate;
            Debug.Log("Script is ready!");
        }
        else
        {
            Debug.LogWarning("Script was still null after timeout.");
        }
    }

    public void SkipAll()
    {
        if (ProcessingDialogue.IsAllSkipable)
        {
            _endSequencer.InitializeSequence();
            StopAllCoroutines();
        }
    }

    public void Reset()
    {
        OnSentenceChanged?.Invoke("");
        OnCanvasGroupChanged?.Invoke(false);
    }

    public void ChangeCanvasGroupAlpha(float alpha)
    {
        
        OnCanvasGroupAlphaChanged?.Invoke(alpha);
    }

    private int GetSectionCount() => ProcessingDialogue.Sections.Length;
    private int GetSentenceCount() => GetSection().Sentences.Length;
    private DialogueSection GetSection() => ProcessingDialogue.Sections[_sectionIndex];
    private DialogueSentence GetSentence() => GetSection().Sentences[_sentenceIndex];
    private char GetCharacter() => GetTranslateText()[_characterIndex];

    private string GetTranslateText() => GetSentence().TextTranslate.GetText(GameManager.Instance.TranslateSystem.GetCurrentLanguage());
}
