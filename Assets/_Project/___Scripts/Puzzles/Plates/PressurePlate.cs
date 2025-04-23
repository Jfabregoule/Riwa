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
    [SerializeField] private LayerMask _whatIsPast;
    [SerializeField] private LayerMask _whatIsPlayer;

    private Vector3 _initialPosition;
    private Vector3 _destination;

    private bool _isTrigger = false;

    private HashSet<Collider> _validColliders = new HashSet<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.CurrentTemporality != _triggerInTemporality) return;

        bool updated = false;

        if (_canBeTriggeredWithPlayer && other.TryGetComponent(out ACharacter chara))
        {
            if (((1 << other.gameObject.layer) & _whatIsPlayer) != 0)
            {
                if (_validColliders.Add(other))
                    updated = true;
            }
        }

        if (_canBeTriggeredWithCrate && other.TryGetComponent(out Crate crate))
        {
            if (((1 << other.gameObject.layer) & _whatIsPast) != 0)
            {
                if (_validColliders.Add(other))
                    updated = true;
            }
        }

        if (updated && !_isTrigger)
        {
            _isTrigger = true;
            Activate();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (_validColliders.Remove(other) && _isTrigger && _validColliders.Count == 0)
        {
            _isTrigger = false;
            Deactivate();
        }
    }

    public void Activate()
    {
        _initialPosition = transform.position;
        _destination = _initialPosition + _offset;
        StartCoroutine(LerpPressed(true));
    }

    public void Deactivate()
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
        }
        else
        {
            OnDesactivated?.Invoke();
        }
    }
}
