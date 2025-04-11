using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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

    private static void ToggleCanvasGroup(bool isEnable, CanvasGroup canvasGroup)
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
}

