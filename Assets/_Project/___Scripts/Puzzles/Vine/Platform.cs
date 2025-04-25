using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour, IRespawnable
{
    private readonly List<VineScript> _triggerVines = new List<VineScript>();
    private VineScript _currentVine;
    private VineScript _previousVine;

    private Rigidbody _rb;
    private TreeStumpTest _tree;
    private bool _isFalling;

    [SerializeField] private float _speed = 2f;

    [SerializeField] private Vector3 _respawnPositon;
    [SerializeField] private Vector3 _respawnRotation;

    public event IRespawnable.RespawnEvent OnRespawn;

    public Vector3 RespawnPosition { get => _respawnPositon; set => _respawnPositon = value; }
    public Vector3 RespawnRotation { get => _respawnRotation; set => _respawnRotation = value; }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _tree = GetComponent<TreeStumpTest>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out VineScript vineScript))
        {
            if (!vineScript.IsActive) return;
            if (!_triggerVines.Contains(vineScript))
                _triggerVines.Add(vineScript);

            if (_currentVine != vineScript && _previousVine != vineScript)
            {
                if (_currentVine != null)
                    _currentVine.SetSocketNull();

                if (_isFalling) return;
                _previousVine = _currentVine;
                _currentVine = vineScript;
                SetPosition(vineScript);
            }
            _tree.CanInteract = false;
        }

        if (other.TryGetComponent(out ACharacter character))
        {
            Debug.Log("TriggerEnter with: " + other.name + ", and transform parent name: " + transform.name);
            character.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out VineScript vineScript)) { 
        
            _triggerVines.Remove(vineScript);

            if (_currentVine == vineScript)
            {
                _currentVine.SetSocketNull();
                _currentVine = null;
                _rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
                _rb.useGravity = true;
                _rb.isKinematic = false;
                _isFalling = true;
                //if (_triggerVines.Count > 0)
                //{
                //    //_currentVine = _triggerVines[_triggerVines.Count - 1];
                //    //SetPosition(_currentVine);
                //    _rb.constraints |= RigidbodyConstraints.FreezePositionY;
                //    _rb.useGravity = false;
                //    _rb.isKinematic = true;
                //}
            }
            _tree.CanInteract = true;
        }


        if (other.TryGetComponent(out ACharacter character))
        {
            Debug.Log("TriggerExit detected with: " + other.name);
            character.transform.SetParent(null);
        }
        
    }

    private void SetPosition(VineScript vine)
    {
        CapsuleCollider capsule = vine.gameObject.GetComponent<CapsuleCollider>();

        Vector3 WorldScale = capsule.transform.lossyScale;

        Vector3 CollisionPoint = capsule.ClosestPoint(transform.position);

        float WorldRadius = capsule.radius * WorldScale.y;

        Vector3 position = capsule.transform.TransformPoint(capsule.center);
        position.x = CollisionPoint.x;
        position.y += WorldRadius;
        position.z = CollisionPoint.z;

        StartCoroutine(MovePlatform(position, vine));
    }

    private IEnumerator MovePlatform(Vector3 position, VineScript vine)
    {
        while (Vector3.Distance(transform.position, position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, _speed * Time.deltaTime);
            yield return null;
        }

        transform.position = position;

        vine.SetSocketTransform(transform);
    }

    public void Respawn()
    {
        transform.position = RespawnPosition;
        transform.localEulerAngles = RespawnRotation;
        _rb.constraints |= RigidbodyConstraints.FreezePositionY;
        _rb.useGravity = false;
        _rb.isKinematic = true;
        _triggerVines.Clear();
        if (_currentVine != null)
            _currentVine.SetSocketNull();
        _currentVine = null;
        _previousVine = null;
        _tree.CanInteract = true;
        _isFalling = false;
    }
}
