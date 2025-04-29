using UnityEngine;

public class RiwaShowingPathTriggerZone : MonoBehaviour, IInteractable
{

    protected Floor1Room3LevelManager _instance;
    private bool _canInteract = true;
    private float _offsetRadius = -0.5f;

    public float OffsetRadius { get => _offsetRadius; set => _offsetRadius = value; }
    public int Priority { get; set; }
    public bool CanInteract { get => _canInteract; set => _canInteract = value; }

    private void Start()
    {
        _instance = (Floor1Room3LevelManager)Floor1Room3LevelManager.Instance;
        _instance.OnPlayerCompletedDamier += EndDamier;
    }

    public void DialogueToCall()
    {
        DialogueSystem.Instance.BeginDialogue(_instance.TutorialRoom3Manager.Room3Dialogue[2]);
    }

    public void Interact()
    {
        DialogueToCall();
    }

    private void EndDamier()
    {
        _canInteract = false;
    }
}