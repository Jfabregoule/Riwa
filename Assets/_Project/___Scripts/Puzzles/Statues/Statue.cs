using System.Collections;
using UnityEngine;

public class Statue : MonoBehaviour, IMovable, IRotatable
{
    [Header("Debug")]
    [SerializeField] private bool _showDebugLog = false;

    private float _lerpTime = 1.5f;
    private float _unitGridSize;
    private float _angle = 45f;
    private CellPos _pos;
    private CellContent _content;
    private bool _validate;
    private bool _isMoving;

    public bool Validate { get => _validate; set => _validate = value; }
    public float UnitGridSize { get => _unitGridSize; set => _unitGridSize = value; }
    public float OffsetRadius { get; set; }
    public float MoveSpeed { get; set; }
    public float MoveDistance { get; set; }
    
    public delegate bool StatueMoveEvent(CellPos oldPos, Vector2Int nextPos, CellContent statueData);
    public delegate void StatueRotateEvent(CellPos pos, CellContent content);
    public delegate void StatueEndMoving();

    public event StatueMoveEvent OnStatueMoved;
    public event StatueRotateEvent OnStatueRotate;
    public event StatueEndMoving OnStatueEndMoving;

    public event IRotatable.RotatableEvent OnRotateFinished;
    public event IMovable.NoArgVoid OnMoveFinished;
    public event IMovable.NoArgVector3 OnReplacePlayer;

    public void Start()
    {
        OffsetRadius = 0.5f;
        MoveSpeed = 2f;
        MoveDistance = _unitGridSize;
    }

    public bool Move(Vector3 direction)
    {
        if (_isMoving || _validate) return true;

        if (_showDebugLog == true) Debug.Log("UnitgridSize: " + _unitGridSize + " | Direction: " + direction);
        if (_showDebugLog == true) Debug.Log("PosX: " + _pos.x + " | PosY: " + _pos.y + " | Rotation: " + _content.rotation + " | ID: " + _content.id);
        
        bool canMove = OnStatueMoved.Invoke(_pos, Helpers.Vector2To2Int(new Vector2(direction.x, direction.z)), _content);
        
        if (!canMove) return false;
        if (direction.x != 0) _pos.x += (int)direction.x;
        if (direction.z != 0) _pos.y += (int)direction.z;
        StartCoroutine(MoveLerp(direction));
        return true;
    }
    public void Rotate(int sens)
    {
        if (_isMoving || _validate) return;
        StartCoroutine(LerpRotation(sens * _angle));
    }

    public void Hold()
    {
        throw new System.NotImplementedException();
    }

    public void Interact()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator MoveLerp(Vector3 direction)
    {
        _isMoving = true;
        float elapsedTime = 0.0f;
        Vector3 initialPosition = transform.position;
        Vector3 destination = new Vector3(transform.position.x + (MoveDistance * direction.x), transform.position.y, transform.position.z + (MoveDistance * direction.z));
        if (_showDebugLog == true) Debug.Log("UnitgridSize: " + MoveDistance + " | Destination: " + destination);
        if (_showDebugLog == true) Debug.Log("PosX: " + _pos.x + " | PosY: " + _pos.y + " | Rotation: " + _content.rotation + " | ID: " + _content.id);

        float distance = Vector3.Distance(initialPosition, destination);
        _lerpTime = distance / MoveSpeed;

        while (elapsedTime < _lerpTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _lerpTime;
            t = Mathf.Clamp01(t);
            transform.position = Vector3.Lerp(initialPosition, destination, t);
            yield return null;
        }
        transform.position = destination;
        _isMoving = false;
        OnStatueEndMoving?.Invoke();
        OnMoveFinished?.Invoke();
    }

    IEnumerator LerpRotation(float angle)
    {
        _isMoving = true;
        float elapsedTime = 0.0f;
        Quaternion initialRotation = transform.localRotation;
        Quaternion desiredRotation = Quaternion.Euler(0, angle, 0) * initialRotation;
        while(elapsedTime < _lerpTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _lerpTime;
            t = Mathf.Clamp01(t);
            transform.localRotation = Quaternion.Slerp(initialRotation, desiredRotation, t);
            yield return null;
        }
        _content.rotation = (int)transform.localRotation.eulerAngles.y;
        transform.localRotation = desiredRotation;
        if (_showDebugLog == true) Debug.Log("Rotation: " + transform.localRotation.eulerAngles + " | Current content rot: " + _content.rotation);
        _isMoving = false;
        OnStatueRotate?.Invoke(_pos, _content);
        OnStatueEndMoving?.Invoke();
        OnRotateFinished?.Invoke();
    }

    public void SetStatuesData(StatueData data)
    {
        _content.id = data.id;
        _content.rotation = data.rotation;
        _unitGridSize = data.unitGridSize;
        _pos.x = data.posX;
        _pos.y = data.posY;
    }
}
