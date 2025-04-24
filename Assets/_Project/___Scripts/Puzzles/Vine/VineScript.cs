using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineScript : MonoBehaviour, IInteractableSoul
{
    [SerializeField] private float _waitBeforeFall;
    [SerializeField] private float _growingSpeed = 1;
    [SerializeField] private float _retractedSpeed = 2;
    [SerializeField] private Animator _bourgeonAnimator;
    [SerializeField] private List<ParticleSystem> _onActivateParticles;

    public Transform SocketPoint { get; private set; }

    public float OffsetRadius { get => -1; set => throw new NotImplementedException(); }

    public bool IsActive { get; private set; }
    public bool CanInteract { get => !IsActive; set => IsActive = !value; }

    [SerializeField, Range(0, 1)]
    private float _minGrow = 0.2f;
    [SerializeField, Range(0, 1)]
    private float _maxGrow = 0.97f;

    private Material _material;
    [Header("Capsule Collider")]
    [SerializeField] private float _maxColliderHeight;
    private CapsuleCollider _capsuleCollider;
    private float _minColliderHeight;
    private float _height;
    private float _offset;
    private Vector3 _startSocketPos;

    void Start()
    {
        SocketPoint = null;
        IsActive = false;
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _minColliderHeight = _capsuleCollider.height;
        _startSocketPos = transform.TransformPoint(_capsuleCollider.center);
        _material = GetComponent<MeshRenderer>().material;
    }

    private void OnEnable()
    {
        if(GameManager.Instance.Character)
            GameManager.Instance.Character.OnRespawn += RetractedAllVines;
    }

    private void OnDisable()
    {
        if (GameManager.Instance)
            GameManager.Instance.Character.OnRespawn -= RetractedAllVines;
    }
    private IEnumerator RaiseVine()
    {
        float growValue = _material.GetFloat("_Grow");

        while (_maxGrow - growValue > 0.01f)
        {
            growValue = Mathf.MoveTowards(growValue, _maxGrow, _growingSpeed * Time.deltaTime);
            _material.SetFloat("_Grow", growValue);

            float lastHeight = _capsuleCollider.height;
            _capsuleCollider.height = _minColliderHeight + growValue * (_maxColliderHeight - _minColliderHeight);
            _capsuleCollider.center = new Vector3(_capsuleCollider.center.x - ((_capsuleCollider.height - lastHeight) / 2),_capsuleCollider.center.y,_capsuleCollider.center.z);

 
            if (SocketPoint != null)
            {
                Vector3 vector = -transform.right * (_capsuleCollider.height - _minColliderHeight) * transform.localScale.y * _offset;
                SocketPoint.position = new Vector3(_startSocketPos.x + vector.x,SocketPoint.position.y,_startSocketPos.z + vector.z);
            }

            yield return null;
        }

        _material.SetFloat("_Grow", _maxGrow);
        StartCoroutine(DissolveVine());

    }

    private IEnumerator RetractedVine()
    {
        float growValue = _material.GetFloat("_Grow");

        while (growValue - _minGrow > 0.01f)
        {
            growValue = Mathf.MoveTowards(growValue, _minGrow, _retractedSpeed * Time.deltaTime);
            _material.SetFloat("_Grow", growValue);

            float lastHeight = _capsuleCollider.height;
            _capsuleCollider.height = _minColliderHeight + growValue * (_maxColliderHeight - _minColliderHeight);
            _capsuleCollider.center = new Vector3(_capsuleCollider.center.x - ((_capsuleCollider.height - lastHeight) / 2),_capsuleCollider.center.y,_capsuleCollider.center.z);

            yield return null;
        }
    }
    private void VineFall()
    {
        if (_bourgeonAnimator)
            _bourgeonAnimator.SetBool("Activate", false);
        StartCoroutine(RetractedVine());
        IsActive = false;

    }
    private IEnumerator DissolveVine()
    {
        yield return new WaitForSeconds(_waitBeforeFall);

        VineFall();
    }


    public void SetSocketTransform(Transform transformObject)
    {

        SocketPoint = transformObject;
        float currentHeight = _capsuleCollider.height;
        float hitDistance = Vector3.Dot(transformObject.position - transform.position, -transform.right);
        _offset = hitDistance / (currentHeight * transform.localScale.y);
    }


    public void SetSocketPoint()
    {
        SocketPoint = null;
    }

    public void SetSocketNull()
    {
        SocketPoint = null;     
    }

    public void Interact()
    {

    }

    public void InteractableSoul()
    {
        IsActive = true;
        if(_bourgeonAnimator)
            _bourgeonAnimator.SetBool("Activate", true);
        if(_onActivateParticles.Count > 0)
        {
            for(int i = 0;  i < _onActivateParticles.Count; i++)
            {
                _onActivateParticles[i].Play();
            }
        }
        StartCoroutine(RaiseVine());
    }

    private void RetractedAllVines()
    {
        if (IsActive)
        {
            if (_bourgeonAnimator)
                _bourgeonAnimator.SetBool("Activate", false);
            StopAllCoroutines();
            StartCoroutine(RetractedVine());
            IsActive = false;
        }
    }
}
