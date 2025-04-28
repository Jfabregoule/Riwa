using UnityEngine;
using UnityEngine.SceneManagement;

public class Navbar : MonoBehaviour
{
    private const string MAIN_MENU_TITLE_TAG = "MainMenuTitle";

    public delegate void MainParam();
    public MainParam OnMainParamOpen;
    public MainParam OnMainParamClose;

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

    public void OpenMainParam()
    {
        OnMainParamOpen?.Invoke();
    }
    
    public void CloseMainParam()
    {
        OnMainParamClose?.Invoke();
    }

    public void StartPause()
    {
        Time.timeScale = 0f;
        if (!DialogueSystem.Instance.IsInDialogue) return;
        DialogueSystem.Instance.ChangeCanvasGroupAlpha(0);
    }

    public void EndPause()
    {
        Time.timeScale = 1.0f;
        if (!DialogueSystem.Instance.IsInDialogue) return;
        DialogueSystem.Instance.ChangeCanvasGroupAlpha(1);
    }

}
