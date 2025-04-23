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
    //private CrateFeet _feet;

    public event IRotatable.RotatableEvent OnRotateFinished;
    public event IMovable.NoArgVoid OnMoveFinished;
    public event IMovable.NoArgVector3 OnReplacePlayer;

    public float MoveSpeed { get => _speed; set => _speed = value; }
    public float OffsetRadius { get; set; }
    public float MoveDistance { get => _distance; set => _distance = value; }

    public void Start()
    {
        _character = GameManager.Instance.Character;
        //_feet = GetComponentInChildren<CrateFeet>();

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
        size = Vector3.Scale(transform.localScale, size);

        LayerMask layerMask = GameManager.Instance.CurrentTemporality == EnumTemporality.Past ? _character.PastLayer : _character.PresentLayer;

        Collider[] colliders = Physics.OverlapBox(transform.position + multiplicator, size, Quaternion.Euler(new Vector3(0,90 * direction.z, 0)), layerMask);

        Vector3 center = transform.position + multiplicator;
        Vector3 halfExtents = size;
        Quaternion orientation = Quaternion.Euler(new Vector3(0, 90 * direction.z, 0));

        DebugDrawBox(center, halfExtents, orientation, UnityEngine.Color.red, 1f);

        foreach (var col in colliders)
        {
            if (col.gameObject != gameObject
                && !col.gameObject.TryGetComponent<ACharacter>(out ACharacter chara)
                && col.isTrigger == false)
            {
                Debug.Log("Collide with: " + col.gameObject.name);
                return false;
            }
        }

        _isMoving = true;
        StartCoroutine(MoveLerp(direction));
        return true;
    }

    void DebugDrawBox(Vector3 center, Vector3 halfExtents, Quaternion orientation, UnityEngine.Color color, float duration)
    {
        Vector3[] points = new Vector3[8];

        Vector3 right = orientation * Vector3.right;
        Vector3 up = orientation * Vector3.up;
        Vector3 forward = orientation * Vector3.forward;

        points[0] = center + right * halfExtents.x + up * halfExtents.y + forward * halfExtents.z;
        points[1] = center + right * halfExtents.x + up * halfExtents.y - forward * halfExtents.z;
        points[2] = center + right * halfExtents.x - up * halfExtents.y + forward * halfExtents.z;
        points[3] = center + right * halfExtents.x - up * halfExtents.y - forward * halfExtents.z;
        points[4] = center - right * halfExtents.x + up * halfExtents.y + forward * halfExtents.z;
        points[5] = center - right * halfExtents.x + up * halfExtents.y - forward * halfExtents.z;
        points[6] = center - right * halfExtents.x - up * halfExtents.y + forward * halfExtents.z;
        points[7] = center - right * halfExtents.x - up * halfExtents.y - forward * halfExtents.z;

        Debug.DrawLine(points[0], points[1], color, duration);
        Debug.DrawLine(points[0], points[2], color, duration);
        Debug.DrawLine(points[0], points[4], color, duration);
        Debug.DrawLine(points[1], points[3], color, duration);
        Debug.DrawLine(points[1], points[5], color, duration);
        Debug.DrawLine(points[2], points[3], color, duration);
        Debug.DrawLine(points[2], points[6], color, duration);
        Debug.DrawLine(points[3], points[7], color, duration);
        Debug.DrawLine(points[4], points[5], color, duration);
        Debug.DrawLine(points[4], points[6], color, duration);
        Debug.DrawLine(points[5], points[7], color, duration);
        Debug.DrawLine(points[6], points[7], color, duration);
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
            //if (!_feet.IsGround) { 
            //    _isMoving = false;
            //    OnMoveFinished?.Invoke();
            //    yield break;
            //}
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
