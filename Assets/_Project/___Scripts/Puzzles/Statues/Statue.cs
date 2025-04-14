using System.Collections;
using UnityEngine;

public class Statue : MonoBehaviour, IMovable, IRotatable
{
    [Header("Debug")]
    [SerializeField] private bool _showDebugLog = false;

    private float _lerpTime = 1.5f;
    private float _unitGridSize;
    private float _speed = 2f;
    private float _angle = 45f;
    private CellPos _pos;
    private CellContent _content;
    private bool _validate;
    private bool _isMoving;

    private float _offsetRadius = 0.3f;

    public bool Validate { get => _validate; set => _validate = value; }
    public float UnitGridSize { get => _unitGridSize; set => _unitGridSize = value; }
    public float OffsetRadius { get => _offsetRadius; set => _offsetRadius = value; }
    public float MoveSpeed { get => _speed; set => _speed = value; }

    public delegate bool StatueMoveEvent(CellPos oldPos, Vector2Int nextPos, CellContent statueData);
    public delegate void StatueRotateEvent(CellPos pos, CellContent content);
    public delegate void StatueEndMoving();

    public event StatueMoveEvent OnStatueMoved;
    public event StatueRotateEvent OnStatueRotate;
    public event StatueEndMoving OnStatueEndMoving;
    public event IRotatable.RotatableEvent OnRotateFinished;

    public void Move(Vector3 direction)
    {

        //Vector3 dominantDir = Helpers.GetDominantDirection(direction);
        if (_isMoving || _validate) return;
        if (_showDebugLog == true) Debug.Log("UnitgridSize: " + _unitGridSize + " | Direction: " + direction);
        if (_showDebugLog == true) Debug.Log("PosX: " + _pos.x + " | PosY: " + _pos.y + " | Rotation: " + _content.rotation + " | ID: " + _content.id);
        bool canMove = OnStatueMoved.Invoke(_pos, Helpers.Vector2To2Int(new Vector2(direction.x, direction.z)), _content);
        if (!canMove) return;
        if (direction.x != 0) _pos.x += (int)direction.x;
        if (direction.z != 0) _pos.y += (int)direction.z;
        StartCoroutine(LerpToTile(direction));
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

    public void Interactable()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator LerpToTile(Vector3 direction)
    {
        _isMoving = true;
        float elapsedTime = 0.0f;
        Vector3 initialPosition = transform.position;
        Vector3 destination = new Vector3(transform.position.x + (_unitGridSize * direction.x), transform.position.y, transform.position.z + (_unitGridSize * direction.z));
        if (_showDebugLog == true) Debug.Log("UnitgridSize: " + _unitGridSize + " | Destination: " + destination);
        if (_showDebugLog == true) Debug.Log("PosX: " + _pos.x + " | PosY: " + _pos.y + " | Rotation: " + _content.rotation + " | ID: " + _content.id);
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
