using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor1Room2LevelManager : BaseLevelManager
{
    [SerializeField] private CameraCinematicRoom2 _cameraCinematicRoom2;

    public CameraCinematicRoom2 CameraCinematicRoom2 { get => _cameraCinematicRoom2; }
}
