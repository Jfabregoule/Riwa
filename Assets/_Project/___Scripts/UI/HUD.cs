using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public void ChangeTime()
    {
        GameManager.Instance.Character.IsChangingTime = true;
    }
}
