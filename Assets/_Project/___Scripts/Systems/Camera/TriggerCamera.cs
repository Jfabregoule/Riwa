using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCamera : MonoBehaviour
{
    public delegate void ExitTrigger(int id);
    public event ExitTrigger OnExitTrigger;

    private int _id; //Va permettre au camera handler de savoir quel zone vient d'etre trigger

    public int Id { get => _id; set => _id = value; }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ACharacter character)) {

            OnExitTrigger?.Invoke(_id);

        }
    }

}
