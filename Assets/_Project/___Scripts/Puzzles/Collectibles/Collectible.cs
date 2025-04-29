using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    [SerializeField] private DialogueAsset _collectibleDialogue;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ACharacter character))
        {
            DialogueSystem.Instance.BeginDialogue(_collectibleDialogue);
            GameManager.Instance.CollectibleManager.AddCollectible(1);
            Destroy(gameObject);
        }
    }
}
