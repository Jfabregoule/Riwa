using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRespawnable
{
    Vector3 RespawnPosition { get; set; }
    Vector3 RespawnRotation { get; set; }

    void Respawn();

}
