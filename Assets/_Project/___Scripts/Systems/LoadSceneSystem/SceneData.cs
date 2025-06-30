using System;

/// <summary>
/// Représente les données associées à une scène à charger ou décharger.
/// Contient le nom de la scène, sa priorité dans la file de chargement, et des callbacks à exécuter.
/// </summary>
[Serializable]
public class SceneData
{
    /// <summary>
    /// Nom exact de la scène tel qu’indiqué dans les Build Settings de Unity.
    /// </summary>
    public string Name;

    /// <summary>
    /// Priorité de chargement : plus la valeur est basse, plus la scène sera chargée tôt.
    /// </summary>
    public int Priority = 0;

    /// <summary>
    /// Callback exécuté lorsque la scène a été entièrement chargée.
    /// </summary>
    public Action[] OnLoaded;

    /// <summary>
    /// Callback exécuté lorsque la scène a été entièrement déchargée.
    /// </summary>
    public Action[] OnUnloaded;

    /// <summary>
    /// Constructeur complet de la scène.
    /// </summary>
    /// <param name="name">Nom de la scène</param>
    /// <param name="priority">Priorité dans la file de chargement</param>
    /// <param name="onLoaded">Action à exécuter après chargement</param>
    /// <param name="onUnloaded">Action à exécuter après déchargement</param>
    public SceneData(string name, int priority = 0, Action[] onLoaded = null, Action[] onUnloaded = null)
    {
        Name = name;
        Priority = priority;
        OnLoaded = onLoaded;
        OnUnloaded = onUnloaded;
    }
}
