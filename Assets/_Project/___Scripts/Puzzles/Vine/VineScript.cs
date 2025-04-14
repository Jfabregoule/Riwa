using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineScript : MonoBehaviour, IInteractableSoul
{
    //[SerializeField] private List<MeshRenderer> _renderers;
    [SerializeField] private float _waitBeforeFall;
    [SerializeField] private float _growingSpeed = 1;
    [SerializeField] private float _retractedSpeed = 2;
    [SerializeField] private float _frictionSpeed;
    public float FrictionSpeed { get { return _frictionSpeed; } set { _frictionSpeed = value; } }

    public Transform SocketPoint { get; private set; }

    public float OffsetRadius { get => -1; set => throw new NotImplementedException(); }

    [SerializeField, Range(0, 1)]
    private float _minGrow = 0.2f;
    [SerializeField, Range(0, 1)]
    private float _maxGrow = 0.97f;

    private Material _material;
    //private List<Material> _materials = new List<Material>();
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
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _minColliderHeight = _capsuleCollider.height;
        _startSocketPos = transform.TransformPoint(_capsuleCollider.center);
        _material = GetComponent<MeshRenderer>().material;
        //for (int i = 0; i < _renderers.Count; i++) 
        //{
        //    for (int j = 0; j < _renderers[i].materials.Length; j++)
        //    {
        //        if (_renderers[i].materials[j].HasProperty("_Grow"))
        //        {
        //            _renderers[i].materials[j].SetFloat("_Grow",_minGrow);
        //            _materials.Add(_renderers[i].materials[j]);
        //        }
        //    }
        //}
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


            //if (SocketPoint != null)
            //{
            //    Vector3 vector = -transform.right * (_capsuleCollider.height - _minColliderHeight) * transform.localScale.y * _offset;
            //    SocketPoint.position = new Vector3(_startSocketPos.x + vector.x, SocketPoint.position.y, _startSocketPos.z + vector.z);
            //}

            yield return null;
        }
    }
    private void VineFall()
    {
        //SocketPoint.GetChild(0).GetComponent<Rigidbody>()

        StartCoroutine(RetractedVine());

    }
    private IEnumerator DissolveVine()
    {
        yield return new WaitForSeconds(_waitBeforeFall);

        VineFall();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!other.transform.TryGetComponent(out ACharacter character)) return;
    //    _capsuleCollider.isTrigger = false;
    //    StartCoroutine(RaiseVine());
    //}


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
        //VineManager.Instance.OnVineChange -= SetSocketPoint;
    }

    public void SetSocketNull()
    {
        //SocketPoint.GetChild(0).GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionY;
        //SocketPoint.GetChild(0).GetComponent<Rigidbody>().useGravity = true;

        SocketPoint = null;     
    }

    public void Interactable()
    {
        //_capsuleCollider.isTrigger = false;

    }

    public void InteractableSoul()
    {
        StartCoroutine(RaiseVine());
    }
}
