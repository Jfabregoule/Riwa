using UnityEditor;
using UnityEngine;

public class LightProbeDataBaker : MonoBehaviour
{
    [MenuItem("Tools/Bake/Save Current Light Probes to Asset")]
    public static void SaveLightProbes()
    {
        var lightProbes = LightmapSettings.lightProbes;

        if (lightProbes == null || lightProbes.bakedProbes == null || lightProbes.count == 0)
        {
            Debug.LogWarning("No light probes found in the current scene.");
            return;
        }

        LightProbeData data = ScriptableObject.CreateInstance<LightProbeData>();
        data.bakedProbes = lightProbes.bakedProbes;

        string path = EditorUtility.SaveFilePanelInProject(
            "Save Light Probe Data",
            "LightProbeData",
            "asset",
            "Choose a location to save the Light Probe Data"
        );

        if (!string.IsNullOrEmpty(path))
        {
            AssetDatabase.CreateAsset(data, path);
            AssetDatabase.SaveAssets();
            Debug.Log("Light Probe data saved to: " + path);
        }
    }
}