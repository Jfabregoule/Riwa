using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DialogueFisrtTutoLiana : MonoBehaviour
{
    [SerializeField] private DialogueAsset _asset;

    [SerializeField] private PlacementZone _zone;
    [SerializeField] private MonoBehaviour _activablePulse;
    private DialogueSystem _dialogueSystem;
    private bool _isAlreadyTrigger = false;

    private System.Action OnPlayerInZone;
    private System.Action OnInteract;

    private void OnDisable()
    {
        if (_dialogueSystem != null)
            _dialogueSystem.OnDialogueEvent -= DispatchDialogueEvent;

        if (_zone != null)
            _zone.OnPlace -= PlayerInZone;

        if (InputManager.Instance)
        {
            InputManager.Instance.OnInteract -= InvokeInteract;
        }
    }
    private void Start()
    {
        StartCoroutine(Helpers.WaitMonoBeheviour(() => DialogueSystem.Instance, SubscribeToDialogueSystem));

        if (_activablePulse.TryGetComponent(out IActivable act))
        {
            act.OnActivated += ActivePulse;
            act.OnDesactivated += DesactivePulse;
        }
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

    private void ActivePulse()
    {
        GameManager.Instance.UIManager.StartPulse(UIElementEnum.ChangeTime);
    }

    private void DesactivePulse()
    {
        GameManager.Instance.UIManager.StopPulse(UIElementEnum.ChangeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out ACharacter character) || _isAlreadyTrigger) return;

        _isAlreadyTrigger = true;
        _dialogueSystem.BeginDialogue(_asset);

        if (_activablePulse.TryGetComponent(out IActivable act))
        {
            act.OnActivated -= ActivePulse;
            act.OnDesactivated -= DesactivePulse;
        }
    }

    private void InvokeInteract()
    {
        InputManager.Instance.EnableOptionsControls();
        GameManager.Instance.UIManager.StopHighlight(UIElementEnum.Interact);
        InputManager.Instance.OnInteract -= InvokeInteract;
        DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.Interact);
    }

    private void DispatchDialogueEvent(DialogueEventType dialogueEventType)
    {
        switch (dialogueEventType)
        {
            case DialogueEventType.StartTutoLiana:
                _zone.gameObject.SetActive(true);
                _zone.OnPlace += PlayerInZone;
                DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.PlayerInZone, OnPlayerInZone);
                break;
            case DialogueEventType.DisplayInteract:
                InputManager.Instance.DisableOptionsControls();
                InputManager.Instance.DisableGameplayControls();
                InputManager.Instance.EnableGameplayInteractControls();
                DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.Interact, OnInteract);
                GameManager.Instance.UIManager.StartHighlight(UIElementEnum.Interact);
                InputManager.Instance.OnInteract += InvokeInteract;
                break;
            case DialogueEventType.WaitInteract:
                DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.Interact, OnInteract);
                InputManager.Instance.OnInteract += InvokeInteract;
                break;
            case DialogueEventType.RiwaOutFloor1Room2:
                ((Floor1Room2LevelManager)Floor1Room2LevelManager.Instance).RiwaFloor1Room2.MoveOutSensa();
                break;
            case DialogueEventType.RiwaInFloor1Room2:
                ((Floor1Room2LevelManager)Floor1Room2LevelManager.Instance).RiwaFloor1Room2.MoveToSensa();
                break;
            case DialogueEventType.EnablePlantRoom2:
                InputManager.Instance.EnableGameplayControls();
                ((Floor1Room2LevelManager)Floor1Room2LevelManager.Instance).BridgeVineScript.CanInteract = true;
                break;
            case DialogueEventType.ShowInput:
                InputManager.Instance.DisableGameplayControls();
                break;
        }
    }

    private void SubscribeToDialogueSystem(DialogueSystem script)
    {
        if (script != null)
        {
            _dialogueSystem = script;
            _dialogueSystem.OnDialogueEvent += DispatchDialogueEvent;
        }
    }
}
