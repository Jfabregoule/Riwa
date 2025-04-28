using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiwaSaveManagerRoom4 : SaveManager<RiwaSaveManagerRoom4>
{
    [SerializeField] protected string _roomPrefix;
    [SerializeField] private List<Transform> _statues;
    [SerializeField] private List<Transform> _pastCrates;
    [SerializeField] private List<Transform> _presentCrates;
    [SerializeField] private List<MuralPiece> _muralPieces;

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
            if (SaveSystem.Instance.ContainsElements(_roomPrefix + $"PastCratePosition{i}"))
                _presentCrates[i].position = SaveSystem.Instance.LoadElement<SerializableVector3>(_roomPrefix + $"PresentCratePosition{i}").ToVector3();
        }

        for (int i = 0; i < _statues.Count; i++)
        {
            if (SaveSystem.Instance.ContainsElements(_roomPrefix + $"StatuePosition{i}"))
                _statues[i].position = SaveSystem.Instance.LoadElement<SerializableVector3>(_roomPrefix + $"StatuePosition{i}").ToVector3();
            if (SaveSystem.Instance.ContainsElements(_roomPrefix + $"StatueRotation{i}"))
                _statues[i].rotation = Quaternion.Euler(SaveSystem.Instance.LoadElement<SerializableVector3>(_roomPrefix + $"StatueRotation{i}").ToVector3());
        }

        for (int i = 0; i < _muralPieces.Count; i++)
        {
            if (SaveSystem.Instance.ContainsElements(_roomPrefix + $"MuralPiecePosition{i}"))
                _muralPieces[i].transform.position = SaveSystem.Instance.LoadElement<SerializableVector3>(_roomPrefix + $"MuralPiecePosition{i}").ToVector3();
            if (SaveSystem.Instance.ContainsElements(_roomPrefix + $"MuralPieceRotation{i}"))
                _muralPieces[i].transform.rotation = Quaternion.Euler(SaveSystem.Instance.LoadElement<SerializableVector3>(_roomPrefix + $"MuralPieceRotation{i}").ToVector3());
            if (SaveSystem.Instance.ContainsElements(_roomPrefix + $"MuralPieceState{i}"))
                _muralPieces[i].CanInteract = SaveSystem.Instance.LoadElement<bool>(_roomPrefix + $"MuralPieceState{i}");
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

        for (int i = 0; i < _pastCrates.Count; i++)
        {
            presentCratePosition = new SerializableVector3(_presentCrates[i].position);
            SaveSystem.Instance.SaveElement<SerializableVector3>(_roomPrefix + $"PresentCratePosition{i}", presentCratePosition);
        }

        SerializableVector3 statuePosition;
        SerializableVector3 statueRotation;

        for (int i = 0; i < _statues.Count; i++)
        {
            statuePosition = new SerializableVector3(_statues[i].position);
            SaveSystem.Instance.SaveElement<SerializableVector3>(_roomPrefix + $"StatuePosition{i}", statuePosition);
            statueRotation = new SerializableVector3(_statues[i].rotation.eulerAngles);
            SaveSystem.Instance.SaveElement<SerializableVector3>(_roomPrefix + $"StatueRotation{i}", statueRotation);
        }

        SerializableVector3 muralPiecePosition;
        SerializableVector3 muralPieceRotation;

        for (int i = 0; i < _muralPieces.Count; i++)
        {
            muralPiecePosition = new SerializableVector3(_muralPieces[i].transform.position);
            SaveSystem.Instance.SaveElement<SerializableVector3>(_roomPrefix + $"MuralPiecePosition{i}", muralPiecePosition);
            muralPieceRotation = new SerializableVector3(_muralPieces[i].transform.rotation.eulerAngles);
            SaveSystem.Instance.SaveElement<SerializableVector3>(_roomPrefix + $"MuralPieceRotation{i}", muralPieceRotation);
            SaveSystem.Instance.SaveElement<bool>(_roomPrefix + $"MuralPieceState{i}", _muralPieces[i].CanInteract);
        }
    }
}
