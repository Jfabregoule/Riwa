using System.Collections;
using UnityEngine;

public class Cell : MonoBehaviour
{

    private LayerMask whatIsPlayer;
    private CellState _state;

    public delegate void CellTrigered(CellPos pos, Cell cell);
    public event CellTrigered OnCellTriggered;

    private Coroutine _breakCoroutine;
    private bool _playerOnTile;

    public CellPos Position { get; private set; }
    public CellState State { get => _state; set => _state = value; }

    public void Init(CellPos pos, CellState state) {  Position = pos; State = state; }

    private void Awake()
    {
        whatIsPlayer = LayerMask.GetMask("whatIsPlayer");
    }

    private void OnTriggerEnter(Collider other) 
    {

        if (((1 << other.gameObject.layer) & whatIsPlayer) != 0)
        {
            _playerOnTile = true;

            OnCellTriggered?.Invoke(Position, this);
            if (State == CellState.Broken)
            {
                _breakCoroutine = StartCoroutine(WaitForBreak());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & whatIsPlayer) != 0)
        {
            _playerOnTile = false;
            if (_breakCoroutine != null)
            {
                StopCoroutine(_breakCoroutine);
                _breakCoroutine = null;
            }
        }
    }

    private IEnumerator WaitForBreak()
    {
        yield return new WaitForSeconds(0.5f);

        if (_playerOnTile)
        {
            ACharacter chara = GameManager.Instance.Character;
            chara.Rb.velocity = Vector3.zero;
            chara.StateMachine.ChangeState(chara.StateMachine.States[EnumStateCharacter.Fall]);
        }

        _breakCoroutine = null;
    }

}
