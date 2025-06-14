using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VineRetraction : MonoBehaviour
{
    public UnityEvent OnGrow;
    public UnityEvent OnUnGrow;

    [Header("GOs")]
    [SerializeField] private GameObject _vine;

    [Header("Retraction data")]
    [SerializeField] private float _maxHeight;
    [SerializeField] private float _maxCenter;
    [SerializeField] private float _minCenter = 0f;
    [SerializeField] private BoxCollider _collider;
    [SerializeField] private float _thresholdToTrigger = 0.7f;
    [SerializeField] private float _minGrowCap = 0f;
    [SerializeField] private bool _isPivotBroken = false;

    private bool _thresholdReached = false;
    private float _growPercentage = 1f;

    public delegate void GrowthPercentageReached();
    public event GrowthPercentageReached OnGrowthPercentageReached;

    public delegate void GrowthPercentageUnreached();
    public event GrowthPercentageUnreached OnGrowthPercentageUnreached;

    private Material _mat;

    private float _originalHeight;
    private Vector3 _originalCenter;

    private void Start()
    {
        GameManager.Instance.OnTimeChangeStarted += SendGrowthPercentage;
        Renderer renderer = _vine.GetComponent<Renderer>();
        _mat = renderer.material;
        _originalHeight = _collider.size.z;
        _originalCenter = _collider.center;
        Debug.Log("Original height " + _originalHeight);
        Debug.Log("Original center " + _originalCenter);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Crate>(out Crate crate))
        {
            Debug.Log(crate.name + " is in the vine retraction zone");
            Vector3 localEntryPos;
            if(_isPivotBroken == false)
                localEntryPos = _vine.transform.InverseTransformPoint(other.transform.position);
            else
                localEntryPos = _vine.transform.parent.transform.InverseTransformPoint(other.transform.position);
            float zLocal = localEntryPos.z;
            float zMin = _originalCenter.z - _originalHeight * 0.5f;
            float zMax = _originalCenter.z + _originalHeight * 0.5f;
            _growPercentage = Mathf.Clamp01(Mathf.InverseLerp(zMin, zMax, zLocal));
            Debug.Log("Percentage " + _growPercentage);

            float growValue = Mathf.Lerp(_minGrowCap, 1f, _growPercentage);
            float height = Mathf.Lerp(1f, _maxHeight, _growPercentage);
            float centerZ = Mathf.Lerp(_maxCenter, _minCenter, _growPercentage);

            ChangeVineDatas(growValue, height, new Vector3(0, 0, centerZ));
            
            OnGrow.Invoke();
        }
    }

    private void SendGrowthPercentage(EnumTemporality temporality)
    {
        if(temporality == EnumTemporality.Present)
        {
            if (_growPercentage <= _thresholdToTrigger)
                OnGrowthPercentageReached?.Invoke();
            else
                OnGrowthPercentageUnreached?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Crate>(out Crate crate))
        {
            _growPercentage = 1f;
            ChangeVineDatas(1, _maxHeight, new Vector3(0, 0, _minCenter));
            OnUnGrow.Invoke();
        }
    }

    private void ChangeVineDatas(float growValue, float height, Vector3 center)
    {
        _mat.SetFloat("_Grow", growValue);
        _collider.center = center;
        _collider.size = new Vector3(_collider.size.x, _collider.size.y, height);

    }
}
