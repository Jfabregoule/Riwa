using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public enum EnumAdvancementRoom1
{
    None,
    LookAtTree,
    HasEnterTree,
    HasOpenDoor
}

public class Floor1Room1LevelManager : BaseLevelManager
{

    [Header("Floor1 Room1")]

    public EnumAdvancementRoom1 CurrentAdvancement = EnumAdvancementRoom1.None; //A sauvegarder
    [SerializeField] private bool _areEventEnabled = true;

    [Header("Event Sequencer")]

    [SerializeField] private Sequencer _event1;
    [SerializeField] private Sequencer _event2;
    [SerializeField] private Sequencer _event3;

    private Floor1Room1Dialogue _dialogue;
    private System.Action OnChangeTime;

    //Enigmes

    private bool _isCrateWellPlaced;

    public override void Start()
    {
        base.Start();
        //RiwaCinematicSystem.Instance.PlayVideoByKey("Starting Cinematic");
        _dialogue = GetComponent<Floor1Room1Dialogue>();
        DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.ChangeTime, OnChangeTime);

        foreach (var cam in _cameras)
        {
            CameraDictionnary[cam._id] = cam._camera;
        }

        _character.InputManager.OnChangeTime += CheckCrateEnigma;
        _character.InputManager.OnChangeTime += InvokeChangeTime;

        if (CurrentAdvancement < EnumAdvancementRoom1.LookAtTree)
        {
            CurrentAdvancement = EnumAdvancementRoom1.LookAtTree;

            if(_areEventEnabled)
            {
                _event1.Init();
                _event2.Init();
                _event3.Init();

                _event1.InitializeSequence();

                _dialogue.LaunchDialogue(0);
            }
        }
    }


    public void TriggerEvent2()
    {
        if (CurrentAdvancement < EnumAdvancementRoom1.HasEnterTree)
        { 
            CurrentAdvancement = EnumAdvancementRoom1.HasEnterTree;
            if (_areEventEnabled)
            {                 
                _event2.InitializeSequence();
            }
        }
    }

    public void SetCrateWellPlaced(bool value)
    {
        _isCrateWellPlaced = value;
    }

    public void CheckCrateEnigma()
    {
        if (!_isCrateWellPlaced) { return; }

        if (CurrentAdvancement < EnumAdvancementRoom1.HasEnterTree)
        {
            CurrentAdvancement = EnumAdvancementRoom1.HasEnterTree;
            
            if (_areEventEnabled)
            {
                _event3.InitializeSequence();
            }
        }

        _character.InputManager.OnChangeTime -= CheckCrateEnigma;

    }

    public void InvokeChangeTime() {
        DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.ChangeTime);
        _character.InputManager.OnChangeTime -= InvokeChangeTime;
    }

}
