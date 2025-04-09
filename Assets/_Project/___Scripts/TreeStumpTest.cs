using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeStumpTest : MonoBehaviour, ITreeStump, IInteractable
{
    public float OffsetRadius { get ; set; }

    TreeStumpTest()
    {
        OffsetRadius = 0;
    }

    public void Interactable()
    {
        Debug.Log("Je passe en ame, TreeSumpTest.cs");
    }
}
