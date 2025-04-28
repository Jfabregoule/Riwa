using System.Collections;
using UnityEngine;

public class MuralPiece : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _fresqueTransform;
    [SerializeField] private float _lerpTime = 3f;
    [SerializeField] private ParticleSystem _onPlaceVFX;
    [SerializeField] private EnumTemporality _muralPieceTemporality;

    private Floor1Room4LevelManager _instance;

    public float OffsetRadius { get => 0.3f; set => OffsetRadius = value; }
    public bool CanInteract { get; set; }
    public int Priority { get ; set; }
    public Transform FresqueTransform { get => _fresqueTransform; }
    public EnumTemporality PieceTemporality { get; set; }

    public delegate void MuralPieceEvent();
    public MuralPieceEvent OnPickUp;
    private void Start()
    {
        CanInteract = true;
        Priority = 0;
        _instance = (Floor1Room4LevelManager)Floor1Room4LevelManager.Instance;
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

        _onPlaceVFX.Play();
        transform.position = _fresqueTransform.position;
        if (TryGetComponent<TemporalItem>(out TemporalItem temporalItem))
            temporalItem.UpdatePresentPosition();
        if (_fresqueTransform.rotation != Quaternion.identity) transform.rotation = _fresqueTransform.rotation;

        EnumTemporality temporality = GameManager.Instance.CurrentTemporality;
        _instance.ChangeFresqueCompletionData(this, temporality);
    }
}
