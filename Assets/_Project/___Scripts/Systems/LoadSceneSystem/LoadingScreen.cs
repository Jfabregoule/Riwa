using UnityEngine;

public class LoadingScreen : MonoBehaviour, ILoadingScreen
{
    public CanvasGroup canvas;
    public UnityEngine.UI.Slider progressBar;

    public void Show() => canvas.alpha = 1;
    public void Hide() => canvas.alpha = 0;
    public void SetProgress(float value) => progressBar.value = value;
}