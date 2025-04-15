using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class InteractTest : MonoBehaviour, IMovable, IRotatable
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

        OffsetRadius = _boxSize.x / 2 + GameManager.Instance.Character.GetComponent<CapsuleCollider>().radius + securityRadius;
    }

    public void Interactable()
    {
    }

    public void Hold()
    {
        
    }

    public bool Move(Vector3 direction)
    {
        //CHECK DE SI ON PEUT BOUGER

        if (_isMoving) return true;

        Vector3 multiplicator = Vector3.Scale(_boxSize / 2, direction);

        Vector3 size = _boxSize;
        size.z = MoveDistance;

        LayerMask layerMask = _character.IsInPast ? _character.PastLayer : _character.PresentLayer;

        Vector3 halfSize = size * 0.5f;

        Vector3 center = transform.position + multiplicator;
        UnityEngine.Color color = UnityEngine.Color.red;
        float duration = 20f;

        // 8 corners
        Vector3 p0 = center + new Vector3(-halfSize.x, -halfSize.y, -halfSize.z);
        Vector3 p1 = center + new Vector3(halfSize.x, -halfSize.y, -halfSize.z);
        Vector3 p2 = center + new Vector3(halfSize.x, -halfSize.y, halfSize.z);
        Vector3 p3 = center + new Vector3(-halfSize.x, -halfSize.y, halfSize.z);

        Vector3 p4 = center + new Vector3(-halfSize.x, halfSize.y, -halfSize.z);
        Vector3 p5 = center + new Vector3(halfSize.x, halfSize.y, -halfSize.z);
        Vector3 p6 = center + new Vector3(halfSize.x, halfSize.y, halfSize.z);
        Vector3 p7 = center + new Vector3(-halfSize.x, halfSize.y, halfSize.z);

        // Bottom square
        Debug.DrawLine(p0, p1, color, duration);
        Debug.DrawLine(p1, p2, color, duration);
        Debug.DrawLine(p2, p3, color, duration);
        Debug.DrawLine(p3, p0, color, duration);

        // Top square
        Debug.DrawLine(p4, p5, color, duration);
        Debug.DrawLine(p5, p6, color, duration);
        Debug.DrawLine(p6, p7, color, duration);
        Debug.DrawLine(p7, p4, color, duration);

        // Vertical edges
        Debug.DrawLine(p0, p4, color, duration);
        Debug.DrawLine(p1, p5, color, duration);
        Debug.DrawLine(p2, p6, color, duration);
        Debug.DrawLine(p3, p7, color, duration);

        Quaternion q = Quaternion.Euler(Vector3.Cross(direction, Vector3.up) * 90);

        Collider[] colliders = Physics.OverlapBox(transform.position + multiplicator, size, Quaternion.Euler(new Vector3(0,0,0)), layerMask);

        foreach(var col in colliders)
        {
            if (col.gameObject != gameObject
             && !col.gameObject.TryGetComponent<ACharacter>(out ACharacter chara)) //ICI METTRE LES OBJECTS QU'ON VEUT EVITER
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

        Vector3 playerTargetPosition = destination + (direction.normalized * OffsetRadius);

        OnReplacePlayer?.Invoke(direction.normalized * OffsetRadius);
        OnMoveFinished?.Invoke();

        _isMoving = false;

        yield return null;
    }
}
