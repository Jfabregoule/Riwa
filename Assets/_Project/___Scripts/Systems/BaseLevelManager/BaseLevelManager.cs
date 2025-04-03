using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BaseLevelManager : Singleton<BaseLevelManager>
{
    public CinemachineBrain CinemachineBrainCamera;
    public CinemachineVirtualCamera VirtualCamera;

    public ACharacter Character;

}
