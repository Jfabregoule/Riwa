using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public interface IMovable
{
    public float Move(Vector3 direction); //Prend la direction du poussage, retoune la speed du poussage
}