using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public enum EnumAdvancementRoom1
{
    None,
    LookAtTree,
    HasEnterTree,
    HasOpenDoor
}

public class Floor1Room1LevelManager : BaseLevelManager
{

    [Header("Floor1 Room1")]

    public EnumAdvancementRoom1 CurrentAdvancement = EnumAdvancementRoom1.None; //A sauvegarder
    [SerializeField] private bool _areEventEnabled = true;

    [SerializeField] private float _durationCameraBlending = 1f;
    private CinemachineBrain _brain;
    private CinemachineBlendDefinition _defaultBlend;
    [SerializeField] private GameObject _riwaHeart;
    [SerializeField] private CinemachineVirtualCamera _endGameCamera;

    [Header("Event Sequencer")]

    [SerializeField] private Sequencer _endGame;
    [SerializeField] private Sequencer _event2;
    [SerializeField] private Sequencer _event3;

    private Floor1Room1Dialogue _dialogue;
    private System.Action OnChangeTime;

    //Enigmes

    private bool _isCrateWellPlaced;

    public Floor1Room1Dialogue Dialogue { get => _dialogue; set => _dialogue = value; }
    public Sequencer EndGameSequencer { get => _endGame; }
    public GameObject RiwaHeart { get => _riwaHeart; }
    public CinemachineVirtualCamera EndGameCamera { get => _endGameCamera; }

    public override void Start()
    {
        base.Start();
        //RiwaCinematicSystem.Instance.PlayVideoByKey("Starting Cinematic");
        _endGame.Init();
        _dialogue = GetComponent<Floor1Room1Dialogue>();
        DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.ChangeTime, OnChangeTime);

        foreach (var cam in _cameras)
        {
            CameraDictionnary[cam._id] = cam._camera;
        }

        _character.InputManager.OnChangeTime += CheckCrateEnigma;
        _character.InputManager.OnChangeTime += InvokeChangeTime;

        if (CurrentAdvancement < EnumAdvancementRoom1.LookAtTree)
        {
            CurrentAdvancement = EnumAdvancementRoom1.LookAtTree;

            if(_areEventEnabled)
            {
                _event2.Init();
                _event3.Init();

                //_dialogue.LaunchDialogue(0);
            }
        }

        _brain = Helpers.Camera.GetComponent<CinemachineBrain>();
        _defaultBlend = _brain.m_DefaultBlend;

        DialogueSystem.Instance.OnDialogueEvent += EventDispatcher;
    }

    public void OnDestroy()
    {
        _character.InputManager.OnChangeTime -= CheckCrateEnigma;
        _character.InputManager.OnChangeTime -= InvokeChangeTime;
        if (DialogueSystem.Instance)
            DialogueSystem.Instance.OnDialogueEvent -= EventDispatcher;
    }


    public void TriggerEvent2()
    {
        if (CurrentAdvancement < EnumAdvancementRoom1.HasEnterTree)
        { 
            CurrentAdvancement = EnumAdvancementRoom1.HasEnterTree;
            if (_areEventEnabled)
            {                 
                _event2.InitializeSequence();
            }
        }
    }

    public void SetCrateWellPlaced(bool value)
    {
        _isCrateWellPlaced = value;
    }

    public void CheckCrateEnigma()
    {
        if (!_isCrateWellPlaced || !_character.CanChangeTime) { return; }

        if (CurrentAdvancement < EnumAdvancementRoom1.HasEnterTree)
        {
            CurrentAdvancement = EnumAdvancementRoom1.HasEnterTree;
            
            if (_areEventEnabled)
            {
                _event3.InitializeSequence();
            }
        }

        _character.InputManager.OnChangeTime -= CheckCrateEnigma;

    }

    public void InvokeChangeTime() {
        if (!_character.CanChangeTime) return;
        DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.ChangeTime);
        _character.InputManager.OnChangeTime -= InvokeChangeTime;
    }

    public void EventDispatcher(DialogueEventType eventType)
    {
        switch (eventType)
        {
            case DialogueEventType.LookAtTreeRoom1:
                StartCoroutine(BlendingCamera(CameraDictionnary[EnumCameraRoom.LookAtTree]));
                break;
            case DialogueEventType.ResetCamRoom1:
                StartCoroutine(BlendingCamera(CameraDictionnary[EnumCameraRoom.Main]));
                StartCoroutine(WaitForPulse());
                break;
            case DialogueEventType.EnableChangeTime:
                break;
        }
    }

    public IEnumerator BlendingCamera(CinemachineVirtualCamera cam)
    {
        _brain.m_DefaultBlend = new CinemachineBlendDefinition(
            CinemachineBlendDefinition.Style.EaseInOut,
            _durationCameraBlending
        );
        (_brain.ActiveVirtualCamera as CinemachineVirtualCamera).Priority = 10;
        cam.Priority = 20;

        //On dirait que isBlending marche aps
        while (_brain.IsBlending)
        {
            yield return null;
        }

        _brain.m_DefaultBlend = _defaultBlend;
    }

    public IEnumerator WaitForPulse()
    {
        yield return new WaitForSeconds(1.5f);
        GameManager.Instance.PulseIndice();
        InputManager.Instance.EnableGameplayChangeTimeControls();

    }
}
