using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndQuit : MonoBehaviour
{
    public void QuitGame()
    {
        SaveSystem.Instance.SaveAllData();
        Application.Quit();
    }
}
