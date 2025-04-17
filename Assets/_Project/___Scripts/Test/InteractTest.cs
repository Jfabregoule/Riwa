using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTest : MonoBehaviour, IInteractable
{
    public float OffsetRadius { get; set; }

    private void Start()
    {
        OffsetRadius = 1.5f;
    }

    public void Interactable()
    {
        Debug.Log("Je fais ma vie");
    }

}
