using UnityEngine;
using UnityEngine.SceneManagement;

public class Navbar : MonoBehaviour
{
    private const string MAIN_MENU_TITLE_TAG = "MainMenuTitle";

    private CanvasGroup _mainMenuTitleCanvasGroup;
    private void Start()
    {
        _mainMenuTitleCanvasGroup = GameObject.FindGameObjectWithTag(MAIN_MENU_TITLE_TAG).GetComponent<CanvasGroup>();
    }
    public void DisableCanvasGroup(CanvasGroup canvasGroup)
    {
        Helpers.DisabledCanvasGroup(canvasGroup);
    }
    public void EnableCanvasGroup(CanvasGroup canvasGroup)
    {
        Helpers.EnabledCanvasGroup(canvasGroup);
    }

    public void DisableMainMenuTitle() {
        if (SceneManager.GetActiveScene().name != "Systems") return;
        Helpers.DisabledCanvasGroup(_mainMenuTitleCanvasGroup);
    }
    public void EnableMainMenuTitle()
    {
        if (SceneManager.GetActiveScene().name != "Systems") return;
        Helpers.EnabledCanvasGroup(_mainMenuTitleCanvasGroup);
    }

    public void DisableCanvasDialog()
    {
        if (!DialogueSystem.Instance.IsInDialogue) return;
        Time.timeScale = 0f;
        DialogueSystem.Instance.ChangeCanvasGroupAlpha(0);
    }

    public void EnableCanvasDialog()
    {
        if (!DialogueSystem.Instance.IsInDialogue) return;
        Time.timeScale = 1.0f;
        DialogueSystem.Instance.ChangeCanvasGroupAlpha(1);

    }

}
