public interface IActivable
{
    public delegate void ActivateEvent();

    void Activate();
    void Desactivate();

    event ActivateEvent OnActivated;
    event ActivateEvent OnDesactivated;
}
