public interface IInteractableBase
{
    void Interact();

    public float OffsetRadius { get; set; }

    public int Priority { get; set; }

    public bool CanInteract { get; set; }
}
