using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    private int _nbCollectibles = 0;

    public delegate void CollectibleEventShow();
    public delegate void CollectibleEvent(int nb);
    public event CollectibleEventShow OnCollect;
    public event CollectibleEvent OnCollectAdd;
    public event CollectibleEvent OnCollectAll;

    private void OnEnable()
    {
        LoadData();
        SaveSystem.Instance.OnLoadProgress += LoadData;
    }

    private void LoadData()
    {
        _nbCollectibles = SaveSystem.Instance.LoadElement<int>("Collectibles");
    }

    private void OnDisable()
    {
        SaveSystem.Instance.OnLoadProgress -= LoadData;
    }

    public void AddCollectible(int nb)
    {
        _nbCollectibles += nb;
        SaveSystem.Instance.SaveElement<int>("Collectibles", _nbCollectibles);
        OnCollect?.Invoke();
        OnCollectAdd?.Invoke(nb);
        OnCollectAll?.Invoke(_nbCollectibles);
    }
}
