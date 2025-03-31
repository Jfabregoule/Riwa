using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneSystem : Singleton<LoadSceneSystem>
{
    /// <summary>
    /// Charge plusieurs scènes en mode additif.
    /// </summary>
    /// <param name="targetScenes">Tableau des noms des scènes à charger</param>
    public IEnumerator LoadTargetScenes(string[] targetScenes)
    {
        foreach (string scene in targetScenes)
        {
            AsyncOperation sceneOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            while (!sceneOperation.isDone)
            {
                yield return null;
            }
            yield return new WaitUntil(() => sceneOperation.isDone);
        }
    }

    /// <summary>
    /// Décharge plusieurs scènes.
    /// </summary>
    /// <param name="targetScenes">Tableau des noms des scènes à décharger</param>
    public IEnumerator UnloadTargetScenes(string[] targetScenes)
    {
        foreach (string scene in targetScenes)
        {
            AsyncOperation sceneOperation = SceneManager.UnloadSceneAsync(scene);
            while (!sceneOperation.isDone)
            {
                yield return null;
            }
            yield return new WaitUntil(() => sceneOperation.isDone);
        }
    }

    /// <summary>
    /// Charge l'écran de chargement.
    /// </summary>
    public IEnumerator LoadLoadingScreen()
    {
        AsyncOperation sceneOperation = SceneManager.LoadSceneAsync("LoadingScreen", LoadSceneMode.Additive);
        while (!sceneOperation.isDone)
        {
            yield return null;
        }
    }

    /// <summary>
    /// Décharge l'écran de chargement.
    /// </summary>
    public IEnumerator UnloadLoadingScreen()
    {
        AsyncOperation sceneOperation = SceneManager.UnloadSceneAsync("LoadingScreen");
        while (!sceneOperation.isDone)
        {
            yield return null;
        }
    }
}
