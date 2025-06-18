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
    EndCinematic,
    End
}

[System.Serializable]
public struct CinematicRoom1
{
    public EnumAdvancementRoom1 ID;
    public DialogueAsset Dialogue;
    public Sequencer[] Sequencers;
    public PlacementZone[] Zones;
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
    [SerializeField] private CinemachineVirtualCamera _cinematicEndCamera;
    [SerializeField] private GameObject _evelator;

    [SerializeField] private Door _backTrakingDoor;
    [SerializeField] private GameObject _blockDoor;
    [SerializeField] private Renderer _aroundDoor;

    [SerializeField] private CinematicRoom1[] _cinematics;
    [SerializeField] private TutoGhost _ghost;


    [SerializeField] private EndGameCinematic _endGameCinematic;
    
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
    public EndGameCinematic EndGameCinematic { get => _endGameCinematic; }
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
    }

    private void LoadData()
    {
        UpdateAdvancement((EnumAdvancementRoom1)SaveSystem.Instance.LoadElement<int>("Room1Progress"));
    }

    public override void Start()
    {
        base.Start();

        _ghost.gameObject.SetActive(false);

        if (CurrentAdvancement == EnumAdvancementRoom1.Start)
        {
            OnLevelEnter += BeginDialogue;
            _currentZone = 0;
            GameManager.Instance.UIManager.Hide(UIElementEnum.Interact);
            GameManager.Instance.UIManager.Hide(UIElementEnum.Push);
        }

        if (CurrentAdvancement >= EnumAdvancementRoom1.Room0)
        {
            GameManager.Instance.UIManager.Display(UIElementEnum.Interact);
            GameManager.Instance.UIManager.Display(UIElementEnum.ChangeTime);
            //GameManager.Instance.UnlockChangeTime();
        }

        if (CurrentAdvancement == EnumAdvancementRoom1.Room4)
            OnLevelEnter += BeginDialogue;

        if (CurrentAdvancement == EnumAdvancementRoom1.EndCinematic)
        {
            CurrentAdvancement -= 1;
            OnLevelEnter += BeginDialogue;
        }

        if (CurrentAdvancement == EnumAdvancementRoom1.End)
        {
            CurrentAdvancement -= 2;
            OnLevelEnter += BeginDialogue;
        }
        
        DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.ChangeTime, OnChangeTime);

        foreach (var cam in _cameras)
        {
            CameraDictionnary[cam._id] = cam._camera;
        }

        _character.InputManager.OnChangeTime += CheckCrateEnigma;

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

        PlacementZone zone = GetCurrentZone();
        if (zone != null)
        {
            GetCurrentZone().OnPlace -= BoxInZone;
            GetCurrentZone().OnPlace -= PlayerInZone;
        }

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
            //DialogueSystem.Instance.BeginDialogue(_cinematics[(int)CurrentAdvancement].Dialogue);
            _cinematics[(int)EnumAdvancementRoom1.Liana].Sequencers[0].InitializeSequence();
        }

        _character.InputManager.OnChangeTime -= CheckCrateEnigma;
    }

    public void InvokeChangeTime() {
        if (!_character.CanChangeTime) return;
        InputManager.Instance.EnableGameplayControls();
        GameManager.Instance.UIManager.StopHighlight(UIElementEnum.ChangeTime);
        _character.InputManager.OnChangeTime -= InvokeChangeTime;
        DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.ChangeTime);
    }

    public void InvokeMove()
    {
        GameManager.Instance.UIManager.StopHighlight(UIElementEnum.Joystick);
        InputManager.Instance.UnlockJoystick();
        _character.InputManager.OnMove -= InvokeMove;
        GetCurrentZone().gameObject.SetActive(true);
        GetCurrentZone().OnPlace += PlayerInZone;
        DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.PlayerInZone, OnPlayerInzone);
        DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.Move);
    }

    private void InvokeInteract()
    {
        GameManager.Instance.UIManager.StopHighlight(UIElementEnum.Interact);
        _character.InputManager.OnInteract -= InvokeInteract;
        DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.Interact);
    }

    private void InvokeEndInteract()
    {
        GameManager.Instance.UIManager.StopHighlight(UIElementEnum.Interact);
        _character.InputManager.OnInteract -= InvokeEndInteract;
        DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.EndInteract);
    }

    private void InvokePull()
    {
        GameManager.Instance.UIManager.StopHighlight(UIElementEnum.Pull);
        _character.InputManager.OnPull -= InvokePull;
        DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.Pull);
    }

    private void InvokePush()
    {
        InputManager.Instance.EnableGameplayControls();
        GameManager.Instance.UIManager.StopHighlight(UIElementEnum.Push);
        _character.InputManager.OnPush -= InvokePush;
        DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.Push); 
    }

    private void PlayerInZone(GameObject go)
    {
        if (go.TryGetComponent(out ACharacter charcter))
        {

            GetCurrentZone().OnPlace -= PlayerInZone;
            GetCurrentZone().gameObject.SetActive(false);
            DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.PlayerInZone);
            _currentZone += 1;
            if (_currentZone < _cinematics[(int)CurrentAdvancement].Zones.Length)
            {
                GetCurrentZone().gameObject.SetActive(true);
                GetCurrentZone().OnPlace += BoxInZone;
                DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.BoxInZone, OnBoxInzone);
            }
        }
    }

    private void BoxInZone(GameObject go)
    {
        if (go.TryGetComponent(out Crate box))
        {
            GetCurrentZone().OnPlace -= BoxInZone;
            GetCurrentZone().gameObject.SetActive(false);
            DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.BoxInZone);
            _currentZone += 1;
            if(_currentZone < _cinematics[(int)CurrentAdvancement].Zones.Length)
            {
                GetCurrentZone().gameObject.SetActive(true);
                if(CurrentAdvancement == EnumAdvancementRoom1.Start)
                {
                    GetCurrentZone().OnPlace += PlayerInZone;
                    DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.PlayerInZone, OnPlayerInzone);
                }
                else
                {
                    GetCurrentZone().OnPlace += BoxInZone;
                    DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.BoxInZone, OnBoxInzone);
                }
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
            case DialogueEventType.DisplayChangeTime:
                //GameManager.Instance.UnlockChangeTime();
                StartCoroutine(DisplayChangeTime());

                break;
            case DialogueEventType.DisplayJoystick:
                StartCoroutine(DisplayJoystick());
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
                InputManager.Instance.DisableGameplayInteractControls();
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
            case DialogueEventType.WaitInteract:
                _character.InputManager.OnInteract += InvokeInteract;
                break;
            case DialogueEventType.ValidateTuto1Room1:
                GameManager.Instance.Character.StateMachine.GoToIdle();
                InputManager.Instance.DisableGameplayControls();
                break;
            case DialogueEventType.ShowGhost:
                _ghost.gameObject.SetActive(true);
                break;
            case DialogueEventType.HideGhost:
                _ghost.gameObject.SetActive(false);
                GameManager.Instance.Character.StateMachine.GoToIdle();
                EventDispatcher(DialogueEventType.DisplayChangeTime);
                break;
            case DialogueEventType.DisableInput:
                GameManager.Instance.Character.StateMachine.GoToIdle();
                InputManager.Instance.DisableGameplayControls();
                break;
            case DialogueEventType.OnFinish:
                UpdateAdvancement(EnumAdvancementRoom1.EndCinematic);
                _cinematics[(int)CurrentAdvancement].Sequencers[0].InitializeSequence();
                StartCoroutine(BlendingCamera(_cinematicEndCamera));
                break;
        }
    }

    private IEnumerator DisplayJoystick()
    {
        yield return Helpers.GetWait(1f);
        InputManager.Instance.EnableGameplayMoveControls();
        InputManager.Instance.LockJoystick();
        DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.Move, OnMove);
        GameManager.Instance.UIManager.StartHighlight(UIElementEnum.Joystick);
        GameManager.Instance.UIManager.Display(UIElementEnum.Joystick);
        _character.InputManager.OnMove += InvokeMove;

    }

    private IEnumerator DisplayChangeTime()
    {
        yield return Helpers.GetWait(1f); 
        InputManager.Instance.DisableGameplayControls();
        InputManager.Instance.EnableGameplayChangeTimeControls();
        DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.ChangeTime, OnChangeTime);
        GameManager.Instance.UIManager.StartHighlight(UIElementEnum.ChangeTime);
        GameManager.Instance.UIManager.Display(UIElementEnum.ChangeTime);
        _character.InputManager.OnChangeTime += InvokeChangeTime;

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

    //public IEnumerator WaitForPulse()
    //{
    //    yield return new WaitForSeconds(1.5f);
    //    GameManager.Instance.UIManager.StartPulse(UIElementEnum.ChangeTime);
    //    GameManager.Instance.OnTimeChangeEnded += OnTimeChangeEndedHandler;
    //    InputManager.Instance.EnableGameplayChangeTimeControls();
    //}

    private void BeginDialogue()
    {
        DialogueSystem.Instance.BeginDialogue(_cinematics[(int)CurrentAdvancement].Dialogue);
    }

    public void EnterFromRoom0()
    {
        if(CurrentAdvancement == EnumAdvancementRoom1.Start)
        {
            GameManager.Instance.UIManager.Display(UIElementEnum.Interact);
            GameManager.Instance.UIManager.Display(UIElementEnum.Push);
            UpdateAdvancement(EnumAdvancementRoom1.Room0);
            _currentZone = 0;
            GetCurrentZone().gameObject.SetActive(true);
            GetCurrentZone().OnPlace += BoxInZone;
            DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.BoxInZone, OnBoxInzone);
            //OnLevelEnter += BeginDialogue;
            //_cinematics[(int)EnumAdvancementRoom1.Room0].Sequencers[0].InitializeSequence();

        }
    }

    public void EnterFromRoom4()
    {
        UpdateAdvancement(EnumAdvancementRoom1.Room4);
        OnLevelEnter += BeginDialogue;
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

    private PlacementZone GetCurrentZone(){
        if (_currentZone < _cinematics[(int)CurrentAdvancement].Zones.Length)
        {
            return _cinematics[(int)CurrentAdvancement].Zones[_currentZone];
        }
        return null;
    }
    
}
