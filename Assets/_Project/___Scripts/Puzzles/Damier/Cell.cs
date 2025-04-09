using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public delegate void CellTrigered(CellPos pos);
    public event CellTrigered OnCellTriggered;

    public CellPos Position { get; private set; }

    public void Init(CellPos pos) {  Position = pos; }

    private void OnTriggerEnter(Collider other) { OnCellTriggered?.Invoke(Position); }
}
