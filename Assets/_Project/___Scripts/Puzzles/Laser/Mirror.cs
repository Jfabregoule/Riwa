using System.Collections;
using UnityEngine;

public class Mirror : MonoBehaviour, IRotatable
{
    [SerializeField] private float _angle;
    [SerializeField] private MonoBehaviour _activable;
    [SerializeField] private float _rotateSpeed = 1;

    public float OffsetRadius { get; set; }
    public bool CanInteract { get; set; }
    public int Priority { get; set; }

    public float RotateSpeed { get => _rotateSpeed; set => _rotateSpeed = value; }

    private void Start()
    {
        Priority = 0;
        OffsetRadius = 1;
        CanInteract = true;

        _activable.GetComponent<IActivable>().OnActivated += Desactivate;
    }

    private void Desactivate()
    {
        _activable.GetComponent<IActivable>().OnActivated -= Desactivate;
        CanInteract = false;
    }

    public event IRotatable.RotatableEvent OnRotateFinished;

    public void Hold()
    {
        throw new System.NotImplementedException();
    }

    public void Interact()
    {
        throw new System.NotImplementedException();
    }

    public void Rotate(int sens)
    {
        StartCoroutine(CoroutineRotate(sens));
    }

    private IEnumerator CoroutineRotate(int sens)
    {
        float clock = 0;

        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0f, _angle * sens, 0f);

        while (clock < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, Mathf.Clamp01(clock * 3));

            clock += Time.deltaTime;

            yield return null;
        }

        OnRotateFinished?.Invoke();
    }
}
