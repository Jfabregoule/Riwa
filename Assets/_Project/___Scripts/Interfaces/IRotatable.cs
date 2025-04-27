public interface IRotatable : IHoldable
{
    void Rotate(int sens);

    float RotateSpeed { get; set; }

    public delegate void RotatableEvent();
    event RotatableEvent OnRotateFinished;
}
