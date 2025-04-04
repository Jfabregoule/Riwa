using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLiana : MonoBehaviour
{

    void Start()
    {
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
