using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oui : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ACharacter character))
            character.TriggerChangeTempo();
    }
}
