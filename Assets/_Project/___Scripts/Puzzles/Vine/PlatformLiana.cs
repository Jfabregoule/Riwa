using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLiana : MonoBehaviour
{
    public Transform SocketPoint {  get; private set; }
    void Start()
    {
        SocketPoint = null;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void SetPlatformPosition(Vector3 position)
    {
        transform.localPosition = position;
    }
}
