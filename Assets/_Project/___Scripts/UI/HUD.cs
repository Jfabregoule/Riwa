using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public void ChangeTime()
    {
        GameManager.Instance.Character.IsChangingTime = true;
    }

    public void ChangeToSoul()
    {
        GameManager.Instance.Character.IsInSoul = !GameManager.Instance.Character.IsInSoul;
    }
}
