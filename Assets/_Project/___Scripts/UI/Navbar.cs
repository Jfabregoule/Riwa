using UnityEngine;
using UnityEngine.SceneManagement;

public class Navbar : MonoBehaviour
{
    private const string MAIN_MENU_TITLE_TAG = "MainMenuTitle";

    public delegate void MainParam();
    public MainParam OnMainParamOpen;
    public MainParam OnMainParamClose;

    private OpenOption _openOption;
    private CanvasGroup _mainMenuTitleCanvasGroup;
    private void Start()
    {
        SaveSystem.Instance.OnLoadProgress += DisableDialogue;

        _mainMenuTitleCanvasGroup = GameObject.FindGameObjectWithTag(MAIN_MENU_TITLE_TAG).GetComponent<CanvasGroup>();
        
        _openOption = transform.GetChild(0).transform.GetChild(0).GetComponent<OpenOption>();
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

    public void CloseOption()
    {
        _openOption.CloseOptions();
    }

    public void DisableDialogue()
    {
        DialogueSystem.Instance.ChangeCanvasGroupAlpha(0);
    }
}
