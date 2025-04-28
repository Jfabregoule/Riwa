using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class SaveManager<T> : Singleton<T> where T : SaveManager<T>
{

    private void Start()
    {
        LoadProgess();
    }

    private void OnApplicationQuit()
    {
        SaveProgress();
    }

    private void OnApplicationPause(bool pause)
    {
        SaveProgress();
    }

    private void OnEnable()
    {
        SaveSystem.Instance.OnLoadProgress += LoadProgess;
        SaveSystem.Instance.OnSaveProgress += SaveProgress;
    }

    private void OnDisable()
    {
        SaveSystem.Instance.OnLoadProgress -= LoadProgess;
        SaveSystem.Instance.OnSaveProgress -= SaveProgress;
    }

    protected virtual void LoadProgess()
    {
    }

    protected virtual void SaveProgress()
    {
    }
}
