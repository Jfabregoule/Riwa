using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiwaEndGame : MonoBehaviour
{

    private Floor1Room1LevelManager _instance;
    private bool _gameEndTriggered = false;

    private void Start()
    {
        _instance = (Floor1Room1LevelManager)Floor1Room1LevelManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && _gameEndTriggered == false)
        {
            _gameEndTriggered = true;
            GameManager.Instance.Character.InputManager.DisableGameplayControls();
            _instance.UpdateAdvancement(EnumAdvancementRoom1.End);
            _instance.EndGameSequencer.InitializeSequence();
        }
    }

}
