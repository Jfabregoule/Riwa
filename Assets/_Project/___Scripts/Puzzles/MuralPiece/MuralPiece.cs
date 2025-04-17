using UnityEngine;

public class MuralPiece : MonoBehaviour, IInteractable
{
    public float OffsetRadius { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public delegate void MuralPieceEvent();
    public MuralPieceEvent OnPickUp;

    public void Interact()
    {
        OnPickUp?.Invoke();
    }
}
