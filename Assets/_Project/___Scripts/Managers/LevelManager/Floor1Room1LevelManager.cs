using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.TextCore.Text;

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
    [SerializeField] private CinemachineVirtualCamera _lianaCamera;
    [SerializeField] private CinemachineVirtualCamera _crateCamera;
    [SerializeField] private GameObject _evelator;

    [SerializeField] private Door _backTrakingDoor;
    [SerializeField] private GameObject _blockDoor;
    [SerializeField] private Renderer _aroundDoor;

    [SerializeField] private CinematicRoom1[] _cinematics;
    [SerializeField] private PlacementZone[] _zones;
    private int _currentZone;

    [Header("Event Sequencer")]

    private System.Action OnChangeTime;
    private System.Action OnMove;
    private System.Action OnInteract;
    private System.Action OnPull;
    private System.Action OnPush;
    private System.Action OnEndInteract;
    private System.Action OnPlayerInzone;
    private System.Action OnBoxInzone;

    //Enigmes

    private bool _isCrateWellPlaced;

    public Sequencer EndGameSequencer { get => _cinematics[(int)EnumAdvancementRoom1.End].Sequencers[0]; }
    public GameObject RiwaHeart { get => _riwaHeart; }
    public CinemachineVirtualCamera EndGameCamera { get => _endGameCamera; }
    public CinemachineVirtualCamera LianaCamera { get => _lianaCamera; }
    public CinemachineVirtualCamera CrateCamera { get => _crateCamera; }
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

        //foreach(PlacementZone zone in _zones)
        //{
        //    zone.OnPlace -= WaitPlayerInZone;
        //}
        _zones[0].OnPlace -= PlayerInZone;

        if (GameManager.Instance)
            GameManager.Instance.OnTimeChangeEnded -= OnTimeChangeEndedHandler;
    }

    private void LoadData()
    {
        UpdateAdvancement((EnumAdvancementRoom1)SaveSystem.Instance.LoadElement<int>("Room1Progress"));
    }

    public override void Start()
    {
        base.Start();
        
        if (CurrentAdvancement == EnumAdvancementRoom1.Start)
        {
            OnLevelEnter += BeginDialogue;
            _currentZone = 0;
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

        _character.InputManager.OnMove += InvokeMove;
        //_character.InputManager.OnInteract += InvokeInteract;
        //_character.InputManager.OnInteract += InvokeEndInteract;
        //_character.InputManager.OnPull += InvokePull;
        //_character.InputManager.OnPush += InvokePush;

        _brain = Helpers.Camera.GetComponent<CinemachineBrain>();
        _defaultBlend = _brain.m_DefaultBlend;

        DialogueSystem.Instance.OnDialogueEvent += EventDispatcher;
    }

    public void OnDestroy()
    {
        _character.InputManager.OnChangeTime -= CheckCrateEnigma;
        _character.InputManager.OnChangeTime -= InvokeChangeTime;
        _character.InputManager.OnMove -= InvokeMove;
        _character.InputManager.OnInteract -= InvokeInteract;
        _character.InputManager.OnInteract -= InvokeEndInteract;
        _character.InputManager.OnPull -= InvokePull;
        _character.InputManager.OnPush -= InvokePush;
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
        if (!_isCrateWellPlaced || !_character.CanChangeTime || GameManager.Instance.CurrentTemporality == EnumTemporality.Present) { return; }
        
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

    public void InvokeMove(Vector2 position)
    {
        DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.Move);
        GameManager.Instance.UIManager.StopHighlight(UIElementEnum.Joystick);
        InputManager.Instance.UnlockJoystick();
        _character.InputManager.OnMove -= InvokeMove;
        _zones[_currentZone].gameObject.SetActive(true);
        _zones[_currentZone].OnPlace += PlayerInZone;
        DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.PlayerInZone, OnPlayerInzone);
    }

    private void InvokeInteract()
    {
        DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.Interact);
        GameManager.Instance.UIManager.StopHighlight(UIElementEnum.Interact);
        _character.InputManager.OnInteract -= InvokeInteract;
    }

    private void InvokeEndInteract()
    {
        DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.EndInteract);
        GameManager.Instance.UIManager.StopHighlight(UIElementEnum.Interact);
        _character.InputManager.OnInteract -= InvokeEndInteract;
    }

    private void InvokePull()
    {
        DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.Pull);
        GameManager.Instance.UIManager.StopHighlight(UIElementEnum.Pull);
        _character.InputManager.OnPull -= InvokePull;
    }

    private void InvokePush()
    {
        InputManager.Instance.EnableGameplayControls();
        DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.Push);
        GameManager.Instance.UIManager.StopHighlight(UIElementEnum.Push);
        _character.InputManager.OnPush -= InvokePush;
    }

    private void PlayerInZone(GameObject go)
    {
        if (go.TryGetComponent(out ACharacter charcter))
        {
            _zones[_currentZone].OnPlace -= PlayerInZone;
            _zones[_currentZone].gameObject.SetActive(false);
            DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.PlayerInZone);
            _currentZone += 1;
            if (_currentZone < _zones.Length)
            {
                _zones[_currentZone].gameObject.SetActive(true);
                _zones[_currentZone].OnPlace += BoxInZone;
                DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.BoxInZone, OnBoxInzone);
            }
        }
    }

    private void BoxInZone(GameObject go)
    {
        if (go.TryGetComponent(out Crate box))
        {
            _zones[_currentZone].OnPlace -= BoxInZone;
            _zones[_currentZone].gameObject.SetActive(false);
            DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.BoxInZone);
            _currentZone += 1;
            if(_currentZone < _zones.Length)
            {
                _zones[_currentZone].gameObject.SetActive(true);
                _zones[_currentZone].OnPlace += PlayerInZone;
                DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.PlayerInZone, OnPlayerInzone);
            }
        }
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
                GameManager.Instance.UIManager.StartPulse(UIElementEnum.ChangeTime);
                GameManager.Instance.OnTimeChangeEnded += OnTimeChangeEndedHandler;
                break;
            case DialogueEventType.DisplayJoystick:
                InputManager.Instance.EnableGameplayMoveControls();
                InputManager.Instance.LockJoystick();
                DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.Move, OnMove);
                GameManager.Instance.UIManager.StartHighlight(UIElementEnum.Joystick);
                GameManager.Instance.UIManager.Display(UIElementEnum.Joystick);
                break;
            case DialogueEventType.DisplayInteract:
                InputManager.Instance.DisableGameplayMoveControls();
                InputManager.Instance.EnableGameplayInteractControls();
                DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.Interact, OnInteract);
                GameManager.Instance.UIManager.StartHighlight(UIElementEnum.Interact);
                GameManager.Instance.UIManager.Display(UIElementEnum.Interact);
                _character.InputManager.OnInteract += InvokeInteract;
                break;
            case DialogueEventType.DisplayPull:
                InputManager.Instance.DisableGameplayInteractControls();
                InputManager.Instance.EnableGameplayPullControls();
                DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.Pull, OnPull);
                GameManager.Instance.UIManager.StartHighlight(UIElementEnum.Pull);
                GameManager.Instance.UIManager.Display(UIElementEnum.Pull);
                _character.InputManager.OnPull += InvokePull;
                break;
            case DialogueEventType.DisplayPush:
                InputManager.Instance.EnableGameplayPushControls();
                DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.Push, OnPush);
                GameManager.Instance.UIManager.StartHighlight(UIElementEnum.Push);
                GameManager.Instance.UIManager.Display(UIElementEnum.Push);
                _character.InputManager.OnPush += InvokePush;
                break;
            case DialogueEventType.TutoEndInteract:
                InputManager.Instance.DisableGameplayPullControls();
                InputManager.Instance.EnableGameplayInteractControls();
                DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.EndInteract, OnEndInteract);
                GameManager.Instance.UIManager.StartHighlight(UIElementEnum.Interact);
                GameManager.Instance.UIManager.Display(UIElementEnum.Interact);
                _character.InputManager.OnInteract += InvokeEndInteract;
                break;
        }
    }

    private void OnTimeChangeEndedHandler(EnumTemporality temporality)
    {
        GameManager.Instance.UIManager.StopPulse(UIElementEnum.ChangeTime);
        GameManager.Instance.OnTimeChangeEnded -= OnTimeChangeEndedHandler;
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
        GameManager.Instance.UIManager.StartPulse(UIElementEnum.ChangeTime);
        GameManager.Instance.OnTimeChangeEnded += OnTimeChangeEndedHandler;
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
            //OnLevelEnter += BeginDialogue;
            _cinematics[(int)EnumAdvancementRoom1.Room0].Sequencers[0].InitializeSequence();

        }
    }

    public void EnterFromRoom4()
    {
        if (CurrentAdvancement == EnumAdvancementRoom1.Liana)
        {
            UpdateAdvancement(EnumAdvancementRoom1.Room4);
            OnLevelEnter += BeginDialogue;
        }
        _backTrakingDoor.EnableDoor();
        _blockDoor.SetActive(false);
        if (_aroundDoor.material.HasProperty("_IsActivated"))
            _aroundDoor.material.SetFloat("_IsActivated", 1);
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
