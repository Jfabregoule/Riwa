using System;
using UnityEngine;

namespace HSM
{
    [Serializable]
    public class CharacterContext
    {
        [Header("Character Shared Attributes"), Space(10)]
        public Vector3 Move;
        public Vector3 Velocity;
        public bool Grounded;
        public float MoveSpeed = 6f;
        public float Acceleration = 40f;
        public Animator Animator;
        public Rigidbody Rigidbody;
    }
}