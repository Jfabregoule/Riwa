using UnityEngine;

public class PlayFromSave : MonoBehaviour
{
    [SerializeField] private string _pathName;

    private string _path;
    void Start()
    {
        _path = Application.dataPath + "/_Project/Resources/Saves/" + _pathName + ".json";
    }

    public void PlayFromSavePath()
    {
        SaveSystem.Instance.LoadProgressData(_path);
    }
}
