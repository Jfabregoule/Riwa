using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Tilemaps.Tile;

public class PlacementZone : MonoBehaviour
{
    public delegate void PlaceEvent(GameObject go);
    public event PlaceEvent OnPlace;
    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnPlace?.Invoke(other.gameObject);
    }
}
