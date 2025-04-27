using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrateSaveInteraction : MonoBehaviour
{

    private bool _isFirstInteraction;

    private void Start()
    {
        _isFirstInteraction = SaveSystem.Instance.LoadElement<bool>("IsFirstInteraction", false);
        Debug.Log(_isFirstInteraction);
    }
    private void OnEnable()
    {
        GameManager.Instance.Character.OnInteractStarted += SetInteraction;
    }
 

    private void SetInteraction()
    {
        if (!_isFirstInteraction) return;
        _isFirstInteraction = false;
        SaveSystem.Instance.SaveElement<bool>("IsFirstInteraction", _isFirstInteraction, false);
        GameManager.Instance.InvokeInteractInput();
    }
}
