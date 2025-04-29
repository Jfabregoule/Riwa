using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ACharacter character))
        {
            GameManager.Instance.CollectibleManager.AddCollectible(1);
            Destroy(gameObject);
        }
    }
}
