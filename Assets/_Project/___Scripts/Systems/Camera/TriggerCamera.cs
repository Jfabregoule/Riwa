using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCamera : MonoBehaviour
{
    public delegate void ExitTrigger();
    public event ExitTrigger OnExitTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ACharacter character)) {

            OnExitTrigger.Invoke();

        }
    }

}
