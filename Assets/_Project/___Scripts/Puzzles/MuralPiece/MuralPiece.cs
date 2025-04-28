using System.Collections;
using UnityEngine;

public class MuralPiece : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _fresqueTransform;
    [SerializeField] private float _lerpTime = 3f;

    public float OffsetRadius { get => 0f; set => OffsetRadius = value; }
    public bool CanInteract { get; set; }
    public int Priority { get ; set; }

    public delegate void MuralPieceEvent();
    public MuralPieceEvent OnPickUp;
    private void Start()
    {
        CanInteract = true;
        Priority = 0;
    }
    public void Interact()
    {
        OnPickUp?.Invoke();
        StartCoroutine(PlacePieceOnFresque());
        CanInteract = false;
    }

    private IEnumerator PlacePieceOnFresque()
    {
        Vector3 initialPos = transform.position;
        Quaternion initialRot = transform.rotation;

        float elapsedTime = 0f;

        while(elapsedTime < _lerpTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / _lerpTime);
            transform.position = Vector3.Lerp(initialPos, _fresqueTransform.position, t);
            if(_fresqueTransform.rotation != Quaternion.identity) transform.rotation = Quaternion.Lerp(initialRot, _fresqueTransform.rotation, t);
            yield return null;
        }

        transform.position = _fresqueTransform.position;
        if (TryGetComponent<TemporalItem>(out TemporalItem temporalItem))
            temporalItem.UpdatePresentPosition();
        if (_fresqueTransform.rotation != Quaternion.identity) transform.rotation = _fresqueTransform.rotation;
    }
}
