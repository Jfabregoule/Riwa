using UnityEngine;

public class MuralPiece : MonoBehaviour, IInteractable
{
    public float OffsetRadius { get => 0f; set => OffsetRadius = value; }

    public delegate void MuralPieceEvent();
    public MuralPieceEvent OnPickUp;

    public void Interact()
    {
        OnPickUp?.Invoke();
        GameObject.Destroy(gameObject);
    }
}
