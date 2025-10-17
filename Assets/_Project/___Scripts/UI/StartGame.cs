using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{

    [SerializeField] private CanvasGroup _mainMenuTitleCanvasGroup;
    [SerializeField] private Button _button;
    private Navbar _navbar;


    private void Awake()
    {
        if (SceneManager.GetActiveScene().name != "Systems") return;

        if (_mainMenuTitleCanvasGroup == null)
        {
            _mainMenuTitleCanvasGroup = GameObject.Find("Title")?.GetComponent<CanvasGroup>();
        }
        if (_button == null)
            _button = GameObject.Find("Button")?.GetComponent<Button>();
    }

    private void OnDisable()
    {
        if (_navbar != null)
        {
            _navbar.OnMainParamOpen -= DisableMainMenuTitle;
            _navbar.OnMainParamClose -= EnableMainMenuTitle;
        }
    }
    private void Start()
    {
        StartCoroutine(Helpers.WaitMonoBeheviour(() => GameManager.Instance.UIManager, SubscribeToNavbar));
    }

    private void SubscribeToNavbar(UIManager manager)
    {
        if (manager != null) {
            _navbar = manager.Navbar;
            _navbar.OnMainParamOpen += DisableMainMenuTitle;
            _navbar.OnMainParamClose += EnableMainMenuTitle;
        }
    }

    public void LoadGameScene()
    {
        RiwaSoundMixerManager.Instance.BlendToTemporality(EnumTemporality.Present);
        _navbar.CloseOption();
        RiwaLoadSceneSystem.Instance.LoadFirstScene();
        _button.interactable = false;
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
