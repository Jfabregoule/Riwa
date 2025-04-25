using System;
using UnityEngine;

public class TreeStumpTest : MonoBehaviour, ITreeStump, IInteractable
{
    public float OffsetRadius { get ; set; }
    public bool CanInteract { get; set; }
    public int Priority { get; set; }

    private DialogueSystem _dialogueSystem;

    public Action OnInteract;

    private void Start()
    {
        Priority = 0;
        OffsetRadius = 0;
        CanInteract = true;
        StartCoroutine(Helpers.WaitMonoBeheviour(() => DialogueSystem.Instance, SubscribeToDialogueSystem));
    }

    public void Interact()
    {
        _dialogueSystem.EventRegistery.Invoke(WaitDialogueEventType.SocleFloor1Room2);
    }

    private void SubscribeToDialogueSystem(DialogueSystem script)
    {
        if (script != null)
        {
            _dialogueSystem = script;
            _dialogueSystem.EventRegistery.Register(WaitDialogueEventType.SocleFloor1Room2, OnInteract);
        }
    }
}
