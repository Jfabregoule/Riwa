using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSave : MonoBehaviour
{
    
    public void ResetSaveConfirmation()
    {
        string CurrentRoomSceneName = RiwaLoadSceneSystem.Instance.GetCurrentRoomSceneName();
        StartCoroutine(RiwaLoadSceneSystem.Instance.ChangeScene(new[] { new SceneData(CurrentRoomSceneName) }, new[] { new SceneData("MainMenu", 0, new System.Action[] { SaveSystem.Instance.DeleteProgressData } )})); ;
    }
}
