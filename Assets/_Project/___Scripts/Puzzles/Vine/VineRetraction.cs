using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineRetraction : MonoBehaviour
{
    [SerializeField] private GameObject _vine;
    [SerializeField] private GameObject _box;

    private CapsuleCollider _boxCollider;
    private Material _mat;

    private void Start()
    {
        Renderer renderer = _vine.GetComponent<Renderer>();
        _boxCollider = _vine.GetComponent<CapsuleCollider>();
        _mat = renderer.material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _box)
        {
            Vector3 localEntryPos = _vine.transform.InverseTransformPoint(other.transform.position);
            float zLocal = localEntryPos.z;
            float zMin = _boxCollider.center.z - _boxCollider.height * 0.5f;
            float zMax = _boxCollider.center.z + _boxCollider.height * 0.5f;
            float zRatio = Mathf.InverseLerp(zMin, zMax, zLocal);

            float growValue = Mathf.Lerp(0f, 1f, zRatio);
            float height = Mathf.Lerp(1f, 13.5f, zRatio);
            float centerZ = Mathf.Lerp(-7f, 0f, zRatio);
            ChangeVineDatas(growValue, height, new Vector3(0, 0, centerZ));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _box)
            ChangeVineDatas(1, 13.5f, new Vector3(0, 0, -0.5f));
    }

    private void ChangeVineDatas(float growValue, float height, Vector3 center)
    {
        _mat.SetFloat("_Grow", growValue);
        _boxCollider.center = center;
        _boxCollider.height = height;

    }
}
