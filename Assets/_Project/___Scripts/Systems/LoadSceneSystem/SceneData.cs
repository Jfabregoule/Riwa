using System;

/// <summary>
/// Repr�sente les donn�es associ�es � une sc�ne � charger ou d�charger.
/// Contient le nom de la sc�ne, sa priorit� dans la file de chargement, et des callbacks � ex�cuter.
/// </summary>
[Serializable]
public class SceneData
{
    /// <summary>
    /// Nom exact de la sc�ne tel qu�indiqu� dans les Build Settings de Unity.
    /// </summary>
    public string Name;

    /// <summary>
    /// Priorit� de chargement : plus la valeur est basse, plus la sc�ne sera charg�e t�t.
    /// </summary>
    public int Priority = 0;

    /// <summary>
    /// Callback ex�cut� lorsque la sc�ne a �t� enti�rement charg�e.
    /// </summary>
    public Action[] OnLoaded;

    /// <summary>
    /// Callback ex�cut� lorsque la sc�ne a �t� enti�rement d�charg�e.
    /// </summary>
    public Action[] OnUnloaded;

    /// <summary>
    /// Constructeur complet de la sc�ne.
    /// </summary>
    /// <param name="name">Nom de la sc�ne</param>
    /// <param name="priority">Priorit� dans la file de chargement</param>
    /// <param name="onLoaded">Action � ex�cuter apr�s chargement</param>
    /// <param name="onUnloaded">Action � ex�cuter apr�s d�chargement</param>
    public SceneData(string name, int priority = 0, Action[] onLoaded = null, Action[] onUnloaded = null)
    {
        Name = name;
        Priority = priority;
        OnLoaded = onLoaded;
        OnUnloaded = onUnloaded;
    }
}
