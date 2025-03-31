using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : MonoBehaviour, IMovable
{

    [SerializeField] private float _mass;
    [SerializeField] private float _finalRotation;
    [SerializeField] private int _id;
    [SerializeField] private StatuePuzzle _gridManager;
    private CellPos _pos;

    private bool _validate;

    public int ID { get => _id; }
    public bool Validate { get => _validate; set => _validate = value; }
    public float FinalRotation { get => _finalRotation; }

    void Start()
    {
        _validate = false;
        if (_mass == 0) _mass = 1.0f;
        AlignToGrid();
    }

    void Update()
    {
        Vector3 direction = Vector3.zero;
        Vector3 rotation = Vector3.zero;

        if (_validate == true) return;
        if (Input.GetKeyDown(KeyCode.W)) direction = Vector3.forward;
        if (Input.GetKeyDown(KeyCode.S)) direction = Vector3.back;
        if (Input.GetKeyDown(KeyCode.A)) direction = Vector3.left;
        if (Input.GetKeyDown(KeyCode.D)) direction = Vector3.right;
        if (Input.GetKeyDown(KeyCode.E)) rotation = Vector3.up * 45f;

        if (direction != Vector3.zero)
            Move(direction * _gridManager.UnitGridSize);

        if(rotation != Vector3.zero)
            transform.Rotate(rotation);
    }


    public float Move(Vector3 direction)
    {
        if (_gridManager == null) return 0f;

        float speed = 1 / _mass;
        Vector3 newPosition = transform.position + direction * speed;
        Vector3 relativePos = newPosition - _gridManager.Origin;

        int gridX = Mathf.RoundToInt(relativePos.x / _gridManager.UnitGridSize);
        int gridY = Mathf.RoundToInt(relativePos.z / _gridManager.UnitGridSize);

        if (gridX < 0 || gridX >= _gridManager.GridSize.x ||
            gridY < 0 || gridY >= _gridManager.GridSize.y)
        {
            return 0f;
        }

        transform.position = _gridManager.Origin + new Vector3(gridX * _gridManager.UnitGridSize, transform.localPosition.y, gridY * _gridManager.UnitGridSize);
        return speed;
    }

    private void AlignToGrid()
    {
        if (_gridManager == null) return;

        Vector3 relativePos = transform.position - _gridManager.Origin;
        int gridX = Mathf.RoundToInt(relativePos.x / _gridManager.UnitGridSize);
        int gridY = Mathf.RoundToInt(relativePos.z / _gridManager.UnitGridSize);

        transform.position = _gridManager.Origin + new Vector3(gridX * _gridManager.UnitGridSize, transform.localPosition.y, gridY * _gridManager.UnitGridSize);
    }

}
