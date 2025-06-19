using UnityEngine;
using System.Collections.Generic;

public class ObstructionGroup : MonoBehaviour
{
    public List<GameObject> objectsToFade = new();

    [Range(0f, 1f)]
    public float _targetObstructedAlpha = 0.15f;
}