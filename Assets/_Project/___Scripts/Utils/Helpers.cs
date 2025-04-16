using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public static class Helpers
{
    private static Camera _camera;
    public static Camera Camera {
        get {
            if(_camera == null)
                _camera = Camera.main;
            return _camera;
        }
    }

    public static Vector2Int Vector2To2Int(Vector2 vec)
    {
        return new Vector2Int((int)vec.x, (int)vec.y);
    }

    public static void ToggleCanvasGroup(bool isEnable, CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = isEnable ? 1 : 0;
        canvasGroup.interactable = isEnable;
        canvasGroup.blocksRaycasts = isEnable;
    }
    public static void EnabledCanvasGroup(CanvasGroup canvasGroup)
    {
        ToggleCanvasGroup(true, canvasGroup);
    }

    public static void DisabledCanvasGroup(CanvasGroup canvasGroup) 
    { 
        ToggleCanvasGroup(false, canvasGroup);
    }

    public static Vector3 GetDominantDirection(Vector3 input)
    {
        Vector3 result = Vector3.zero;

        if (Mathf.Abs(input.x) > Mathf.Abs(input.z))
            result.x = Mathf.Sign(input.x);
        else
            result.z = Mathf.Sign(input.z);

        return result;
    }

    private static Dictionary<float, WaitForSeconds> _waitDictionay = new();

    public static WaitForSeconds GetWait(float time)
    {
        if (_waitDictionay.TryGetValue(time, out WaitForSeconds wait))
            return wait;

        _waitDictionay[time] = new WaitForSeconds(time);
        return _waitDictionay[time];
    }

    
    public static IEnumerator WaitMonoBeheviour<T>(Func<T> script, Action<T> callback)
    {
        float timer = 0;

        T instance = script();

        while (instance == null && timer < 5f)
        {
            timer += Time.deltaTime;
            yield return null;
            instance = script();
        }

        if (instance != null)
        {
            callback?.Invoke(instance);
        }
        else
        {
            Debug.LogWarning($"[WaitAndSubscribe] Timeout: {typeof(T).Name} was still null after {5} seconds.");
        }
    }
}

