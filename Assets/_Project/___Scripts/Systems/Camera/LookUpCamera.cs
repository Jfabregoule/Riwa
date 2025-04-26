using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookUpCamera : MonoBehaviour
{
    
    [SerializeField] private Vector3 _targetCameraAngle = new Vector3(25,0,0);
    [SerializeField] private float _duration = 2;
    
    private CameraHandler _cam;
    private Vector3 _defaultCameraAngle;
    private Vector3 _currentCameraAngle;

    private Coroutine _currentCoroutine;
   

    public void Start()
    {
        _cam = GameManager.Instance.CameraHandler;
        _defaultCameraAngle = _cam.transform.localRotation.eulerAngles;
        _currentCameraAngle = _defaultCameraAngle;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(_currentCoroutine != null) StopCoroutine(_currentCoroutine);
            _currentCoroutine = StartCoroutine(CameraRotation(_currentCameraAngle, _targetCameraAngle)); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(_currentCoroutine != null) StopCoroutine(_currentCoroutine);
            _currentCoroutine = StartCoroutine(CameraRotation(_currentCameraAngle, _defaultCameraAngle)); 
        }
        
    }
    
    public IEnumerator CameraRotation(Vector3 from, Vector3 target)
    {
        float clock = 0;

        Quaternion a = Quaternion.Euler(from);
        Quaternion b = Quaternion.Euler(target);

        while (clock < _duration)
        {       
            clock += Time.deltaTime;    
            float t = clock / _duration;
            _cam.gameObject.transform.localRotation = Quaternion.Slerp(a,b,t);
            _currentCameraAngle = Vector3.Lerp(from, target, t);
            yield return null;
        }

    }


}
