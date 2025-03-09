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
}

