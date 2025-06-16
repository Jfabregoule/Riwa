using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameCinematic : MonoBehaviour
{
    [SerializeField] private Transform _socle;
    [SerializeField] private Transform _bourgeon;
    [SerializeField] private Transform _lianaLandTransform;
    [SerializeField] private Transform _elevatorTransform;
    [SerializeField] private Transform _elevatorPlatformTransform;
    public Transform Socle { get => _socle; }
    public Transform Bourgeon { get => _bourgeon; }
    public Transform LianaLandTransform { get => _lianaLandTransform; }
    public Transform ElevatorTransform { get => _elevatorTransform; }
    public Transform ElevatorPlatformTransform { get => _elevatorPlatformTransform; }
}
