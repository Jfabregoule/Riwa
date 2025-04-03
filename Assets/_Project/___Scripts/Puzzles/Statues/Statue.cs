using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Statue : MonoBehaviour, IMovable, IRotatable
{
    [Header("Debug")]
    [SerializeField] private bool _startDebugLog = false;
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

    public bool Validate { get => _validate; set => _validate = value; }
    public bool IsLocked { get => _lockPosition; set => _lockPosition = value; }

    public delegate bool StatueMoveEvent(CellPos oldPos, Vector2Int nextPos, CellContent statueData);
    public delegate void StatueRotateEvent(CellPos pos, CellContent content);
    public delegate void StatueEndMoving();

    public event StatueMoveEvent OnStatueMoved;
    public event StatueRotateEvent OnStatueRotate;
    public event StatueEndMoving OnStatueEndMoving;

    public void Move(Vector2 direction)
    {
        if (_isMoving || _lockPosition || _validate) return;
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
        Vector3 destination = new Vector3(transform.localPosition.x + (_unitGridSize * direction.x), transform.localPosition.y, transform.localPosition.z + (_unitGridSize * direction.y));
        while(elapsedTime < _lerpTime)
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
        _isMoving = false;
        OnStatueRotate.Invoke(_pos, _content);
        OnStatueEndMoving.Invoke();
    }

    public void SetStatuesData(int id, int rotation, int unitGridSize, int posX, int posY)
    {;
        this._content.id = id;
        this._content.rotation = rotation;
        this._unitGridSize = unitGridSize;
        this._pos.x = posX;
        this._pos.y = posY;
    }

}
