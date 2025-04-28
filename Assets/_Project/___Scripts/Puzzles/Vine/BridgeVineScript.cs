using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeVineScript : MonoBehaviour, IInteractableSoul
{
    [SerializeField] private float _growingSpeed = 1;
    [SerializeField] private float _frictionSpeed;
    [SerializeField] private Animator _bourgeonAnimator;
    [SerializeField] private List<ParticleSystem> _bourgeonActivationVFX;
    public float FrictionSpeed { get { return _frictionSpeed; } set { _frictionSpeed = value; } }

    public float OffsetRadius { get => -1; set => throw new NotImplementedException(); }
    public bool CanInteract { get; set; }
    public int Priority { get ; set; }

    [SerializeField, Range(0, 1)]
    private float _minGrow = 0.2f;
    [SerializeField, Range(0, 1)]
    private float _maxGrow = 0.97f;

    private Material _material;

    [Header("Capsule Collider")]
    [SerializeField] private float _maxColliderHeight;
    private BoxCollider _boxCollider;
    private float _minColliderHeight;

    private DialogueSystem _dialogueSystem;
    public Action OnInteract;

    void Start()
    {
        Priority = 1;
        _boxCollider = GetComponent<BoxCollider>();
        _minColliderHeight = _boxCollider.size.x;
        _material = GetComponent<MeshRenderer>().material;
        CanInteract = true;

        StartCoroutine(Helpers.WaitMonoBeheviour(() => DialogueSystem.Instance, SubscribeToDialogueSystem));
    }

    private IEnumerator RaiseVine()
    {
        CanInteract = false;
        float growValue = _material.GetFloat("_Grow");

        while (_maxGrow - growValue > 0.01f)
        {
            growValue = Mathf.MoveTowards(growValue, _maxGrow, _growingSpeed * Time.deltaTime);
            _material.SetFloat("_Grow", growValue);

            float lastHeight = _boxCollider.size.x;
            _boxCollider.size = new Vector3(_minColliderHeight + growValue * (_maxColliderHeight - _minColliderHeight), _boxCollider.size.y, _boxCollider.size.z);
            _boxCollider.center = new Vector3(_boxCollider.center.x - ((_boxCollider.size.x - lastHeight) / 2),_boxCollider.center.y,_boxCollider.center.z);

            yield return null;
        }

        _material.SetFloat("_Grow", _maxGrow);
    }

    public void Interact()
    {
        if (CanInteract)
            _dialogueSystem.EventRegistery.Invoke(WaitDialogueEventType.LianaFloor1Room2);
    }
    public void InteractableSoul()
    {
        if (_bourgeonAnimator)
            _bourgeonAnimator.SetBool("Activate", true);
        if(_bourgeonActivationVFX.Count > 0)
            foreach(var particle in _bourgeonActivationVFX)
            {
                particle.Play();
            }
        StartCoroutine(RaiseVine());
    }

    private void SubscribeToDialogueSystem(DialogueSystem script)
    {
        if (script != null)
        {
            _dialogueSystem = script;
            _dialogueSystem.EventRegistery.Register(WaitDialogueEventType.LianaFloor1Room2, OnInteract);
        }
    }
}
