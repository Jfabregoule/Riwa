using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Floor1Room3LevelManager : BaseLevelManager
{
    private List<Cell> _brokenCells = new List<Cell>();

    [Header("Damier")]
    [SerializeField] private CinemachineVirtualCamera _damierCamera;

    public List<Cell> BrokenCells { get => _brokenCells; set => _brokenCells = value; }
    public CinemachineVirtualCamera DamierCamera { get => _damierCamera; }

}
