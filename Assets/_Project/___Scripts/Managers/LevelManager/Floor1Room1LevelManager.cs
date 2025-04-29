using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public enum EnumAdvancementRoom1
{
    None,
    LookAtTree,
    HasEnterTree,
    HasOpenDoor
}

public enum EnumAdvancementRoom1Test
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
    public EnumAdvancementRoom1Test ID;
    public DialogueAsset Dialogue;
    public Sequencer[] Sequencers;
}

public class Floor1Room1LevelManager : BaseLevelManager
{

    [Header("Floor1 Room1")]

    public EnumAdvancementRoom1 CurrentAdvancement = EnumAdvancementRoom1.None; //A sauvegarder
    public EnumAdvancementRoom1Test CurrentAdvancementTest = EnumAdvancementRoom1Test.Start; //A sauvegarder
    [SerializeField] private bool _areEventEnabled = true;

    [SerializeField] private float _durationCameraBlending = 1f;
    private CinemachineBrain _brain;
    private CinemachineBlendDefinition _defaultBlend;
    [SerializeField] private GameObject _riwaHeart;
    [SerializeField] private List<ParticleSystem> _riwaHeartPS;
    [SerializeField] private CinemachineVirtualCamera _endGameCamera;
    [SerializeField] private GameObject _evelator;

    [SerializeField] private CinematicRoom1[] _cinematics;

    [Header("Event Sequencer")]

    //[SerializeField] private Sequencer _endGame;
    //[SerializeField] private Sequencer _event3;

    //[SerializeField] private DialogueAsset _dialogue;

    private System.Action OnChangeTime;

    //Enigmes

    private bool _isCrateWellPlaced;

    public Sequencer EndGameSequencer { get => _cinematics[(int)EnumAdvancementRoom1Test.End].Sequencers[0]; }
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
        SaveSystem.Instance.SaveElement<int>("Room1Progress", (int)CurrentAdvancementTest);
    }

    private void LoadData()
    {
        UpdateAdvancement((EnumAdvancementRoom1Test)SaveSystem.Instance.LoadElement<int>("Room1Progress"));
    }

    public override void Start()
    {
        base.Start();
        //OnLevelEnter += BeginDialogue;

        if(CurrentAdvancementTest == EnumAdvancementRoom1Test.Start)
        {
            OnLevelEnter += BeginDialogue;
        }

        if (CurrentAdvancementTest > EnumAdvancementRoom1Test.Room0)
        {
            GameManager.Instance.UnlockChangeTime();
        }

        //_endGame.Init();
        DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.ChangeTime, OnChangeTime);

        foreach (var cam in _cameras)
        {
            CameraDictionnary[cam._id] = cam._camera;
        }

        _character.InputManager.OnChangeTime += CheckCrateEnigma;
        _character.InputManager.OnChangeTime += InvokeChangeTime;

        //if (CurrentAdvancement < EnumAdvancementRoom1.LookAtTree)
        //{
        //    CurrentAdvancement = EnumAdvancementRoom1.LookAtTree;

        //    if(_areEventEnabled)
        //    {
        //        _event3.Init();

        //        //_dialogue.LaunchDialogue(0);
        //    }
        //}

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

        if (CurrentAdvancement < EnumAdvancementRoom1.HasEnterTree)
        {
            CurrentAdvancement = EnumAdvancementRoom1.HasEnterTree;
            
            if (_areEventEnabled)
            {
                GameManager.Instance.Character.StateMachine.GoToIdle();
                UpdateAdvancement(EnumAdvancementRoom1Test.Liana);
                DialogueSystem.Instance.BeginDialogue(_cinematics[(int)CurrentAdvancementTest].Dialogue);
                _cinematics[(int)EnumAdvancementRoom1Test.Liana].Sequencers[0].InitializeSequence();
                //_event3.InitializeSequence();
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
                break;
            case DialogueEventType.UnlockChangeTime:
                GameManager.Instance.UnlockChangeTime();
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
        GameManager.Instance.PulseIndice();
        InputManager.Instance.EnableGameplayChangeTimeControls();
    }

    private void BeginDialogue()
    {
        //DialogueSystem.Instance.BeginDialogue(_dialogue);

        DialogueSystem.Instance.BeginDialogue(_cinematics[(int)CurrentAdvancementTest].Dialogue);
    }

    public void EnterFromRoom0()
    {
        if(CurrentAdvancementTest == EnumAdvancementRoom1Test.Start)
        {
            UpdateAdvancement(EnumAdvancementRoom1Test.Room0);
        }
    }

    public void EnterFromRoom4()
    {
        if (CurrentAdvancementTest == EnumAdvancementRoom1Test.Liana)
        {
            UpdateAdvancement(EnumAdvancementRoom1Test.Room4);
            OnLevelEnter += BeginDialogue;
        }
    }

    public void UpdateAdvancement(EnumAdvancementRoom1Test advancement)
    {
        CurrentAdvancementTest = advancement;
        foreach (Sequencer sequencer in _cinematics[(int)CurrentAdvancementTest].Sequencers)
        {
            sequencer.Init();
        }
    }

}
