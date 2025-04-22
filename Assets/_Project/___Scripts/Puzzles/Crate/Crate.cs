using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class Crate : MonoBehaviour, IMovable, IRotatable
{
    ACharacter _character;

    [SerializeField] private float _speed = 0.1f;
    [SerializeField] private float _angle;
    [SerializeField] private float _distance = 0.1f;

    Vector3 _floorOffset;
    Vector3 _boxSize;

    bool _isMoving;

    public event IRotatable.RotatableEvent OnRotateFinished;
    public event IMovable.NoArgVoid OnMoveFinished;
    public event IMovable.NoArgVector3 OnReplacePlayer;

    public float MoveSpeed { get => _speed; set => _speed = value; }
    public float OffsetRadius { get; set; }
    public float MoveDistance { get => _distance; set => _distance = value; }

    public void Start()
    {
        _character = GameManager.Instance.Character;

        float security = 0.01f;
        float securityRadius = 0.1f;

        _boxSize = GetComponent<BoxCollider>().size;
        _floorOffset = -Vector3.up * (_boxSize.y * 0.5f) - (-Vector3.up * security);

        OffsetRadius = _boxSize.x / 2 + _character.GetComponent<CapsuleCollider>().radius * _character.transform.localScale.x * 1.8f + securityRadius; //J'agrandit loffset pour que l'anime de coup de boule rentre pas dans la crate
    }

    public void Interact()
    {
    }

    public void Hold()
    {
        
    }

    public bool Move(Vector3 direction)
    {
        if (_isMoving) return true;

        Vector3 multiplicator = Vector3.Scale(_boxSize / 2, direction);


        Vector3 size = _boxSize * 0.5f;
        size.x = MoveDistance;

        LayerMask layerMask = GameManager.Instance.CurrentTemporality == EnumTemporality.Past ? _character.PastLayer : _character.PresentLayer;

        Collider[] colliders = Physics.OverlapBox(transform.position + multiplicator, size, Quaternion.Euler(new Vector3(0,90 * direction.z, 0)), layerMask);

        Debug.Log(multiplicator);

        foreach (var col in colliders)
        {
            if (col.gameObject != gameObject
                && !col.gameObject.TryGetComponent<ACharacter>(out ACharacter chara)
                && col.isTrigger == false)
            {
                return false;
            }
        }

        _isMoving = true;
        StartCoroutine(MoveLerp(direction));
        return true;
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

    public IEnumerator MoveLerp(Vector3 direction)
    {

        Vector3 initialPosition = transform.position;
        Vector3 destination = new Vector3(transform.position.x + (MoveDistance * direction.x), transform.position.y, transform.position.z + (MoveDistance * direction.z));

        float distance = Vector3.Distance(initialPosition, destination);

        while (Vector3.Distance(transform.position, destination) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, MoveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = destination;

        //Vector3 playerTargetPosition = destination + (direction.normalized * OffsetRadius * transform.localScale.x);

        OnReplacePlayer?.Invoke(direction.normalized * OffsetRadius * transform.localScale.x);
        OnMoveFinished?.Invoke();

        _isMoving = false;

        yield return null;
    }
}
