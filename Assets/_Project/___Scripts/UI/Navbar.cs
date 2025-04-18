using UnityEngine;

public class Navbar : MonoBehaviour
{
    private InputManager _inputManager;

    //void OnEnable()
    //{
    //    StartCoroutine(Helpers.WaitMonoBeheviour(() => InputManager.Instance, SubscribeToInputManager));
    //}

    //void OnDisable()
    //{
    //    if (_inputManager != null)
    //    {
    //        script.OnOpenOptions -= EnableCanvasGroup;
    //    }
    //}

    public void DisableCanvasGroup(CanvasGroup canvasGroup)
    {
        Helpers.DisabledCanvasGroup(canvasGroup);
    }
    public void EnableCanvasGroup(CanvasGroup canvasGroup)
    {
        Helpers.EnabledCanvasGroup(canvasGroup);
    }

    //private void SubscribeToInputManager(InputManager script)
    //{
    //    if (script != null)
    //    {
    //        _inputManager = script;
    //        script.OnOpenOptions += EnableCanvasGroup;
    //    }
    //}
}
