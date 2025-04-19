using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRespawnable
{
    public delegate void RespawnEvent();
    event RespawnEvent OnRespawn; 

    Vector3 RespawnPosition { get; set; }
    Vector3 RespawnRotation { get; set; }

    void Respawn();

}
