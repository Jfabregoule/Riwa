using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Statue : MonoBehaviour, IMovable, IRotatable
{
    [Header("Datas")]
    [SerializeField] private int _id;
    [SerializeField] private StatuePuzzle _gridManager;
    [SerializeField] private float _lerpTime = 3f;
    [Header("Debug")]
    [SerializeField] private int _currentRotation;
    [SerializeField] private bool _startDebugLog = false;
    [SerializeField] private bool _lockPosition = true;

    private CellPos _pos;
    private bool _validate;
    private bool _isMoving;

    public bool Validate { get => _validate; set => _validate = value; }
    public bool IsLocked { get => _lockPosition; set => _lockPosition = value; }

    void Start()
    {
        _pos.x = Mathf.Abs(Mathf.RoundToInt((transform.position.x - _gridManager.Origin.x) / _gridManager.UnitGridSize));
        _pos.y = Mathf.Abs(Mathf.RoundToInt((transform.position.z - _gridManager.Origin.z) / _gridManager.UnitGridSize));

        if (_pos.x < 0 || _pos.x >= _gridManager.GridSize.x || _pos.y < 0 || _pos.y >= _gridManager.GridSize.y)
        {
            if(_startDebugLog == true) Debug.LogWarning("Position de statue hors limites de la grille !");
            _pos.x = Mathf.Clamp(_pos.x, 0, _gridManager.GridSize.x - 1);
            _pos.y = Mathf.Clamp(_pos.y, 0, _gridManager.GridSize.y - 1);
        }

        if(_startDebugLog == true) Debug.Log("Statue: " + gameObject.name + " | POS X: " + _pos.x + " | POS Y: " + _pos.y);
        AlignToGrid();
    }

    private void AlignToGrid()
    {
        transform.position = _gridManager.Origin + new Vector3(_pos.x * _gridManager.UnitGridSize, transform.localPosition.y, _pos.y * _gridManager.UnitGridSize);
        if(_startDebugLog == true) Debug.Log("Statue: " + gameObject.name + " | Origin: " + _gridManager.Origin);
        _gridManager.PlaceStatueData(_pos, new CellContent(_id, _currentRotation));
        
    }

    public void Move(Vector2 direction)
    {
        if (_isMoving || _lockPosition || _validate) return;
        bool canMove = _gridManager.Move(_pos, Helpers.Vector2To2Int(direction.normalized), new CellContent(_id, _currentRotation));
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
        Vector3 destination = new Vector3(transform.localPosition.x + (_gridManager.UnitGridSize * direction.x), transform.localPosition.y, transform.localPosition.z + (_gridManager.UnitGridSize * direction.y));
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
        _gridManager.Check();
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
        _currentRotation = (int)transform.localRotation.eulerAngles.y;
        transform.localRotation = desiredRotation;
        _isMoving = false;
        _gridManager.UpdateStatueRotation(_pos, new CellContent(_id, _currentRotation));
        _gridManager.Check();
    }

}
