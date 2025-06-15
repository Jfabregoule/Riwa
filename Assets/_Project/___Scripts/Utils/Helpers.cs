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

    public static void ToggleCanvasGroup(bool isEnable, CanvasGroup canvasGroup, bool isInteractable = true)
    {
        canvasGroup.alpha = isEnable ? 1 : 0;
        canvasGroup.interactable = isEnable && isInteractable;
        canvasGroup.blocksRaycasts = isEnable && isInteractable;
    }
    public static void EnabledCanvasGroup(CanvasGroup canvasGroup, bool isInteractable = true)
    {
        ToggleCanvasGroup(true, canvasGroup, isInteractable);
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

    public static bool HasParameter(string paramName, Animator animator)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }
    
    public static Vector3 InterpolationHermite(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return 0.5f * (
            2f * p1 +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t
        );
    }

}

