using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
public class SerializableVector3
{
    public float x, y, z;

    public SerializableVector3(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public Vector3 ToVector3() => new Vector3(x, y, z);
}

/// <summary>
/// Entrée de sauvegarde avec une clé et une value.
/// </summary>
[System.Serializable]
public class SavedDataEntry<T>
{
    public string Key; // Clé de valeur à sauvegarder.
    public T Value; // Valeur à sauvegarder.

    public SavedDataEntry(string key, T value)
    {
        Key = key;
        Value = value;
    }
}

[DefaultExecutionOrder(-10)]
public class SaveSystem : Singleton<SaveSystem>
{
    private string _settingsPath; // Chemin du dossier contenant les settings.
    private string _progressPath; // Chemin du dossier contenant la progress.
    public delegate void SaveEvent();
    public SaveEvent OnSaveSettings; // Event de sauvegarde des settings.
    public SaveEvent OnLoadSettings; // Event de load des settings.
    public SaveEvent OnSaveProgress; // Event de save de la progression.
    public SaveEvent OnLoadProgress; // Event de load de la progression.

    private Dictionary<string, object> _settingsData = new Dictionary<string, object>(); // Dictionnaire contenant la sauvegarde actuelle des settings.
    private Dictionary<string, object> _progressData = new Dictionary<string, object>(); // Dictionnaire contenant la sauvegarde actuelle de la progression.

    private Dictionary<string, object> _defaultSettings = new Dictionary<string, object> // Dictionnaire contenant les settings par defaut.
    {
        { "_masterValue", 5f },
        { "_masterTempValue", 5f},
        { "_musicValue", 5f },
        { "_sfxValue", 5f },
        { "_cinematicValue", 5f },
        { "_vibrationIsOn", true },
        { "_audioIsOn", true },
        { "_isRightHanded", true },

    };

    private Dictionary<string, object> _defaultProgress = new Dictionary<string, object> // Dictionnaire contenant la progression par defaut.
    {
        {"FinishPrologue", false },
        {"CurrentFloor", 1},
        {"CurrentRoom", 1},
        {"LastDoorID", 0},
        {"LastDoorDirection", DoorDirection.North},
    };

    /// <summary>
    /// Initialise les chemins des fichiers de sauvegarde.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        _settingsPath = Application.persistentDataPath + "/SettingsData.json";
        _progressPath = Application.persistentDataPath + "/ProgressData.json";
    }

    /// <summary>
    /// Charge toutes les données au démarrage du jeu.
    /// </summary>
    private void Start()
    {
        LoadAllData();
    }

    /// <summary>
    /// Sauvegarde toutes les données lorsque l'application se ferme.
    /// </summary>
    protected override void OnApplicationQuit()
    {
        SaveAllData();
    }

    /// <summary>
    /// Sauvegarde les données lorsque l'application est mise en pause.
    /// </summary>
    /// <param name="pause">Indique si l'application est mise en pause</param>
    private void OnApplicationPause(bool pause)
    {
        if (pause) SaveAllData();
    }

    #region AllData

    /// <summary>
    /// Sauvegarde toutes les données (paramètres et progression).
    /// </summary>
    public void SaveAllData()
    {
        SaveSettingsData();
        SaveProgressData();
    }

    /// <summary>
    /// Charge toutes les données (paramètres et progression).
    /// </summary>
    public void LoadAllData()
    {
        LoadSettingsData();
        LoadProgressData();
    }

    /// <summary>
    /// Supprime toutes les données (paramètres et progression).
    /// </summary>
    public void DeleteAllData()
    {
        DeleteSettingsData();
        DeleteProgressData();
    }

    #endregion

    #region SettingsData

    /// <summary>
    /// Sauvegarde les données de paramètres dans un fichier JSON.
    /// </summary>
    public void SaveSettingsData()
    {
        OnSaveSettings?.Invoke();
        File.WriteAllText(_settingsPath, JsonConvert.SerializeObject(_settingsData, Formatting.Indented));
    }

    /// <summary>
    /// Charge les données de paramètres depuis un fichier JSON.
    /// </summary>
    public void LoadSettingsData()
    {
        if (File.Exists(_settingsPath))
        {
            _settingsData = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(_settingsPath));
        }
        else
        {
            _settingsData = new Dictionary<string, object>(_defaultSettings);
        }

        OnLoadSettings?.Invoke();
    }

    /// <summary>
    /// Supprime les données de paramètres et recharge les valeurs par défaut.
    /// </summary>
    public void DeleteSettingsData()
    {
        if (File.Exists(_settingsPath))
            File.Delete(_settingsPath);
        LoadSettingsData();
    }

    #endregion

    #region ProgressData

    /// <summary>
    /// Sauvegarde les données de progression dans un fichier JSON.
    /// </summary>
    public void SaveProgressData()
    {
        OnSaveProgress?.Invoke();
        File.WriteAllText(_progressPath, JsonConvert.SerializeObject(_progressData, Formatting.Indented));
    }

    /// <summary>
    /// Charge les données de progression depuis un fichier JSON.
    /// </summary>
    public void LoadProgressData()
    {
        if (File.Exists(_progressPath))
        {
            _progressData = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(_progressPath));
        }
        else
        {
            _progressData = new Dictionary<string, object>(_defaultProgress);
        }

        OnLoadProgress?.Invoke();
    }

    /// <summary>
    /// Supprime les données de progression et recharge les valeurs par défaut.
    /// </summary>
    public void DeleteProgressData()
    {
        if (File.Exists(_progressPath))
            File.Delete(_progressPath);
        LoadProgressData();
    }

    #endregion

    #region SingleElement

    /// <summary>
    /// Charge un élément spécifique depuis les paramètres ou la progression.
    /// </summary>
    /// <typeparam name="T">Type de l'élément à charger</typeparam>
    /// <param name="key">Clé de l'élément</param>
    /// <param name="isSetting">Indique si l'élément appartient aux paramètres (true) ou à la progression (false)</param>
    /// <returns>Valeur de l'élément ou valeur par défaut</returns>
    public T LoadElement<T>(string key, bool isSetting = false)
    {
        var data = isSetting ? _settingsData : _progressData;
        var defaults = isSetting ? _defaultSettings : _defaultProgress;

        if (data.TryGetValue(key, out object value))
        {
            if (value is T typedValue)
            {
                return typedValue;
            }

            try
            {
                return JToken.FromObject(value).ToObject<T>();
            }
            catch
            {
                return defaults.ContainsKey(key) ? (T)defaults[key] : default;
            }
        }
        return defaults.ContainsKey(key) ? (T)defaults[key] : default;
    }

    /// <summary>
    /// Sauvegarde un élément spécifique dans les paramètres ou la progression.
    /// </summary>
    /// <typeparam name="T">Type de l'élément à sauvegarder</typeparam>
    /// <param name="key">Clé de l'élément</param>
    /// <param name="value">Valeur de l'élément</param>
    /// <param name="isSetting">Indique si l'élément appartient aux paramètres (true) ou à la progression (false)</param>
    public void SaveElement<T>(string key, T value, bool isSetting = false)
    {
        if (isSetting)
        {
            _settingsData[key] = JToken.FromObject(value);
        }
        else
        {
            _progressData[key] = JToken.FromObject(value);
        }
    }

    public bool ContainsElements(string key, bool isSetting = false)
    {
        var data = isSetting ? _settingsData : _progressData;
        return data.TryGetValue(key, out object value);
    }

    #endregion
}
