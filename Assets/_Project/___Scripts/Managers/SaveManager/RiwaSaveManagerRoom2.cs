using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiwaSaveManagerRoom2 : SaveManager<RiwaSaveManagerRoom2>
{
    [SerializeField] private string _roomPrefix;
    [SerializeField] private List<Transform> _mirrors;
    [SerializeField] private Transform _pastCrate;
    [SerializeField] private Transform _presentCrate;

    protected override void LoadProgess()
    {
        base.LoadProgess();

        if (SaveSystem.Instance.ContainsElements(_roomPrefix + "PastCratePosition"))
            _pastCrate.position = SaveSystem.Instance.LoadElement<SerializableVector3>(_roomPrefix + "PastCratePosition").ToVector3();
        if (SaveSystem.Instance.ContainsElements(_roomPrefix + "PastCrateRotation"))
            _pastCrate.rotation = Quaternion.Euler(SaveSystem.Instance.LoadElement<SerializableVector3>(_roomPrefix + "PastCrateRotation").ToVector3());

        if (SaveSystem.Instance.ContainsElements(_roomPrefix + "PresentCratePosition"))
            _presentCrate.position = SaveSystem.Instance.LoadElement<SerializableVector3>(_roomPrefix + "PastCratePosition").ToVector3();
        if (SaveSystem.Instance.ContainsElements(_roomPrefix + "PresentCrateRotation"))
            _presentCrate.rotation = Quaternion.Euler(SaveSystem.Instance.LoadElement<SerializableVector3>(_roomPrefix + "PastCrateRotation").ToVector3());

        for (int i = 0; i < _mirrors.Count; i++)
        {
            if (SaveSystem.Instance.ContainsElements(_roomPrefix + $"MirrorRotation{i}"))
                _mirrors[i].rotation = Quaternion.Euler(SaveSystem.Instance.LoadElement<SerializableVector3>(_roomPrefix + $"MirrorRotation{i}").ToVector3());
        }
    }

    protected override void SaveProgress()
    {
        base.SaveProgress();

        SerializableVector3 pastCratePosition = new SerializableVector3(_pastCrate.position);
        SerializableVector3 pastCrateRotation = new SerializableVector3(_pastCrate.rotation.eulerAngles);
        SaveSystem.Instance.SaveElement<SerializableVector3>(_roomPrefix + "PastCratePosition", pastCratePosition);
        SaveSystem.Instance.SaveElement<SerializableVector3>(_roomPrefix + "PastCrateRotation", pastCrateRotation);

        SerializableVector3 presentCratePosition = new SerializableVector3(_presentCrate.position);
        SerializableVector3 presentCrateRotation = new SerializableVector3(_presentCrate.rotation.eulerAngles);
        SaveSystem.Instance.SaveElement<SerializableVector3>(_roomPrefix + "PresentCratePosition", presentCratePosition);
        SaveSystem.Instance.SaveElement<SerializableVector3>(_roomPrefix + "PresentCrateRotation", presentCrateRotation);

        for (int i = 0; i < _mirrors.Count; i++)
        {
            SerializableVector3 mirrorRotation = new SerializableVector3(_mirrors[i].rotation.eulerAngles);
            SaveSystem.Instance.SaveElement<SerializableVector3>(_roomPrefix + $"MirrorRotation{i}", mirrorRotation);
        }
    }
}
