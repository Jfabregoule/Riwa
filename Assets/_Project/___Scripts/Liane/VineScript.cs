using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class VineScript : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> _renderers;
    [SerializeField] private float _waitBeforeFall;
    [SerializeField] private float _growingSpeed = 1;
    [SerializeField] private float _refreshRate = 0.05f;
    [SerializeField] private List<Vector3> _positions;
    [SerializeField] private float _frictionSpeed;

    public float FrictionSpeed { get { return _frictionSpeed; } set { _frictionSpeed = value; } }

    [SerializeField, Range(0, 1)]
    private float _minGrow = 0.2f;
    [SerializeField, Range(0, 1)]
    private float _maxGrow = 0.97f;

    private List<Material> _materials = new List<Material>();
    private bool _isFullyGrown;
    [SerializeField] private bool _isActivated;

    [Header("Capsule Collider")]
    [SerializeField] private float _maxColliderHeight;
    [SerializeField] private float _refreshRateFactor = 70.0f;
    private CapsuleCollider _capsuleCollider;
    private float _minCapsuleHeight;

    void Start()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _minCapsuleHeight = _capsuleCollider.height;
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

    // Update is called once per frame
    void Update()
    {
        if (_isActivated)
        {
            RaiseVine(_materials[0]);
        }
    }

    private void RaiseVine(Material mat)
    {
        float growValue = mat.GetFloat("_Grow");
        float currentHeight = _capsuleCollider.height;
        float value = Mathf.MoveTowards(growValue, _maxGrow, _growingSpeed * Time.deltaTime);

        if(_maxGrow - growValue <= 0.01f)
        {
            _maxGrow = _minGrow;
            _isActivated = false;
            StartCoroutine(DissolveVine());
        }

        mat.SetFloat("_Grow", value);
        float lastHeight = _capsuleCollider.height;
        _capsuleCollider.height = _minCapsuleHeight + value * (_maxColliderHeight - _minCapsuleHeight);

        _capsuleCollider.center = new Vector3(_capsuleCollider.center.x - ((_capsuleCollider.height - lastHeight) / 2), _capsuleCollider.center.y, _capsuleCollider.center.z);
    }

    private void VineFall()
    {
        _capsuleCollider.enabled = false;
    }
    private IEnumerator DissolveVine()
    {
        yield return new WaitForSeconds(_waitBeforeFall);

        VineFall();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.parent.TryGetComponent(out Character character)) return;
        _capsuleCollider.isTrigger = false;
        _isActivated = true;
    }

    public List<Vector3> GetSpline()
    {
        return _positions;
    }
}
