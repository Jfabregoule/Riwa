using System;
using UnityEngine;

public class TreeStumpTest : MonoBehaviour, ITreeStump, IInteractable
{
    public float OffsetRadius { get ; set; }

    private DialogueSystem _dialogueSystem;

    public Action OnInteract;

    private void Start()
    {
        OffsetRadius = 0;
        StartCoroutine(Helpers.WaitMonoBeheviour(() => DialogueSystem.Instance, SubscribeToDialogueSystem));
    }

    public void Interact()
    {
        Debug.Log("Je passe en ame, TreeSumpTest.cs");
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
