using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public enum EnumAdvancementRoom1
{
    Start,
    Room0,
    Liana,
    Room4,
    End
}

[System.Serializable]
public struct CinematicRoom1
{
    public EnumAdvancementRoom1 ID;
    public DialogueAsset Dialogue;
    public Sequencer[] Sequencers;
}

public class Floor1Room1LevelManager : BaseLevelManager
{

    [Header("Floor1 Room1")]

    public EnumAdvancementRoom1 CurrentAdvancement = EnumAdvancementRoom1.Start; //A sauvegarder

    [SerializeField] private float _durationCameraBlending = 1f;
    private CinemachineBrain _brain;
    private CinemachineBlendDefinition _defaultBlend;
    [SerializeField] private GameObject _riwaHeart;
    [SerializeField] private List<ParticleSystem> _riwaHeartPS;
    [SerializeField] private CinemachineVirtualCamera _endGameCamera;
    [SerializeField] private GameObject _evelator;

    [SerializeField] private Door _backTrakingDoor;
    [SerializeField] private GameObject _blockDoor;
    [SerializeField] private Renderer _aroundDoor;

    [SerializeField] private CinematicRoom1[] _cinematics;

    [Header("Event Sequencer")]

    private System.Action OnChangeTime;

    //Enigmes

    private bool _isCrateWellPlaced;

    public Sequencer EndGameSequencer { get => _cinematics[(int)EnumAdvancementRoom1.End].Sequencers[0]; }
    public GameObject RiwaHeart { get => _riwaHeart; }
    public CinemachineVirtualCamera EndGameCamera { get => _endGameCamera; }
    public List<ParticleSystem> RiwaHeartPS { get => _riwaHeartPS; }
    public GameObject Evelator { get => _evelator; set => _evelator = value; }

    public override void OnEnable()
    {
        base.OnEnable();
        LoadData();
        SaveSystem.Instance.OnLoadProgress += LoadData;
    }

    private void OnDisable()
    {
        SaveSystem.Instance.OnLoadProgress -= LoadData;
        SaveSystem.Instance.SaveElement<int>("Room1Progress", (int)CurrentAdvancement);
    }

    private void LoadData()
    {
        UpdateAdvancement((EnumAdvancementRoom1)SaveSystem.Instance.LoadElement<int>("Room1Progress"));
    }

    public override void Start()
    {
        base.Start();
        _backTrakingDoor.DisableDoor();
        if (CurrentAdvancement == EnumAdvancementRoom1.Start)
        {
            OnLevelEnter += BeginDialogue;
        }

        if (CurrentAdvancement >= EnumAdvancementRoom1.Room0)
        {
            GameManager.Instance.UnlockChangeTime();
        }
        DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.ChangeTime, OnChangeTime);

        foreach (var cam in _cameras)
        {
            CameraDictionnary[cam._id] = cam._camera;
        }

        _character.InputManager.OnChangeTime += CheckCrateEnigma;
        _character.InputManager.OnChangeTime += InvokeChangeTime;

        _brain = Helpers.Camera.GetComponent<CinemachineBrain>();
        _defaultBlend = _brain.m_DefaultBlend;

        DialogueSystem.Instance.OnDialogueEvent += EventDispatcher;
    }

    public void OnDestroy()
    {
        _character.InputManager.OnChangeTime -= CheckCrateEnigma;
        _character.InputManager.OnChangeTime -= InvokeChangeTime;
        OnLevelEnter -= BeginDialogue;
        if (DialogueSystem.Instance)
            DialogueSystem.Instance.OnDialogueEvent -= EventDispatcher;
    }

    public void SetCrateWellPlaced(bool value)
    {
        _isCrateWellPlaced = value;
    }

    public void CheckCrateEnigma()
    {
        if (!_isCrateWellPlaced || !_character.CanChangeTime) { return; }

            
        if (CurrentAdvancement == EnumAdvancementRoom1.Room0)
        {
            GameManager.Instance.Character.StateMachine.GoToIdle();
            UpdateAdvancement(EnumAdvancementRoom1.Liana);
            DialogueSystem.Instance.BeginDialogue(_cinematics[(int)CurrentAdvancement].Dialogue);
            _cinematics[(int)EnumAdvancementRoom1.Liana].Sequencers[0].InitializeSequence();
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
                break;
            case DialogueEventType.UnlockChangeTime:
                GameManager.Instance.UnlockChangeTime();
                GameManager.Instance.PulseChangeTime();
                break;
            case DialogueEventType.ShowInput:
                GameManager.Instance.InvokeBasicInput();
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
        GameManager.Instance.PulseChangeTime();
        InputManager.Instance.EnableGameplayChangeTimeControls();
    }

    private void BeginDialogue()
    {
        DialogueSystem.Instance.BeginDialogue(_cinematics[(int)CurrentAdvancement].Dialogue);
    }

    public void EnterFromRoom0()
    {
        if(CurrentAdvancement == EnumAdvancementRoom1.Start)
        {
            UpdateAdvancement(EnumAdvancementRoom1.Room0);
        }
    }

    public void EnterFromRoom4()
    {
        if (CurrentAdvancement == EnumAdvancementRoom1.Liana)
        {
            UpdateAdvancement(EnumAdvancementRoom1.Room4);
            OnLevelEnter += BeginDialogue;
            _backTrakingDoor.EnableDoor();
            _blockDoor.SetActive(false);
            if (_aroundDoor.material.HasProperty("_IsActivated"))
                _aroundDoor.material.SetFloat("_IsActivated", 1);
        }
    }

    public void UpdateAdvancement(EnumAdvancementRoom1 advancement)
    {
        CurrentAdvancement = advancement;
        foreach (Sequencer sequencer in _cinematics[(int)CurrentAdvancement].Sequencers)
        {
            sequencer.Init();
        }
    }

}
