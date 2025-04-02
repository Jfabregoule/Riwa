using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : MonoBehaviour, IMovable, IRotatable
{
    [SerializeField] private int _currentRotation;
    [SerializeField] private int _id;
    [SerializeField] private StatuePuzzle _gridManager;
    [SerializeField] private float _lerpTime = 3f;

    private CellPos _pos;
    private bool _validate;
    private bool _isMoving;

    public delegate bool StatueMoveEvent(CellPos oldPos, Vector2Int nextPos, CellContent statueData);
    public event StatueMoveEvent OnStatueMoved;

    public int ID { get => _id; }
    public bool Validate { get => _validate; set => _validate = value; }
    public int CurrentRotation { get => _currentRotation; }

    void Start()
    {
        _pos.x = 1;//(int)transform.position.x;
        _pos.y = 1;//(int)transform.position.z;
        _validate = false;
        AlignToGrid();
    }

    void Update()
    {
        Vector2 direction = Vector2.zero;

        if (_isMoving) return;
        if (Input.GetKeyDown(KeyCode.W)) Move(Vector2.up);
        if (Input.GetKeyDown(KeyCode.S)) Move(Vector2.down);
        if (Input.GetKeyDown(KeyCode.A)) Move(Vector2.left);
        if (Input.GetKeyDown(KeyCode.D)) Move(Vector2.right);
        if (Input.GetKeyDown(KeyCode.E)) Rotate(45f);
    }

    private void AlignToGrid()
    {
        transform.position = _gridManager.Origin + new Vector3(_pos.x * _gridManager.UnitGridSize, transform.localPosition.y, _pos.y * _gridManager.UnitGridSize);
    }

    public void Move(Vector2 direction)
    {
        bool canMove = _gridManager.Move(_pos, Helpers.Vector2To2Int(direction.normalized), new CellContent(_id, _currentRotation));
        if(!canMove) return;
        if (direction.x != 0) _pos.x += (int)direction.x;
        if (direction.y != 0) _pos.y += (int)direction.y;
        StartCoroutine(LerpToTile(direction));
    }

    public void Rotate(float angle)
    {
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

    private void StatueMoved(CellPos oldPos, Vector2Int nextPos, CellContent statueData)
    {
        OnStatueMoved?.Invoke(oldPos, nextPos, statueData);
    }

}
