/// <summary>
/// Interface pour créer des écrans de chargement personnalisés.
/// Permet d’afficher, masquer et mettre à jour la progression visuelle du chargement.
/// </summary>
public interface ILoadingScreen
{
    /// <summary>
    /// Affiche l’écran de chargement.
    /// </summary>
    void Show();

    /// <summary>
    /// Masque l’écran de chargement.
    /// </summary>
    void Hide();

    /// <summary>
    /// Met à jour la valeur de progression sur l’écran de chargement (entre 0 et 1).
    /// </summary>
    /// <param name="value">Progression actuelle du chargement</param>
    void SetProgress(float value);
}
