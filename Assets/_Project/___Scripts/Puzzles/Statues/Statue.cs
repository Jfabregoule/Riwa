using System.Collections;
using UnityEngine;

public class Statue : MonoBehaviour, IMovable, IRotatable
{
    [SerializeField] private float _rotateSpeed = 1;

    [Header("Debug")]
    [SerializeField] private bool _showDebugLog = false;


    private float _lerpTime = 1.5f;
    private float _unitGridSize;
    private float _angle = 45f;
    private float _blend;
    private CellPos _pos;
    private CellContent _content;
    private bool _validate;
    private bool _isMoving;

    private Animator _animator;

    public bool Validate { get => _validate; set => _validate = value; }
    public float UnitGridSize { get => _unitGridSize; set => _unitGridSize = value; }
    public float OffsetRadius { get; set; }
    public float MoveSpeed { get; set; }
    public float MoveDistance { get; set; }
    public bool CanInteract { get => !_validate; set => throw new System.NotImplementedException(); }
    public int Priority { get; set; }
    public float RotateSpeed { get => _rotateSpeed; set => _rotateSpeed = value; }
    public Animator StatueAnimator { get => _animator; set => _animator = value; }
    public CellPos Pos { get => _pos; set => _pos = value; }
    public CellContent Content { get => _content; set => _content = value; }
    public bool IsMoving { get => _isMoving; set => _isMoving = value; }

    public delegate bool StatueMoveEvent(CellPos oldPos, Vector2Int nextPos, CellContent statueData, Statue statue);
    public delegate void StatueRotateEvent(CellPos pos, CellContent content, Statue statue);
    public delegate void StatueEndMoving();

    public event StatueMoveEvent OnStatueMoved;
    public event StatueRotateEvent OnStatueRotate;
    public event StatueEndMoving OnStatueEndMoving;

    public event IRotatable.RotatableEvent OnRotateFinished;
    public event IMovable.NoArgVoid OnMoveFinished;
    public event IMovable.NoArgVector3 OnReplacePlayer;

    public void Start()
    {
        Priority = 0;
        OffsetRadius = 0.8f;
        MoveSpeed = 2f;
        MoveDistance = _unitGridSize;
        _blend = 0f;
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (_validate)
            _animator.SetBool("isValidate", true);
    }

    public bool Move(Vector3 direction)
    {
        if (_isMoving || _validate) return true;

        if (_showDebugLog == true) Debug.Log("UnitgridSize: " + _unitGridSize + " | Direction: " + direction);
        if (_showDebugLog == true) Debug.Log("PosX: " + _pos.x + " | PosY: " + _pos.y + " | Rotation: " + _content.rotation + " | ID: " + _content.id);
        
        bool canMove = OnStatueMoved.Invoke(_pos, Helpers.Vector2To2Int(new Vector2(direction.x, direction.z)), _content, this);
        
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
        _content.rotation = Mathf.RoundToInt(transform.localRotation.eulerAngles.y / 45f) * 45 % 360; ;
        transform.localRotation = desiredRotation;
        if (_showDebugLog == true) Debug.Log("Rotation: " + transform.localRotation.eulerAngles + " | Current content rot: " + _content.rotation);
        _isMoving = false;
        OnStatueRotate?.Invoke(_pos, _content, this);
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

    public void OnStatueValidate()
    {
        foreach(Transform child in transform)
        {
            if(child.TryGetComponent(out Renderer renderer))
            {
                if (renderer.material.HasProperty("_IsActivated"))
                    renderer.material.SetFloat("_IsActivated", 1f);
            }
        }
    }
}
