/// <summary>
/// Interface pour cr�er des �crans de chargement personnalis�s.
/// Permet d�afficher, masquer et mettre � jour la progression visuelle du chargement.
/// </summary>
public interface ILoadingScreen
{
    /// <summary>
    /// Affiche l��cran de chargement.
    /// </summary>
    void Show();

    /// <summary>
    /// Masque l��cran de chargement.
    /// </summary>
    void Hide();

    /// <summary>
    /// Met � jour la valeur de progression sur l��cran de chargement (entre 0 et 1).
    /// </summary>
    /// <param name="value">Progression actuelle du chargement</param>
    void SetProgress(float value);
}
