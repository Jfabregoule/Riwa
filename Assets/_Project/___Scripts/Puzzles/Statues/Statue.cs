using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : MonoBehaviour, IMovable, IRotatable
{
    [SerializeField] private int _currentRotation;
    [SerializeField] private int _id;
    [SerializeField] private StatuePuzzle _gridManager;

    private CellPos _pos;
    private bool _validate;

    public delegate bool StatueMoveEvent(CellPos oldPos, Vector2Int nextPos, CellContent statueData);
    public event StatueMoveEvent OnStatueMoved;

    public int ID { get => _id; }
    public bool Validate { get => _validate; set => _validate = value; }
    public int CurrentRotation { get => _currentRotation; }

    void Start()
    {
        _pos.x = 3;//(int)transform.position.x;
        _pos.y = 1;//(int)transform.position.z;
        _validate = false;
        AlignToGrid();
    }

    void Update()
    {
        Vector2 direction = Vector2.zero;

        if (_validate == true) return;
        if (Input.GetKeyDown(KeyCode.W)) Move(Vector2.up);
        if (Input.GetKeyDown(KeyCode.S)) Move(Vector2.down);
        if (Input.GetKeyDown(KeyCode.A)) Move(Vector2.left);
        if (Input.GetKeyDown(KeyCode.D)) Move(Vector2.right);
        if (Input.GetKeyDown(KeyCode.E)) Rotate(45f);
    }

    private void AlignToGrid()
    {
        if (_gridManager == null) return;

        Vector3 relativePos = transform.position - _gridManager.Origin;
        int gridX = Mathf.RoundToInt(relativePos.x / _gridManager.UnitGridSize);
        int gridY = Mathf.RoundToInt(relativePos.z / _gridManager.UnitGridSize);

        transform.position = _gridManager.Origin + new Vector3(gridX * _gridManager.UnitGridSize, transform.localPosition.y, gridY * _gridManager.UnitGridSize);
    }

    public void Move(Vector2 direction)
    {
        bool canMove = _gridManager.Move(_pos, Helpers.Vector2To2Int(direction.normalized), new CellContent(_id, _currentRotation));
        if(!canMove) return;
        if (direction.x != 0) _pos.x += (int)direction.x;
        if (direction.y != 0) _pos.y += (int)direction.y;
        transform.position = new Vector3(transform.localPosition.x + (_gridManager.UnitGridSize * direction.x), transform.localPosition.y, transform.localPosition.z + (_gridManager.UnitGridSize * direction.y));
    }

    public void Rotate(float angle)
    {
        transform.localRotation *= Quaternion.Euler(transform.localRotation.eulerAngles.x, angle, transform.localRotation.eulerAngles.z);
        // Update dans la grid la rotation dans la statue
    }

    public void Hold()
    {
        throw new System.NotImplementedException();
    }

    public void Interactable()
    {
        throw new System.NotImplementedException();
    }

    private void StatueMoved(CellPos oldPos, Vector2Int nextPos, CellContent statueData)
    {
        OnStatueMoved?.Invoke(oldPos, nextPos, statueData);
    }

}
