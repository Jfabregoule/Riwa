using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalItem : MonoBehaviour
{
    [SerializeField] private GameObject presentItem;

    public void UpdatePresentPosition()
    {
        presentItem.transform.position = transform.position;
        presentItem.transform.rotation = transform.rotation;
    }
}
