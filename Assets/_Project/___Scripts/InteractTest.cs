using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTest : MonoBehaviour, IHoldable
{
    public float OffsetRadius { get; set; }

    InteractTest()
    {
        OffsetRadius = 1;
    }

    public void Interactable()
    {
        Debug.Log("GameObject name: " + name);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hold()
    {
        throw new System.NotImplementedException();
    }
}
