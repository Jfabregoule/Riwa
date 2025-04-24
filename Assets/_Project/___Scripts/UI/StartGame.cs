using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public void LoadGameScene()
    {
        RiwaLoadSceneSystem.Instance.LoadFirstScene();
        GetComponent<Button>().interactable = false;
    }
}
