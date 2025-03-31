using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private int _id;
    private Statue _statue;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Statue statue))
        {
            _statue = statue;
            float yRotation = statue.gameObject.transform.eulerAngles.y;
            if (yRotation > 180f)
                yRotation -= 360f;
            if (statue.ID == _id && Mathf.Approximately(yRotation, statue.FinalRotation))
            {
                Debug.Log("Y Rotation: " + yRotation + " | FinalRotation: " + statue.FinalRotation);
                Debug.Log("YES ?: " + (Mathf.Approximately(yRotation, statue.FinalRotation)));
                statue.Validate = true;
                Debug.Log(statue.name + " is well placed and rotated !");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == _statue.gameObject)
        {
            _statue = null;
            Debug.Log("Statue exit her tile");
        }
    }

    private void FixedUpdate()
    {
        Debug.Log("ZZZZZZZZZZZ");
        if (!_statue) return;
        Debug.Log("YYYYYYYYYYYY");
        float yRotation = _statue.gameObject.transform.eulerAngles.y;
        if (yRotation > 180f)
            yRotation -= 360f;
        if(_statue.Validate == false && Mathf.Approximately(yRotation, _statue.FinalRotation))
        {
            _statue.Validate = true;
            Debug.Log(_statue.name + " is well placed and rotated !");
        }
    }
}
