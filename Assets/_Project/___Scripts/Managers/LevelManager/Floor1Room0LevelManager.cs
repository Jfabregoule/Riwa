using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Floor1Room0LevelManager : BaseLevelManager
{

    [Header("GameObjects")]
    [SerializeField] private GameObject _riwa;
    [SerializeField] private GameObject _chawa;

    [Header("Transforms")]
    [SerializeField] private Transform _sensaLandPos;

    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera _cameraRiwaSensa;

    private bool _isCinematicDone = false;

    public GameObject Riwa { get => _riwa; }
    public GameObject Chawa { get => _chawa; }
    public Transform SensaLandPos { get => _sensaLandPos; }
    public CinemachineVirtualCamera RiwaSensaCamera { get => _cameraRiwaSensa; }
    public bool IsCinematicDone { get => _isCinematicDone; set =>  _isCinematicDone = value; }

}
