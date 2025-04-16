using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTriggerZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IRespawnable>(out IRespawnable obj))
        {
            obj.Respawn();
        }
        else
        {
            Destroy(other.gameObject);
        }
    }
}
