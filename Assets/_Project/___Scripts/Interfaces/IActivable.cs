public interface IActivable
{
    public delegate void ActivateEvent();

    void Activate();
    void Deactivate();

    event ActivateEvent OnActivated;
    event ActivateEvent OnDesactivated;
}
