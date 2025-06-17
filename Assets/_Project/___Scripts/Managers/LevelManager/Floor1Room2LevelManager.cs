using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor1Room2LevelManager : BaseLevelManager
{
    [SerializeField] private CameraCinematicRoom2 _cameraCinematicRoom2;
    [SerializeField] private RiwaFloor1Room2 _riwaFloor1Room2;

    [SerializeField] private BridgeVineScript _bridgeVineScript;

    public CameraCinematicRoom2 CameraCinematicRoom2 { get => _cameraCinematicRoom2; }
    public RiwaFloor1Room2 RiwaFloor1Room2 { get => _riwaFloor1Room2; }
    public BridgeVineScript BridgeVineScript { get => _bridgeVineScript; set => _bridgeVineScript = value; }

    public override void Start()
    {
        base.Start();
        GameManager.Instance.UnlockChangeTime();

        BridgeVineScript.CanInteract = false;
    }


}
