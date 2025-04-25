using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadScene : MonoBehaviour
{
    public void ReloadCurrentScene()
    {
        RiwaLoadSceneSystem.Instance.ReloadCurrentScene();
    }
}
