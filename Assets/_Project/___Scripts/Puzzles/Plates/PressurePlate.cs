using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour, IActivable
{
    public event IActivable.ActivateEvent OnActivated;
    public event IActivable.ActivateEvent OnDesactivated;

    [SerializeField] private float _lerpTime = 0.3f;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private EnumTemporality _triggerInTemporality = EnumTemporality.Past;
    [SerializeField] private bool _canBeTriggeredWithPlayer = true;
    [SerializeField] private bool _canBeTriggeredWithCrate = true;
    //[SerializeField] private LayerMask _whatIsPast;
    //[SerializeField] private LayerMask _whatIsPlayer;

    private Vector3 _initialPosition;
    private Vector3 _destination;

    private bool _isTrigger = false;

    private HashSet<Collider> _validColliders = new HashSet<Collider>();

    private void Start()
    {
        GameManager.Instance.CurrentLevelManager.OnLevelEnter += CheckForObjectsOnStart;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.CurrentTemporality != _triggerInTemporality) return;

        if ((_canBeTriggeredWithPlayer && other.TryGetComponent(out ACharacter chara)) ||
            (_canBeTriggeredWithCrate && other.TryGetComponent(out Crate crate)))
        {
            if (_validColliders.Add(other))
            {
                if (!_isTrigger)
                {
                    _isTrigger = true;
                    Activate();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_validColliders.Remove(other))
        {
            if (_validColliders.Count == 0 && _isTrigger)
            {
                _isTrigger = false;
                Desactivate();
            }
        }
    }

    public void Activate()
    {
        _initialPosition = transform.position;
        _destination = _initialPosition + _offset;
        StartCoroutine(LerpPressed(true));
    }

    public void Desactivate()
    {
        _initialPosition = transform.position;
        _destination = _initialPosition - _offset;
        StartCoroutine(LerpPressed(false));
    }

    IEnumerator LerpPressed(bool active)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < _lerpTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _lerpTime;
            t = Mathf.Clamp01(t);
            transform.position = Vector3.Lerp(_initialPosition, _destination, t);
            yield return null;
        }
        transform.position = _destination;
        if (active)
        {
            OnActivated?.Invoke();
            OnPressurePlateStateUpdated(true);
        }
        else
        {
            OnDesactivated?.Invoke();
            OnPressurePlateStateUpdated(false);
        }
    }

    private void OnPressurePlateStateUpdated(bool isActive)
    {
        float shaderIsActiveFloatValue = 0;
        if (isActive) shaderIsActiveFloatValue = 1;

        Renderer renderer = GetComponent<Renderer>();

        if (renderer.material.HasProperty("_IsActivated"))
            renderer.material.SetFloat("_IsActivated", shaderIsActiveFloatValue);

        if (transform.parent.gameObject.TryGetComponent(out Renderer parentRenderer))
        {
            if (parentRenderer.material.HasProperty("_IsActivated"))
                parentRenderer.material.SetFloat("_IsActivated", shaderIsActiveFloatValue);
        }
    }

    private void CheckForObjectsOnStart()
    {
        Collider[] overlappingColliders = Physics.OverlapBox(
            transform.position,
            GetComponent<Collider>().bounds.extents,
            transform.rotation
        );

        foreach (var collider in overlappingColliders)
        {
            if ((_canBeTriggeredWithPlayer && collider.TryGetComponent(out ACharacter chara)) ||
                (_canBeTriggeredWithCrate && collider.TryGetComponent(out Crate crate)))
            {
                if (_validColliders.Add(collider))
                {
                    if (!_isTrigger)
                    {
                        _isTrigger = true;
                        Activate();
                    }
                }
            }
        }
    }
}
