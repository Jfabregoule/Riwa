using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformFeet : MonoBehaviour
{
    [SerializeField] private GameObject _plateform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 platformTop = _plateform.transform.position + Vector3.up * (_plateform.GetComponent<Collider>().bounds.extents.y);

            other.transform.position = new Vector3(
                other.transform.position.x,
                platformTop.y + 0.5f,
                other.transform.position.z
            );
        }
    }
}
