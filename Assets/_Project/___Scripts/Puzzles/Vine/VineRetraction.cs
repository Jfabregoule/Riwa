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
    [SerializeField] private List<GameObject> _box;

    [Header("Retraction data")]
    [SerializeField] private float _maxHeight;
    [SerializeField] private float _maxCenter;
    [SerializeField] private BoxCollider _collider;
    [SerializeField] private float _thresholdToTrigger = 0.7f;

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
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Percentage " + _growPercentage);
        if (other.TryGetComponent<Crate>(out Crate crate))
        {
            Vector3 localEntryPos = _vine.transform.InverseTransformPoint(other.transform.position);
            float zLocal = localEntryPos.z;
            float zMin = _originalCenter.z - _originalHeight * 0.5f;
            float zMax = _originalCenter.z + _originalHeight * 0.5f;
            _growPercentage = Mathf.Clamp01(Mathf.InverseLerp(zMin, zMax, zLocal));

            float growValue = Mathf.Lerp(0f, 1f, _growPercentage);
            float height = Mathf.Lerp(1f, _maxHeight, _growPercentage);
            float centerZ = Mathf.Lerp(_maxCenter, 0f, _growPercentage);

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
        if (_box.Contains(other.gameObject))
        {
            _growPercentage = 1f;
            ChangeVineDatas(1, 13.5f, new Vector3(0, 0, -0.5f));
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
