using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Syst�me de chargement de sc�nes avec file d'attente, gestion de la priorit�, progression, et �cran de chargement optionnel.
/// </summary>
public class LoadSceneSystem<T> : Singleton<T> where T : LoadSceneSystem<T>
{
    public event Action<string> OnSceneLoaded;
    public event Action<string> OnSceneUnloaded;
    public event Action<float> OnLoadingProgress;

    private Queue<SceneData> _loadQueue = new Queue<SceneData>();
    private bool _isLoading = false;

    private ILoadingScreen _loadingScreen;

    #region Public API

    /// <summary>
    /// Ajoute plusieurs sc�nes � charger � la file, tri�es par priorit�.
    /// D�marre le chargement si aucun n�est en cours.
    /// </summary>
    /// <param name="scenes">Les sc�nes � charger</param>
    /// <param name="useLoadingScreen">Utiliser la sc�ne de chargement ?</param>
    public void EnqueueScenes(IEnumerable<SceneData> scenes, bool useLoadingScreen = true)
    {
        foreach (var scene in scenes.OrderBy(s => s.Priority))
        {
            if (!IsSceneAlreadyLoaded(scene.Name))
                _loadQueue.Enqueue(scene);
            else
                Debug.LogWarning($"Scene '{scene.Name}' is already loaded.");
        }

        if (!_isLoading)
        {
            if (useLoadingScreen)
                StartCoroutine(ProcessQueueWithLoadingScreen());
            else
                StartCoroutine(ProcessQueue());
        }
    }

    /// <summary>
    /// Ajoute une seule sc�ne � la file de chargement.
    /// </summary>
    /// <param name="scene">La sc�ne � charger</param>
    /// <param name="useLoadingScreen">Utiliser la sc�ne de chargement ?</param>
    public void EnqueueScene(SceneData scene, bool useLoadingScreen = true)
    {
        EnqueueScenes(new[] { scene }, useLoadingScreen);
    }

    /// <summary>
    /// V�rifie si toutes les sc�nes donn�es peuvent �tre charg�es.
    /// </summary>
    public bool ValidateScenes(IEnumerable<SceneData> scenes)
    {
        return scenes.All(s => Application.CanStreamedLevelBeLoaded(s.Name));
    }

    /// <summary>
    /// D�charge les sc�nes sp�cifi�es de mani�re asynchrone.
    /// </summary>
    public IEnumerator UnloadScenes(IEnumerable<SceneData> scenes)
    {
        foreach (var scene in scenes)
        {
            if (!SceneManager.GetSceneByName(scene.Name).isLoaded)
                continue;

            AsyncOperation op = SceneManager.UnloadSceneAsync(scene.Name);
            yield return new WaitUntil(() => op.isDone);

            scene.OnUnloaded?.Invoke();
            OnSceneUnloaded?.Invoke(scene.Name);
        }

        Resources.UnloadUnusedAssets();
        GC.Collect();
    }

    /// <summary>
    /// D�charge la sc�ne sp�cifi�e de mani�re asynchrone.
    /// </summary>
    public IEnumerator UnloadScene(SceneData scene)
    {
        if (!SceneManager.GetSceneByName(scene.Name).isLoaded) yield return null;

        AsyncOperation op = SceneManager.UnloadSceneAsync(scene.Name);
        yield return new WaitUntil(() => op.isDone);

        scene.OnUnloaded?.Invoke();
        OnSceneUnloaded?.Invoke(scene.Name);

        Resources.UnloadUnusedAssets();
        GC.Collect();
    }

    /// <summary>
    /// Change la sc�ne en d�chargeant les sc�nes donn�es, en chargeant les nouvelles sc�nes, et en affichant l'�cran de chargement.
    /// </summary>
    /// <param name="scenesToUnload">Les sc�nes � d�charger</param>
    /// <param name="scenesToLoad">Les sc�nes � charger</param>
    public IEnumerator ChangeScene(IEnumerable<SceneData> scenesToUnload, IEnumerable<SceneData> scenesToLoad)
    {
        // D�marrer l'�cran de chargement
        yield return StartCoroutine(ChangeSceneCoroutine(scenesToUnload, scenesToLoad));
    }

