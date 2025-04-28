using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiwaSaveManagerRoom2 : SaveManager<RiwaSaveManagerRoom2>
{
    [SerializeField] protected string _roomPrefix;
    [SerializeField] private List<Transform> _mirrors;
    [SerializeField] private List<Transform> _pastCrates;
    [SerializeField] private List<Transform> _presentCrates;

    protected override void LoadProgess()
    {
        base.LoadProgess();

        for (int i = 0; i < _pastCrates.Count; i++)
        {
            if (SaveSystem.Instance.ContainsElements(_roomPrefix + $"PastCratePosition{i}"))
                _pastCrates[i].position = SaveSystem.Instance.LoadElement<SerializableVector3>(_roomPrefix + $"PastCratePosition{i}").ToVector3();
        }

        for (int i = 0; i < _presentCrates.Count; i++)
        {
            if (SaveSystem.Instance.ContainsElements(_roomPrefix + $"PresentCratePosition{i}"))
                _presentCrates[i].position = SaveSystem.Instance.LoadElement<SerializableVector3>(_roomPrefix + $"PresentCratePosition{i}").ToVector3();
        }

        for (int i = 0; i < _mirrors.Count; i++)
        {
            if (SaveSystem.Instance.ContainsElements(_roomPrefix + $"MirrorRotation{i}"))
                _mirrors[i].rotation = Quaternion.Euler(SaveSystem.Instance.LoadElement<SerializableVector3>(_roomPrefix + $"MirrorRotation{i}").ToVector3());
        }
    }

    protected override void SaveProgress()
    {
        base.SaveProgress();

        SerializableVector3 pastCratePosition;

        for (int i = 0; i < _pastCrates.Count; i++)
        {
            pastCratePosition = new SerializableVector3(_pastCrates[i].position);
            SaveSystem.Instance.SaveElement<SerializableVector3>(_roomPrefix + $"PastCratePosition{i}", pastCratePosition);
        }

        SerializableVector3 presentCratePosition;

        for (int i = 0; i < _presentCrates.Count; i++)
        {
            presentCratePosition = new SerializableVector3(_presentCrates[i].position);
            SaveSystem.Instance.SaveElement<SerializableVector3>(_roomPrefix + $"PresentCratePosition{i}", presentCratePosition);
        }

        SerializableVector3 mirrorRotation;

        for (int i = 0; i < _mirrors.Count; i++)
        {
            mirrorRotation = new SerializableVector3(_mirrors[i].rotation.eulerAngles);
            SaveSystem.Instance.SaveElement<SerializableVector3>(_roomPrefix + $"MirrorRotation{i}", mirrorRotation);
        }
    }
}
