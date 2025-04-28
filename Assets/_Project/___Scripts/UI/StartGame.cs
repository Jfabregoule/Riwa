using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{

    private CanvasGroup _mainMenuTitleCanvasGroup;
    private void Start()
    {
        StartCoroutine(Helpers.WaitMonoBeheviour(() => GameManager.Instance.Navbar, SubscribeToNavbar));
        _mainMenuTitleCanvasGroup = transform.parent.GetComponent<CanvasGroup>();
    }

    private void SubscribeToNavbar(Navbar navbar)
    {
        if (navbar != null) {
            navbar.OnMainParamOpen += DisableMainMenuTitle;
            navbar.OnMainParamClose += EnableMainMenuTitle;
        }
    }

    public void LoadGameScene()
    {
        RiwaLoadSceneSystem.Instance.LoadFirstScene();
        GetComponent<Button>().interactable = false;
    }

    public void DisableMainMenuTitle()
    {
        if (SceneManager.GetActiveScene().name != "Systems") return;
        Helpers.DisabledCanvasGroup(_mainMenuTitleCanvasGroup);
    }
    public void EnableMainMenuTitle()
    {
        if (SceneManager.GetActiveScene().name != "Systems") return;
        Helpers.EnabledCanvasGroup(_mainMenuTitleCanvasGroup);
    }
}
