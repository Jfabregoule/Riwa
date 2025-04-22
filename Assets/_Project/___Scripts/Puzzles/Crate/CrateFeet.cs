using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrateFeet : MonoBehaviour
{
    private GameObject _crate;
    private ACharacter _character;
    private Rigidbody _rb;

    [SerializeField] float _duration;

    public bool IsGround;

    private HashSet<Collider> _colliders = new();

    public void Start()
    {
        _crate = transform.parent.gameObject;
        _character = GameManager.Instance.Character;
        _rb = _crate.GetComponent<Rigidbody>();

        IsGround = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (IsValidObject(other))
        {
            _colliders.Add(other);
            if (_colliders.Count == 1)
            {
                ToucheGround();
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (IsValidObject(other))
        {
            _colliders.Remove(other);
            if( _colliders.Count == 0)
            {
                LeaveGround();
            }
        }
    }

    private void ToucheGround()
    {
        IsGround = true;
    }

    bool oue = true;
    private void LeaveGround()
    {
        IsGround = false;
        _character.StateMachine.ChangeState(_character.StateMachine.States[EnumStateCharacter.Idle]);
        if (oue)
        {
            oue = false;
            StartCoroutine(Fall());
        }

    }

    private bool IsValidObject(Collider collider)
    {
        return collider.gameObject != gameObject 
            && collider.gameObject != _crate
            && 1 << collider.gameObject.layer == (GameManager.Instance.CurrentTemporality == EnumTemporality.Past ? _character.PastLayer : _character.PresentLayer);
    }

    public IEnumerator Fall()
    {
        float clock = 0;
        Vector3 startPos = _crate.transform.position - Vector3.up * (_crate.GetComponent<BoxCollider>().size.y * _crate.transform.localScale.y / 2);

        LayerMask layerMask = GameManager.Instance.CurrentTemporality == EnumTemporality.Past ? _character.PastLayer : _character.PresentLayer;

        RaycastHit hit;
        Physics.Raycast(startPos, -Vector3.up, out hit, 100, layerMask);
        Debug.DrawRay(startPos, -Vector3.up, UnityEngine.Color.red, 20f);
        Vector3 targetPos = hit.point;
        targetPos += Vector3.up * (_crate.GetComponent<BoxCollider>().size.y * _crate.transform.localScale.y / 2) * 1.2f;
        Debug.DrawRay(targetPos, Vector3.right, UnityEngine.Color.green, 20f);
        

        while (clock < _duration)
        {
            clock += Time.deltaTime;
            float t = Mathf.Clamp01(clock / _duration);
            _crate.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        _crate.transform.position = targetPos;

        yield break;

    }
}
