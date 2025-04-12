using UnityEngine;

public class Cell : MonoBehaviour
{

    private LayerMask whatIsPlayer;

    public delegate void CellTrigered(CellPos pos, Cell cell);
    public event CellTrigered OnCellTriggered;

    public CellPos Position { get; private set; }

    public void Init(CellPos pos) {  Position = pos; }

    private void Awake()
    {
        whatIsPlayer = LayerMask.GetMask("whatIsPlayer");
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (((1 << other.gameObject.layer) & whatIsPlayer) != 0)
            OnCellTriggered?.Invoke(Position, this); 
    }
}
