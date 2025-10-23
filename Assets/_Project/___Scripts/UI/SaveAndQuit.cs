using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveAndQuit : MonoBehaviour
{
    public void QuitGame()
    {
        SaveSystem.Instance.SaveAllData();
        //Application.Quit();

        if (DialogueSystem.Instance.IsInDialogue)
        {
            DialogueSystem.Instance.ChangeCanvasGroupAlpha(0);
            DialogueSystem.Instance.FinishDialogue();
        }

        if (SceneManager.GetActiveScene().name != "Systems")
        {
            InputManager.Instance.EnableGameplayControls();
            GameManager.Instance.UIManager.BlackScreen.ResetCercle();
            string CurrentRoomSceneName = RiwaLoadSceneSystem.Instance.GetCurrentRoomSceneName();
            StartCoroutine(RiwaLoadSceneSystem.Instance.ChangeScene(new[] { new SceneData(CurrentRoomSceneName), }, new[] { new SceneData("MainMenu") }));
        }
    }
}
