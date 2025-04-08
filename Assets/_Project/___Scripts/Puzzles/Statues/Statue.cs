using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Statue : MonoBehaviour, IMovable, IRotatable
{
    [Header("Debug")]
    [SerializeField] private bool _showDebugLog = false;
    /// <summary>
    /// The serialized variable down has to be removed when the CC will be up
    /// because the CC will be able to get the Statue his currently holding so don't need to handle if the statue is either lock or not
    /// </summary>
    [SerializeField] private bool _lockPosition = true;

    private float _lerpTime = 1.5f;
    private int _unitGridSize;
    private CellPos _pos;
    private CellContent _content;
    private bool _validate;
    private bool _isMoving;

    private float _offsetRadius = 2;

    public bool Validate { get => _validate; set => _validate = value; }
    public bool IsLocked { get => _lockPosition; set => _lockPosition = value; }
    public int UnitGridSize { get => _unitGridSize; set => _unitGridSize = value; }
    public float OffsetRadius { get => _offsetRadius; set => _offsetRadius = value; }

    public delegate bool StatueMoveEvent(CellPos oldPos, Vector2Int nextPos, CellContent statueData);
    public delegate void StatueRotateEvent(CellPos pos, CellContent content);
    public delegate void StatueEndMoving();

    public event StatueMoveEvent OnStatueMoved;
    public event StatueRotateEvent OnStatueRotate;
    public event StatueEndMoving OnStatueEndMoving;


    public void Move(Vector2 direction)
    {
        if (_isMoving || _lockPosition || _validate) return;
        if (_showDebugLog == true) Debug.Log("UnitgridSize: " + _unitGridSize + " | Direction: " + direction);
        if (_showDebugLog == true) Debug.Log("PosX: " + _pos.x + " | PosY: " + _pos.y + " | Rotation: " + _content.rotation + " | ID: " + _content.id);
        bool canMove = OnStatueMoved.Invoke(_pos, Helpers.Vector2To2Int(direction.normalized), _content);
        if(!canMove) return;
        if (direction.x != 0) _pos.x += (int)direction.x;
        if (direction.y != 0) _pos.y += (int)direction.y;
        StartCoroutine(LerpToTile(direction));
    }

    public void Rotate(float angle)
    {
        if(_isMoving || _lockPosition || _validate) return;
        StartCoroutine(LerpRotation(angle));
    }

    public void Hold()
    {
        throw new System.NotImplementedException();
    }

    public void Interactable()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator LerpToTile(Vector2 direction)
    {
        _isMoving = true;
        float elapsedTime = 0.0f;
        Vector3 initialPosition = transform.position;
        Vector3 destination = new Vector3(transform.position.x + (_unitGridSize * direction.x), transform.position.y, transform.position.z + (_unitGridSize * direction.y));
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
        OnStatueEndMoving.Invoke();
    }

    IEnumerator LerpRotation(float angle)
    {
        _isMoving = true;
        float elapsedTime = 0.0f;
        Quaternion initialRotation = transform.localRotation;
        Quaternion desiredRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, angle, transform.localRotation.eulerAngles.z) * initialRotation;
        while(elapsedTime < _lerpTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _lerpTime;
            t = Mathf.Clamp01(t);
            transform.localRotation = Quaternion.Lerp(initialRotation, desiredRotation, t);
            yield return null;
        }
        _content.rotation = (int)transform.localRotation.eulerAngles.y;
        transform.localRotation = desiredRotation;
        if (_showDebugLog == true) Debug.Log("Rotation: " + transform.localRotation.eulerAngles + " | Current content rot: " + _content.rotation);
        _isMoving = false;
        OnStatueRotate.Invoke(_pos, _content);
        OnStatueEndMoving.Invoke();
    }

    public void SetStatuesData(StatueData data)
    {;
        _content.id = data.id;
        _content.rotation = data.rotation;
        _unitGridSize = data.unitGridSize;
        _pos.x = data.posX;
        _pos.y = data.posY;
    }

}
