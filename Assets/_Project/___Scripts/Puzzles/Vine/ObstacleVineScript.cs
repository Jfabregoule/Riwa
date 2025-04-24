using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleVineScript : MonoBehaviour, IInteractableSoul
{
    [SerializeField] private float _growingSpeed = 1;
    [SerializeField] private float _frictionSpeed;
    public float FrictionSpeed { get { return _frictionSpeed; } set { _frictionSpeed = value; } }

    public float OffsetRadius { get => -1; set => throw new NotImplementedException(); }
    public bool CanInteract { get; set; }

    [SerializeField, Range(0, 1)]
    private float _minGrow = 0.2f;
    [SerializeField, Range(0, 1)]
    private float _maxGrow = 0.97f;

    private Material _material;

    [Header("Capsule Collider")]
    [SerializeField] private float _maxColliderHeight;
    private BoxCollider _boxCollider;
    private float _minColliderHeight;

    void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _minColliderHeight = _boxCollider.size.x;
        _material = GetComponent<MeshRenderer>().material;
        CanInteract = true;

        // Start fully grown
        _material.SetFloat("_Grow", _maxGrow);

        float currentHeight = _minColliderHeight + _maxGrow * (_maxColliderHeight - _minColliderHeight);
        _boxCollider.size = new Vector3(currentHeight, _boxCollider.size.y, _boxCollider.size.z);
    }

    private IEnumerator ShrinkVine()
    {
        float growValue = _material.GetFloat("_Grow");

        while (growValue - _minGrow > 0.01f)
        {
            growValue = Mathf.MoveTowards(growValue, _minGrow, _growingSpeed * Time.deltaTime);
            _material.SetFloat("_Grow", growValue);

            float newHeight = _minColliderHeight + growValue * (_maxColliderHeight - _minColliderHeight);

            _boxCollider.size = new Vector3(newHeight, _boxCollider.size.y, _boxCollider.size.z);

            // Center of collider is halfway along its length
            _boxCollider.center = new Vector3(-newHeight / 2f, _boxCollider.center.y, _boxCollider.center.z);

            yield return null;
        }

        _material.SetFloat("_Grow", _minGrow);
    }

    public void Interact()
    {
        // Optional: Trigger shrink via Interact
    }

    public void InteractableSoul()
    {
        StartCoroutine(ShrinkVine());
    }
}
