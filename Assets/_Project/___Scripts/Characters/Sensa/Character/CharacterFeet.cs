using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterFeet : MonoBehaviour, IRespawnable
{
    [SerializeField] private float _fallThreshold = 0.25f;
    private float _currentThreshold;

    public bool IsGround;
    private ACharacter _character;
    private float _radius;

    private int _playerMask;

    public System.Action OnFall;
    public System.Action OnGround;

    private readonly HashSet<Collider> _colliders = new();

    public Vector3 RespawnPosition { get ; set; }
    public Vector3 RespawnRotation { get ; set; }

    public event IRespawnable.RespawnEvent OnRespawn;

    public void Start()
    {
        _character = GameManager.Instance.Character;
        _fallThreshold *= _character.transform.localScale.y;

        _currentThreshold = _fallThreshold;
        _radius = _character.GetComponent<CapsuleCollider>().radius * _character.transform.localScale.x * 1.1f;

        int playerLayer = LayerMask.NameToLayer("whatIsPlayer");
        _playerMask = ~(1 << playerLayer);

    }

    public void Update()
    {
        LayerMask mask = GameManager.Instance.CurrentTemporality == EnumTemporality.Past ? _character.PastLayer : _character.PresentLayer;

        IsGround = Physics.CheckCapsule(transform.position, transform.position - Vector3.up * _currentThreshold, _radius, mask, QueryTriggerInteraction.Ignore);
        
        Color color = IsGround ? Color.green : Color.red;
        Debug.DrawLine(transform.position, transform.position - Vector3.up * _currentThreshold, color);
        Debug.DrawLine(transform.position, transform.position + Vector3.right * _radius, color);
    }

    private bool IsValidObject(Collider collider, EnumTemporality currentTempo)
    {
        return collider.gameObject != gameObject
            && collider.gameObject != _character.gameObject
            && 1 << collider.gameObject.layer == (GameManager.Instance.CurrentTemporality == EnumTemporality.Past ? _character.PastLayer : _character.PresentLayer);
    }

    private void ClearListOnChangeTempo(EnumTemporality temporality)
    {
        CapsuleCollider sphere = GetComponent<CapsuleCollider>();

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
        OnRespawn?.Invoke();
    }
}
