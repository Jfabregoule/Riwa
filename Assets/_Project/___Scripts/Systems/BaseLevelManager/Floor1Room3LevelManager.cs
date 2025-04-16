using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor1Room3LevelManager : BaseLevelManager
{
    private List<Cell> _brokenCells = new List<Cell>();

    public List<Cell> BrokenCells { get => _brokenCells; set => _brokenCells = value; }

}
