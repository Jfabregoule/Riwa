using UnityEngine;

public class TreeStumpTest : MonoBehaviour, ITreeStump, IInteractable
{
    public float OffsetRadius { get ; set; }

    TreeStumpTest()
    {
        OffsetRadius = 0;
    }

    public void Interact()
    {
        Debug.Log("Je passe en ame, TreeSumpTest.cs");
    }
}
