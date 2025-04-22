using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor1Room1Dialogue : MonoBehaviour
{

    [SerializeField] private List<DialogueAsset> _listDialogue;

    public void LaunchDialogue(int index)
    {
        DialogueSystem.Instance.BeginDialogue(_listDialogue[index]);
    }

}
