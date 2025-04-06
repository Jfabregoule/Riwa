using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class VineScript : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> _renderers;
    [SerializeField] private float _waitBeforeFall;
    [SerializeField] private float _growingSpeed = 1;
    [SerializeField] private float _refreshRate = 0.05f;
    [SerializeField] private float _frictionSpeed;
    [SerializeField] private Transform _socketPoint;
    public float FrictionSpeed { get { return _frictionSpeed; } set { _frictionSpeed = value; } }

    [SerializeField, Range(0, 1)]
    private float _minGrow = 0.2f;
    [SerializeField, Range(0, 1)]
    private float _maxGrow = 0.97f;

    private List<Material> _materials = new List<Material>();
    [Header("Capsule Collider")]
    [SerializeField] private float _maxColliderHeight;
    [SerializeField] private float _refreshRateFactor = 70.0f;
    private CapsuleCollider _capsuleCollider;
    private float _minColliderHeight;
    private float _height;
    private Vector3 _test;
    private Vector3 _startSocketPos;
    void Start()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _minColliderHeight = _capsuleCollider.height;
        _startSocketPos = transform.TransformPoint(_capsuleCollider.center);
        for (int i = 0; i < _renderers.Count; i++) 
        {
            for (int j = 0; j < _renderers[i].materials.Length; j++)
            {
                if (_renderers[i].materials[j].HasProperty("_Grow"))
                {
                    _renderers[i].materials[j].SetFloat("_Grow",_minGrow);
                    _materials.Add(_renderers[i].materials[j]);
                }
            }
        }
    }

    private IEnumerator RaiseVine(Material mat)
    {
        float growValue = mat.GetFloat("_Grow");

        while (_maxGrow - growValue > 0.01f)
        {
            growValue = Mathf.MoveTowards(growValue, _maxGrow, _growingSpeed * Time.deltaTime);
            mat.SetFloat("_Grow", growValue);

            float lastHeight = _capsuleCollider.height;
            _capsuleCollider.height = _minColliderHeight + growValue * (_maxColliderHeight - _minColliderHeight);
            _capsuleCollider.center = new Vector3(_capsuleCollider.center.x - ((_capsuleCollider.height - lastHeight) / 2),_capsuleCollider.center.y,_capsuleCollider.center.z);

            Vector3 vector = -transform.right * (_capsuleCollider.height - _minColliderHeight) * transform.localScale.y;
            _socketPoint.position = new Vector3(_startSocketPos.x + vector.x,_socketPoint.position.y,_startSocketPos.z + vector.z);

            yield return null;
        }

        mat.SetFloat("_Grow", _maxGrow);
        StartCoroutine(DissolveVine());

    }

    private IEnumerator RetractedVine(Material mat)
    {
        float growValue = mat.GetFloat("_Grow");

        while (growValue - _minGrow > 0.01f)
        {
            growValue = Mathf.MoveTowards(growValue, _minGrow, _growingSpeed * Time.deltaTime);
            mat.SetFloat("_Grow", growValue);

            float lastHeight = _capsuleCollider.height;
            _capsuleCollider.height = _minColliderHeight + growValue * (_maxColliderHeight - _minColliderHeight);
            _capsuleCollider.center = new Vector3(_capsuleCollider.center.x - ((_capsuleCollider.height - lastHeight) / 2),_capsuleCollider.center.y,_capsuleCollider.center.z);

            Vector3 vector = -transform.right * (_capsuleCollider.height - _minColliderHeight) * transform.localScale.y;
            _socketPoint.position = new Vector3(_startSocketPos.x + vector.x,_socketPoint.position.y,_startSocketPos.z + vector.z);

            yield return null;

        }
    }
    private void VineFall()
    {
        //_capsuleCollider.enabled = false;
        StartCoroutine(RetractedVine(_materials[0]));
        //_capsuleCollider.isTrigger = true;
    }
    private IEnumerator DissolveVine()
    {
        yield return new WaitForSeconds(_waitBeforeFall);

        VineFall();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.TryGetComponent(out ACharacter character)) return;
        _capsuleCollider.isTrigger = false;
        StartCoroutine(RaiseVine(_materials[0]));
        //_isActivated = true;
    }


    public void SetSocketTransform(Vector3 position)
    {
        //position.y += 0.2f; 
        _socketPoint.transform.position = position;
        _test = position; 
    }

    public void SetSocketChild(Transform child)
    {
        child.SetParent(_socketPoint,true);
        child.localPosition = Vector3.zero;
    }

}
