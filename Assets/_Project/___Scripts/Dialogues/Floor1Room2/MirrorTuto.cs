using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class MirrorTuto : MonoBehaviour
{
    [SerializeField] private DialogueAsset _dialogue;
    private DialogueSystem _dialogueSystem;

    [SerializeField] private CinemachineVirtualCamera[] _cameras;
    [SerializeField] private float _waitOnCamera = 3f;

    [SerializeField] private PlacementZone _zone;

    [SerializeField] private MonoBehaviour[] _activables;
    private int CurrentActive;
    private bool _done;

    private System.Action OnRotate;
    private System.Action OnPlayerInZone;
    private System.Action CameraFinish;
    private System.Action OnInteract;

    private void OnEnable()
    {
        LoadData();
        SaveSystem.Instance.OnLoadProgress += LoadData;
    }

    private void LoadData()
    {
        _done = SaveSystem.Instance.LoadElement<bool>("Room2TutoMirror");
    }
    void Start()
    {
        foreach (var activable in _activables)
        {
            if (activable.TryGetComponent(out IActivable act))
            {
                act.OnActivated += AddActivate;
                act.OnDesactivated += RemoveActivate;
            }
        }

        GameManager.Instance.UIManager.Hide(UIElementEnum.Rotate);
    }

    private void OnDisable()
    {
        if (_dialogueSystem != null)
            _dialogueSystem.OnDialogueEvent -= DispatchDialogueEvent;

        if (InputManager.Instance)
        {
            InputManager.Instance.OnInteract -= InvokeInteract;
            InputManager.Instance.OnRotateLeft -= InvokeRotate;
            InputManager.Instance.OnRotateRight -= InvokeRotate;
        }
        if(_zone != null)
            _zone.OnPlace -= PlayerInZone;

        SaveSystem.Instance.OnLoadProgress -= LoadData;
        SaveSystem.Instance.SaveElement<bool>("Room2TutoMirror", _done);
    }

    private void InvokeRotate()
    {
        GameManager.Instance.UIManager.StopHighlight(UIElementEnum.Rotate);
        InputManager.Instance.OnRotateLeft -= InvokeRotate;
        InputManager.Instance.OnRotateRight -= InvokeRotate;
        _done = true;
        SaveSystem.Instance.SaveElement<bool>("Room2TutoMirror", _done);
        DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.Rotate);
    }

    private void InvokeInteract()
    {
        InputManager.Instance.OnInteract -= InvokeInteract;
        DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.Interact);
    }

    private void PlayerInZone(GameObject go)
    {
        if (go.TryGetComponent(out ACharacter charcter))
        {

            _zone.OnPlace -= PlayerInZone;
            _zone.gameObject.SetActive(false);
            DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.PlayerInZone);
        }
    }

    private void DispatchDialogueEvent(DialogueEventType dialogueEventType)
    {
        switch (dialogueEventType)
        {
            case DialogueEventType.DisplayRotate:
                InputManager.Instance.DisableGameplayControls();
                InputManager.Instance.EnableGameplayRotateControls();
                DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.Rotate, OnRotate);
                GameManager.Instance.UIManager.StartHighlight(UIElementEnum.Rotate);
                GameManager.Instance.UIManager.Display(UIElementEnum.Rotate);
                InputManager.Instance.OnRotateLeft += InvokeRotate;
                InputManager.Instance.OnRotateRight += InvokeRotate;
                break;

            case DialogueEventType.OnFinish:
                InputManager.Instance.EnableGameplayControls();
                _zone.gameObject.SetActive(true);
                _zone.OnPlace += PlayerInZone;
                DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.PlayerInZone, OnPlayerInZone);
                break;

            case DialogueEventType.WaitInteract:
                DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.Interact, OnInteract);
                InputManager.Instance.OnInteract += InvokeInteract;
                break;
        }
    }

    private void SubscribeToDialogueSystem(DialogueSystem script)
    {
        if (script != null)
        {
            _dialogueSystem = script;
            _dialogueSystem.OnDialogueEvent += DispatchDialogueEvent;
            DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.SequenceCinematicFloor1Room2, CameraFinish);
            _dialogueSystem.BeginDialogue(_dialogue);
            StartCoroutine(SwitchCamera());
        }
    }

    private void AddActivate()
    {
        CurrentActive++;
        if (CurrentActive == _activables.Length && !_done)
        {
            StartCoroutine(Helpers.WaitMonoBeheviour(() => DialogueSystem.Instance, SubscribeToDialogueSystem));
        }
    }

    private void RemoveActivate()
    {
        CurrentActive--;
    }

    private IEnumerator SwitchCamera()
    {
        _cameras[0].Priority = 20;
        yield return new WaitForSeconds(_waitOnCamera);
        for (int i = 0; i < _cameras.Length - 1; i++)
        {
            _cameras[i].Priority = 0;
            _cameras[i + 1].Priority = 20;
            yield return new WaitForSeconds(_waitOnCamera);
        }
        _cameras[_cameras.Length - 1].Priority = 0;
        DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.SequenceCinematicFloor1Room2);
    }
}
