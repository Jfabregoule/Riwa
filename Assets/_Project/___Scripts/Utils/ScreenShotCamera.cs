using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class ScreenshotFromCamera : MonoBehaviour
{
    public Key ScreenshotKey = Key.Digit5; // This corresponds to '5' key
    public int _resolutionWidth = 1920;
    public int _resolutionHeight = 1080;

    private Camera _cam;

    void Start()
    {
        _cam = GetComponent<Camera>();
        if (_cam == null)
        {
            Debug.LogError("No Camera component found on this GameObject.");
        }
    }

    void Update()
    {
        if (Keyboard.current[ScreenshotKey].wasPressedThisFrame)
        {
            TakeScreenshot();
        }
    }

    void TakeScreenshot()
    {
        RenderTexture rt = new RenderTexture(_resolutionWidth, _resolutionHeight, 24);
        _cam.targetTexture = rt;
        Texture2D screenShot = new Texture2D(_resolutionWidth, _resolutionHeight, TextureFormat.RGB24, false);
        _cam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, _resolutionWidth, _resolutionHeight), 0, 0);
        screenShot.Apply();
        _cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] bytes = screenShot.EncodeToPNG();
        string filename = ScreenshotName();
        File.WriteAllBytes(filename, bytes);

        Debug.Log($"Screenshot taken and saved to: {filename}");
    }

    string ScreenshotName()
    {
        string folderPath = Application.persistentDataPath + "/Screenshots/";
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff");
        return folderPath + $"screenshot_{timestamp}.png";
    }
}
