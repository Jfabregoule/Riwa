using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour, IRespawnable
{


    private readonly List<VineScript> _triggerVines = new List<VineScript>();
    private VineScript _currentVine;
    private VineScript _previousVine;

    private Rigidbody _rb;

    [SerializeField] private Vector3 _respawnPositon;
    [SerializeField] private Vector3 _respawnRotation;

    public event IRespawnable.RespawnEvent OnRespawn;

    public Vector3 RespawnPosition { get => _respawnPositon; set => _respawnPositon = value; }
    public Vector3 RespawnRotation { get => _respawnRotation; set => _respawnRotation = value; }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Debug.Log(_respawnPositon);
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

                _previousVine = _currentVine;
                _currentVine = vineScript;
                SetPosition(vineScript);
            }
        }

        if (other.TryGetComponent(out ACharacter character))
            other.transform.SetParent(transform);
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

                if (_triggerVines.Count > 0)
                {
                    _currentVine = _triggerVines[_triggerVines.Count - 1];
                    SetPosition(_currentVine);
                    _rb.constraints |= RigidbodyConstraints.FreezePositionY;
                    _rb.useGravity = false;
                    _rb.isKinematic = true;
                }
            }
        }


        if (other.TryGetComponent(out ACharacter character))
            other.transform.SetParent(null);
        
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
        //transform.position = position;
        //vine.SetSocketTransform(transform);

        StartCoroutine(MovePlatform(position, vine));
    }

    private IEnumerator MovePlatform(Vector3 position, VineScript vine)
    {
        float speed = 2f; 

        while (Vector3.Distance(transform.position, position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
            yield return null;
        }

        transform.position = position;

        vine.SetSocketTransform(transform);
    }

    public void Respawn()
    {
        //transform.GetChild(0).SetParent(null);
        transform.position = RespawnPosition;
        transform.localEulerAngles = RespawnRotation;
        _rb.constraints |= RigidbodyConstraints.FreezePositionY;
        _rb.useGravity = false;
        _rb.isKinematic = true;
    }
}
