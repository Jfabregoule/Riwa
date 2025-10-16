using System.Collections;
using UnityEngine;

public interface IMovable : IHoldable
{
    float MoveSpeed { get; set;}
    float MoveDistance { get; set;}

    ////Sert pour les mouvements crampt�s
    ////Si on stock pas le surplus de temps � chaque lerp, si on fait des mouvements 
    ////continus l'objet ne bougera pas de mani�re constante, sur une certaines distance Sensa sera d�cal�e
    //float TimeOverflow { get; set;} 

    bool Move(Vector3 direction);
    IEnumerator MoveLerp(Vector3 direction);

    delegate void NoArgVoid();
    event NoArgVoid OnMoveFinished;

    delegate void NoArgVector3(Vector3 targetPosition);
    event NoArgVector3 OnReplacePlayer;

}