    #endregion

    #region Internals

    /// <summary>
    /// Charge la sc�ne du LoadingScreen, puis traite la file de chargement, et la d�charge ensuite.
    /// </summary>
    private IEnumerator ProcessQueueWithLoadingScreen()
    {
        _isLoading = true;

        yield return SceneManager.LoadSceneAsync("LoadingScreen", LoadSceneMode.Additive);

        _loadingScreen = GameObject.FindGameObjectWithTag("LoadingScreen")?.GetComponent<ILoadingScreen>();

        if (_loadingScreen == null)
            Debug.LogWarning("LoadingScreen GameObject not trouv� dans la sc�ne 'LoadingScreen'.");

        yield return StartCoroutine(ProcessQueue());

        yield return SceneManager.UnloadSceneAsync("LoadingScreen");

        _loadingScreen = null;
        _isLoading = false;
    }

    /// <summary>
    /// Traite la file de sc�nes � charger avec suivi de progression.
    /// </summary>
    private IEnumerator ProcessQueue()
    {
        _isLoading = true;

        List<AsyncOperation> operations = new List<AsyncOperation>();
        List<SceneData> currentBatch = new List<SceneData>();

        while (_loadQueue.Count > 0)
        {
            var scene = _loadQueue.Dequeue();

            if (!Application.CanStreamedLevelBeLoaded(scene.Name))
            {
                Debug.LogWarning($"Scene '{scene.Name}' not found in Build Settings.");
                continue;
            }

            var op = SceneManager.LoadSceneAsync(scene.Name, LoadSceneMode.Additive);
            op.allowSceneActivation = false;

            operations.Add(op);
            currentBatch.Add(scene);
        }

        _loadingScreen?.Show();

        while (operations.Count > 0)
        {
            float total = operations.Sum(o => Mathf.Clamp01(o.progress / 0.9f));
            float progress = total / operations.Count;

            OnLoadingProgress?.Invoke(progress);
            _loadingScreen?.SetProgress(progress);

            if (progress >= 1f)
            {
                foreach (var op in operations)
                    op.allowSceneActivation = true;

                break;
            }

            yield return null;
        }

        for (int i = 0; i < operations.Count; i++)
        {
            yield return new WaitUntil(() => operations[i].isDone);

            var scene = currentBatch[i];
            scene.OnLoaded?.Invoke();
            OnSceneLoaded?.Invoke(scene.Name);
        }

        _loadingScreen?.Hide();

        Resources.UnloadUnusedAssets();
        GC.Collect();

        _isLoading = false;
    }

    /// <summary>
    /// V�rifie si une sc�ne est d�j� charg�e.
    /// </summary>
    private bool IsSceneAlreadyLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == sceneName)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Coroutine qui g�re le changement de sc�ne : d�chargement, chargement et �cran de chargement.
    /// </summary>
    private IEnumerator ChangeSceneCoroutine(IEnumerable<SceneData> scenesToUnload, IEnumerable<SceneData> scenesToLoad)
    {

        yield return SceneManager.LoadSceneAsync("LoadingScreen", LoadSceneMode.Additive);
        _loadingScreen = GameObject.FindGameObjectWithTag("LoadingScreen")?.GetComponent<ILoadingScreen>();

        if (_loadingScreen == null)
            Debug.LogWarning("LoadingScreen GameObject not trouv� dans la sc�ne 'LoadingScreen'.");

        _loadingScreen?.Show();

        yield return StartCoroutine(UnloadScenes(scenesToUnload));

        EnqueueScenes(scenesToLoad, useLoadingScreen: false);

        while (_isLoading)
        {
            yield return null;
        }

        _loadingScreen?.Hide();

        yield return SceneManager.UnloadSceneAsync("LoadingScreen");

        Resources.UnloadUnusedAssets();
        GC.Collect();
    }

    #endregion
}
