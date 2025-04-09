using System.Collections;
using System.Collections.Generic;
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

    public static Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        // Catmull-Rom interpolation
        float t2 = t * t;
        float t3 = t2 * t;

        // Calcul des coefficients
        float f0 = -0.5f * t + t2 - 0.5f * t3;
        float f1 = 1.0f - 2.5f * t2 + 1.5f * t3;
        float f2 = 0.5f * t + 2.0f * t2 - 1.5f * t3;
        float f3 = -0.5f * t2 + 0.5f * t3;

        // Application de l'interpolation
        return f0 * p0 + f1 * p1 + f2 * p2 + f3 * p3;
    }
}

