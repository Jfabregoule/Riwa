using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Floor1Room3LevelManager : BaseLevelManager
{

    #region Fields

    [Header("Damier")]
    [SerializeField] private CinemachineVirtualCamera _damierCamera;

    [Header("Tutorial Camera")]
    [SerializeField] private List<CinemachineVirtualCamera> _riwaSensaCamera;
    [SerializeField] private List<CinemachineVirtualCamera> _vinesCameras;

    [Header("Chawa")]
    [SerializeField] private GameObject _chawa;

    [Header("Dialogue Manager")]
    [SerializeField] private TutorialRoom3Manager _tutorialRoom3Manager;

    private List<Cell> _brokenCells = new List<Cell>();

    #endregion

    #region Properties

    public List<Cell> BrokenCells { get => _brokenCells; set => _brokenCells = value; }
    public CinemachineVirtualCamera DamierCamera { get => _damierCamera; }
    public List<CinemachineVirtualCamera> RiwaSensaCamera { get => _riwaSensaCamera; }
    public List<CinemachineVirtualCamera> VinesCameras { get => _vinesCameras; }
    public GameObject Chawa { get => _chawa; set => _chawa = value; }
    public TutorialRoom3Manager TutorialRoom3Manager { get => _tutorialRoom3Manager; }

    #endregion

    #region Events

    public delegate void RiwaShowingPath();
    public event RiwaShowingPath OnRiwaShowingPath;

    #endregion

    public void InvokeOnRiwaShowingPath() { OnRiwaShowingPath?.Invoke(); }

}
