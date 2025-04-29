using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiwaSaveManagerRoom4 : SaveManager<RiwaSaveManagerRoom4>
{
    [SerializeField] protected string _roomPrefix;
    [SerializeField] private List<Statue> _statues;
    [SerializeField] private Grid _grid;
    [SerializeField] private MovingPlatform _finalMovingPlatform;
    [SerializeField] private ActivableDoor _activableDoor;
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

        if (SaveSystem.Instance.ContainsElements("GridActivated"))
            _grid.IsGridActivated = SaveSystem.Instance.LoadElement<bool>("GridActivated");

        if(_grid.IsGridActivated == true)
        {
            for (int i = 0; i < _statues.Count; i++)
            {
                if (SaveSystem.Instance.ContainsElements(_roomPrefix + $"StatuePosition{i}"))
                    _statues[i].gameObject.transform.position = SaveSystem.Instance.LoadElement<SerializableVector3>(_roomPrefix + $"StatuePosition{i}").ToVector3();
                if (SaveSystem.Instance.ContainsElements(_roomPrefix + $"StatueRotation{i}"))
                    _statues[i].gameObject.transform.rotation = Quaternion.Euler(SaveSystem.Instance.LoadElement<SerializableVector3>(_roomPrefix + $"StatueRotation{i}").ToVector3());
                if (SaveSystem.Instance.ContainsElements(_roomPrefix + $"StatueIsValidate{i}"))
                    _statues[i].Validate = SaveSystem.Instance.LoadElement<bool>(_roomPrefix + $"StatueIsValidate{i}");
                if (_statues[i].Validate == true)
                {
                    _statues[i].OnStatueValidate();
                    _statues[i].StatueAnimator.SetBool("isValidate", true);
                }
            }
            _finalMovingPlatform.StartMoving();
        }

        for (int i = 0; i < _muralPieces.Count; i++)
        {
            if (SaveSystem.Instance.ContainsElements(_roomPrefix + $"MuralPiecePosition{i}"))
                _muralPieces[i].transform.position = SaveSystem.Instance.LoadElement<SerializableVector3>(_roomPrefix + $"MuralPiecePosition{i}").ToVector3();
            if (SaveSystem.Instance.ContainsElements(_roomPrefix + $"MuralPieceRotation{i}"))
                _muralPieces[i].transform.rotation = Quaternion.Euler(SaveSystem.Instance.LoadElement<SerializableVector3>(_roomPrefix + $"MuralPieceRotation{i}").ToVector3());
            if (SaveSystem.Instance.ContainsElements(_roomPrefix + $"MuralPieceState{i}"))
                _muralPieces[i].CanInteract = SaveSystem.Instance.LoadElement<bool>(_roomPrefix + $"MuralPieceState{i}");
            if (SaveSystem.Instance.ContainsElements(_roomPrefix + $"MuralPieceEnum{i}"))
                _muralPieces[i].PieceTemporality = (EnumTemporality)SaveSystem.Instance.LoadElement<int>(_roomPrefix + $"MuralPieceEnum{i}");
            if (SaveSystem.Instance.ContainsElements(_roomPrefix + $"MuralPieceIsPlace{i}"))
                _muralPieces[i].IsPiecePlaced = SaveSystem.Instance.LoadElement<bool>(_roomPrefix + $"MuralPieceIsPlace{i}");
        }

        if (SaveSystem.Instance.ContainsElements("ActivableDoorState"))
            _activableDoor.IsActivated = SaveSystem.Instance.LoadElement<bool>("ActivableDoorState");

        if(_activableDoor.IsActivated == true)
            _activableDoor.OpenDoorOnLoad();

        ((Floor1Room4LevelManager)Floor1Room4LevelManager.Instance).FillMuralPieceDictionary();
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
            if (_statues[i].IsMoving == true || _grid.IsGridActivated == false) return;
            statuePosition = new SerializableVector3(_statues[i].gameObject.transform.position);
            SaveSystem.Instance.SaveElement<SerializableVector3>(_roomPrefix + $"StatuePosition{i}", statuePosition);
            statueRotation = new SerializableVector3(_statues[i].gameObject.transform.rotation.eulerAngles);
            SaveSystem.Instance.SaveElement<SerializableVector3>(_roomPrefix + $"StatueRotation{i}", statueRotation);
            SaveSystem.Instance.SaveElement<bool>(_roomPrefix + $"StatueIsValidate{i}", _statues[i].Validate);
        }

        SerializableVector3 muralPiecePosition;
        SerializableVector3 muralPieceRotation;

        for (int i = 0; i < _muralPieces.Count; i++)
        {
            if ((_muralPieces[i].IsTutorialPiece && _muralPieces[i].GetIsTutorialDone() == false) || _muralPieces[i].IsPiecePlaced == false) return;
            muralPiecePosition = new SerializableVector3(_muralPieces[i].transform.position);
            SaveSystem.Instance.SaveElement<SerializableVector3>(_roomPrefix + $"MuralPiecePosition{i}", muralPiecePosition);
            muralPieceRotation = new SerializableVector3(_muralPieces[i].transform.rotation.eulerAngles);
            SaveSystem.Instance.SaveElement<SerializableVector3>(_roomPrefix + $"MuralPieceRotation{i}", muralPieceRotation);
            SaveSystem.Instance.SaveElement<bool>(_roomPrefix + $"MuralPieceState{i}", _muralPieces[i].CanInteract);
            SaveSystem.Instance.SaveElement<bool>(_roomPrefix + $"MuralPieceIsPlace{i}", _muralPieces[i].IsPiecePlaced);
            SaveSystem.Instance.SaveElement<int>(_roomPrefix + $"MuralPieceEnum{i}", (int)_muralPieces[i].PieceTemporality);
        }

        //SUIcide toi feu feu
    }
}
