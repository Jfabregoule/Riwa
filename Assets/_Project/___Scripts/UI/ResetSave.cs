using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetSave : MonoBehaviour
{
    
    public void ResetSaveConfirmation()
    {
        if (DialogueSystem.Instance.IsInDialogue)
        {
            DialogueSystem.Instance.ChangeCanvasGroupAlpha(0);
        }

        if (SceneManager.GetActiveScene().name == "Systems")
        {
            SaveSystem.Instance.DeleteAllData();
        }
        else
        {
            string CurrentRoomSceneName = RiwaLoadSceneSystem.Instance.GetCurrentRoomSceneName();
            StartCoroutine(RiwaLoadSceneSystem.Instance.ChangeScene(new[] { new SceneData(CurrentRoomSceneName) }, new[] { new SceneData("MainMenu", 0, new System.Action[] { SaveSystem.Instance.DeleteAllData, GameManager.Instance.ResetSave }) }));
        }
    }
}
