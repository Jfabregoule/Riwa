using System.Collections;
using UnityEngine;

public class Mirror : MonoBehaviour, IRotatable
{
    [SerializeField] private float _angle;
    public float OffsetRadius { get; set; }

    private void Start()
    {
        OffsetRadius = 1;
    }

    public event IRotatable.RotatableEvent OnRotateFinished;

    public void Hold()
    {
        throw new System.NotImplementedException();
    }

    public void Interactable()
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
