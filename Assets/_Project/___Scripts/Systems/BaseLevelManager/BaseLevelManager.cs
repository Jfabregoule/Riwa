using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BaseLevelManager : Singleton<BaseLevelManager>
{
    /// <summary>
    /// Chaque level aura un enfant de ce script
    /// Ce Manager va concerner l'avancée de la scene à un niveau logique
    /// 
    /// Contient les 3C du niveau (mettre en enfant du gameObject du manager les 3C pour y accéder plus facilement ?)
    /// 
    /// </summary>

    public CinemachineBrain CinemachineBrainCamera;
    
    [Header("Les 3C")]

    public CameraHandler CameraHandler;
    public ACharacter Character;
    public Joystick Joystick;

}
