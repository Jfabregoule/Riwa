using System.Collections;
using UnityEngine;

public class MuralPiece : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _fresqueTransform;
    [SerializeField] private float _lerpTime = 3f;
    [SerializeField] private ParticleSystem _onPlaceVFX;
    [SerializeField] private EnumTemporality _muralPieceTemporality;
    [SerializeField] private bool _isTutorialPiece = false;

    private Floor1Room4LevelManager _instance;
    private bool _canInteract = true;
    private bool _isPiecePlaced = false;

    public float OffsetRadius { get => 0f; set => OffsetRadius = value; }
    public bool CanInteract { get => _canInteract; set => _canInteract = value; }
    public int Priority { get ; set; }
    public Transform FresqueTransform { get => _fresqueTransform; }
    public EnumTemporality PieceTemporality { get => _muralPieceTemporality; set => _muralPieceTemporality = value; }
    public bool IsTutorialPiece { get => _isTutorialPiece; }
    public bool IsPiecePlaced { get => _isPiecePlaced; set => _isPiecePlaced = value; }

    public delegate void MuralPieceEvent(MuralPiece piece);
    public MuralPieceEvent OnPickUp;

    private void Start()
    {
        Priority = 0;
        _instance = (Floor1Room4LevelManager)Floor1Room4LevelManager.Instance;
    }
    public void Interact()
    {
        _instance.ChangeFresqueCompletionData(this, PieceTemporality);
        OnPickUp?.Invoke(this);
        StartCoroutine(PlacePieceOnFresque());
        CanInteract = false;
    }

    public IEnumerator PlacePieceOnFresque()
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
        _isPiecePlaced = true;
        if (TryGetComponent<TemporalItem>(out TemporalItem temporalItem))
            temporalItem.UpdatePresentPosition();
        if (_fresqueTransform.rotation != Quaternion.identity) transform.rotation = _fresqueTransform.rotation;
    }

    public bool GetIsTutorialDone()
    {
        return _instance.IsTutorialDone;
    }
}
