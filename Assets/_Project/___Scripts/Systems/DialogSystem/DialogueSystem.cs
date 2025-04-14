using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueSystem : Singleton<DialogueSystem>
{
    private const string DIALOGUE_TEXT_NAME = "SentenceText";

    [SerializeField] private Sequencer _beginSequencer;
    [SerializeField] private Sequencer _endSequencer;

    public CanvasGroup FadeCanvas { get; private set; }
    public TextMeshProUGUI DialogueText { get; private set; }
    public DialogueAsset ProcessingDialogue { get; private set; }

    private int sectionIndex;
    private int sentenceIndex;

    public delegate void DialogueEvent(DialogueEventType eventType);
    public event DialogueEvent OnDialogueEvent;

    public void Init()
    {
        ProcessingDialogue = null;

        //FadeCanvas = GameManager.Instance.DialogueUI.GetComponent<CanvasGroup>();
        //DialogueText = GameManager.Instance.DialogueUI.transform.Find(DIALOGUE_TEXT_NAME).GetComponent<TextMeshProUGUI>();

        _beginSequencer.Init();
        _endSequencer.Init();
    }

    private void OnEnable()
    {
        //MonoBehaviour test = StartCoroutine(Helpers.WaitMonoBeheviour(InputManager.Instance));
        StartCoroutine(SubscribeDialogueInputManager());
        //InputManager.Instance.OnAdvanceDialogue += AdvanceDialogue;
    }

    private void OnDisable() { if (InputManager.Instance != null) InputManager.Instance.OnAdvanceDialogue -= AdvanceDialogue; }

    public void BeginDialogue(DialogueAsset asset)
    {
        if (asset == null)
        {
            Debug.LogWarning("Dialogue started with unspecified or invalid Dialogue Asset !");
            return;
        }
        ProcessingDialogue = asset;
        sectionIndex = 0;
        sentenceIndex = 0;

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
        sentenceIndex++;
        if (sentenceIndex >= GetSentenceCount())
            UpdateSection();
        else
            UpdateSentence();
    }

    private void UpdateSection()
    {
        sentenceIndex = 0;
        DialogueSection section = GetSection();
        if (section.TriggerEvent)
            OnDialogueEvent?.Invoke(section.EventType);

        if (section.DisableDialogueInputs)
        {
            InputManager.Instance?.DisableDialogueControls();
        }
        else
        {
            InputManager.Instance?.EnableDialogueControls();
        }

        sectionIndex++;
        if (sectionIndex >= GetSectionCount())
            _endSequencer.InitializeSequence();
        else
            UpdateSentence();
    }

    public void UpdateSentence()
    {
        string sentence = GetSentence();
        DialogueText.SetText(sentence);

        StartCoroutine(WaitTime(1f));
    }

    private IEnumerator WaitTime(float time)
    {
        yield return Helpers.GetWait(time);
    }

    private IEnumerator SubscribeDialogueInputManager()
    {
        while (InputManager.Instance == null)
            yield return null;

        InputManager.Instance.OnAdvanceDialogue += AdvanceDialogue;
    }

    private int GetSectionCount() => ProcessingDialogue.Sections.Length;
    private int GetSentenceCount() => ProcessingDialogue.Sections[sectionIndex].Sentences.Length;
    private DialogueSection GetSection() => ProcessingDialogue.Sections[sectionIndex];
    private string GetSentence() => GetSection().Sentences[sentenceIndex];
}
