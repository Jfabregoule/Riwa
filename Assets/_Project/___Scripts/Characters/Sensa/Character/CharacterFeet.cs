using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterFeet : MonoBehaviour, IRespawnable
{
    [HideInInspector] public bool IsGround;
    private ACharacter _character;

    public System.Action OnFall;
    public System.Action OnGround;

    private readonly HashSet<Collider> _colliders = new();

    public Vector3 RespawnPosition { get ; set; }
    public Vector3 RespawnRotation { get ; set; }

    public event IRespawnable.RespawnEvent OnRespawn;

    public void Start()
    {
        _character = GameManager.Instance.Character;
        GameManager.Instance.OnTimeChangeStarted += ClearListOnChangeTempo;
    }

    public void OnDestroy()
    {
        if (GameManager.Instance)
            GameManager.Instance.OnTimeChangeStarted -= ClearListOnChangeTempo;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (IsValidObject(other, GameManager.Instance.CurrentTemporality))
        {
            _colliders.Add(other);
            if (_colliders.Count == 1)
            {
                IsGround = true;
                OnGround?.Invoke();
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (IsValidObject(other, GameManager.Instance.CurrentTemporality))
        {
            _colliders.Remove(other);
            if (_colliders.Count == 0)
            {
                IsGround = false;
                OnFall?.Invoke();
            }
        }
    }

    private bool IsValidObject(Collider collider, EnumTemporality currentTempo)
    {
        return collider.gameObject != gameObject
            && collider.gameObject != _character.gameObject
            && 1 << collider.gameObject.layer == (GameManager.Instance.CurrentTemporality == EnumTemporality.Past ? _character.PastLayer : _character.PresentLayer);
    }

    private void ClearListOnChangeTempo(EnumTemporality temporality)
    {
        SphereCollider sphere = GetComponent<SphereCollider>();

        Collider[] colliders = Physics.OverlapSphere(transform.position, sphere.radius * transform.localScale.x);

        foreach (var col in colliders) 
        {
            if (IsValidObject(col, temporality)) 
            { 
                _colliders.Add(col);
            }
        }

        foreach (var collider in _colliders.ToList())
        {
            if (1 << collider.gameObject.layer != (temporality == EnumTemporality.Past ? _character.PastLayer : _character.PresentLayer))
            {
                _colliders.Remove(collider);
            }
        }
    }

    public void Respawn()
    {
        
    }
}
