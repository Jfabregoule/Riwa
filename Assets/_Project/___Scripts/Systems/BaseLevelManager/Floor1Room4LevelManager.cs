using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Floor1Room4LevelManager : BaseLevelManager
{
    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera _stairCamera;

    public CinemachineVirtualCamera StairCamera { get => _stairCamera; }
}
