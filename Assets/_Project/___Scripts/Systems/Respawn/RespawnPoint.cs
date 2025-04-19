using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{

    [SerializeField] private Vector3 _playerPosition;
    [SerializeField] private Vector3 _playerRotation = new (0,0,0);

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ACharacter player))
        {
            player.RespawnPosition = _playerPosition;
            player.RespawnRotation = _playerRotation;
        }
    }
}
