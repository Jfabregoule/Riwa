using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    [SerializeField] private DialogueAsset _collectibleDialogue;
    [SerializeField] private string _roomPrefix;

    private bool _taken = false;

    private void OnEnable()
    {
        LoadData();
        SaveSystem.Instance.OnLoadProgress += LoadData;
    }

    private void LoadData()
    {
        _taken = SaveSystem.Instance.LoadElement<bool>(_roomPrefix + "CollectiblePickUp");
    }

    private void OnDisable()
    {
        SaveSystem.Instance.OnLoadProgress -= LoadData;
    }

    private void Start()
    {
        if(_taken == true)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ACharacter character))
        {
            DialogueSystem.Instance.BeginDialogue(_collectibleDialogue);
            GameManager.Instance.CollectibleManager.AddCollectible(1);
            _taken = true;
            SaveSystem.Instance.SaveElement<bool>(_roomPrefix + "CollectiblePickUp", _taken);
            Destroy(gameObject);
        }
    }
}
