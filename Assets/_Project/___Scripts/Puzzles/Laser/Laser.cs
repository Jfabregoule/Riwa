using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Laser : MonoBehaviour
{
    private GameObject _cylinder;
    private Vector3 _startPos;
    void Start()
    {
        _cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        _cylinder.transform.position = transform.position;
        _cylinder.transform.rotation = new Quaternion(0f, 0f, -1f, 1f);
        _cylinder.transform.localScale = new Vector3(0.3f, 0f, 0.3f);

        _startPos = transform.position;

        if (Physics.Raycast(transform.position, transform.right, out RaycastHit hit, 10f))
        {
            float distance = Vector3.Distance(hit.point, _startPos);
            _cylinder.transform.localScale = new Vector3(0.3f, distance, 0.3f);
            _cylinder.transform.position = new Vector3(_startPos.x + (distance) * 0.5f, _startPos.y, _startPos.z);
        }
    }

    void Update()
    {
        //if (Physics.Raycast(transform.position, transform.right, out RaycastHit hit, 20f))
        //{
        //    float distance = Vector3.Distance(hit.point, transform.position);
        //    _cylinder.transform.localScale = new Vector3(0.3f, distance, 0.3f);
        //    _cylinder.transform.position = new Vector3(_startPos.x + (distance) * 0.5f, _startPos.y, _startPos.z);  
        //}

        //Debug.DrawRay(transform.position, transform.right * 10f, Color.red);
    }
}
