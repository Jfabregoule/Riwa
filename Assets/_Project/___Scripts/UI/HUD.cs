using UnityEngine;

public class HUD : MonoBehaviour
{
    private bool _isInteracting = true;

    public void ChangeTime()
    {
        GameManager.Instance.Character.TriggerChangeTempo();
    }

    public void ChangeToSoul()
    {
        GameManager.Instance.Character.IsInSoul = !GameManager.Instance.Character.IsInSoul;
    }

    public void ToggleInteract()
    {
        if (_isInteracting)
        {
            GameManager.Instance.Character.InputManager.InteractTrue();
        }
        else
        {
            GameManager.Instance.Character.InputManager.InteractFalse();
        }
        _isInteracting = !_isInteracting;
    }

}
