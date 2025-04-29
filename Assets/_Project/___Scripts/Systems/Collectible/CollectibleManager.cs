using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    private int _nbCollectibles;

    public delegate void CollectibleEventShow();
    public delegate void CollectibleEvent(int nb);
    public event CollectibleEventShow OnCollect;
    public event CollectibleEvent OnCollectAdd;
    public event CollectibleEvent OnCollectAll;

    void Start()
    {
        _nbCollectibles = 0;
    }

    public void AddCollectible(int nb)
    {
        _nbCollectibles += nb;
        OnCollect?.Invoke();
        OnCollectAdd?.Invoke(nb);
        OnCollectAll?.Invoke(_nbCollectibles);
    }
}
