using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class AutoSceneLoader
{
    private static string systemScenePath = "Assets/_Project/__Scenes/Global/Systems.unity";
    private static string hudScenePath = "Assets/_Project/__Scenes/UI/HUD.unity";

    private static bool scenesLoaded = false;

    static AutoSceneLoader()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode && !scenesLoaded)
        {
            string currentScenePath = SceneManager.GetActiveScene().path;

            if (currentScenePath == systemScenePath || currentScenePath == hudScenePath)
                return;

            scenesLoaded = true;

            EditorApplication.delayCall += () =>
            {
                EditorSceneManager.OpenScene(systemScenePath, OpenSceneMode.Single);
                EditorSceneManager.OpenScene(currentScenePath, OpenSceneMode.Additive);
                EditorSceneManager.OpenScene(hudScenePath, OpenSceneMode.Additive);

                EditorApplication.isPlaying = true;
            };

            EditorApplication.isPlaying = false;
        }
        else if (state == PlayModeStateChange.EnteredEditMode)
        {
            scenesLoaded = false;
        }
    }
}
