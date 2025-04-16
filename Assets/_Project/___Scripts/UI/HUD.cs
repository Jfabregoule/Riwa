using UnityEngine;

public class HUD : MonoBehaviour
{
    public void ChangeTime()
    {
        GameManager.Instance.Character.TriggerChangeTempo();
    }

    public void ChangeToSoul()
    {
        GameManager.Instance.Character.IsInSoul = !GameManager.Instance.Character.IsInSoul;
    }
}
