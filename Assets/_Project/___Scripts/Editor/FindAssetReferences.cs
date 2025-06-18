using UnityEngine;
using UnityEditor;
using System.IO;

public class FindAssetReferences
{
    // Right-click menu item
    [MenuItem("Assets/Find References In Project &r")]
    static void FindReferences()
    {
        Object selected = Selection.activeObject;
        if (selected == null)
        {
            Debug.LogWarning("No asset selected.");
            return;
        }

        string selectedPath = AssetDatabase.GetAssetPath(selected);
        string selectedName = Path.GetFileNameWithoutExtension(selectedPath);
        string[] allPaths = AssetDatabase.GetAllAssetPaths();

        Debug.Log($"Searching references to: {selectedName}");

        foreach (string assetPath in allPaths)
        {
            if (assetPath.EndsWith(".prefab") || assetPath.EndsWith(".mat") || assetPath.EndsWith(".unity") || assetPath.EndsWith(".asset"))
            {
                string content = File.ReadAllText(assetPath);
                if (content.Contains(selectedName))
                {
                    Debug.Log($"Possibly referenced in: {assetPath}", AssetDatabase.LoadMainAssetAtPath(assetPath));
                }
            }
        }

        Debug.Log("Search complete.");
    }
}
