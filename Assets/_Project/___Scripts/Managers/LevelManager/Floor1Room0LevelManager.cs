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

    [SerializeField] private CinemachineVirtualCamera _cameraCollectible;

    [Header("Cinematic")]
    [SerializeField] private CinematicRoom0Manager _cinematicManager;

    private bool _isCinematicDone = false;

    public GameObject Riwa { get => _riwa; }
    public GameObject Chawa { get => _chawa; }
    public Transform SensaLandPos { get => _sensaLandPos; }
    public CinemachineVirtualCamera RiwaSensaCamera { get => _cameraRiwaSensa; }
    public CinemachineVirtualCamera CollectibleCamera { get => _cameraCollectible; }
    public bool IsCinematicDone { get => _isCinematicDone; set =>  _isCinematicDone = value; }
    public CinematicRoom0Manager CinematicManager { get => _cinematicManager; }

    public override void OnEnable()
    {
        base.OnEnable();
        LoadData();
        SaveSystem.Instance.OnLoadProgress += LoadData;
    }

    private void OnDisable()
    {
        SaveSystem.Instance.OnLoadProgress -= LoadData;
    }

    public override void Start()
    {
        base.Start();
        
    }

    private void LoadData()
    {
        if((EnumAdvancementRoom1)SaveSystem.Instance.LoadElement<int>("Room1Progress") >= EnumAdvancementRoom1.Room0)
        {
            GameManager.Instance.UnlockChangeTime();
        }
    }

}
