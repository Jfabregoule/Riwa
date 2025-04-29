using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpElevator", menuName = "Riwa/Room1/UpElevator")]
public class SequenceActionUpElevator : SequencerAction
{
    private Floor1Room1LevelManager _instance;
    private GameObject _elevator;

    private Vector3 _startPos;
    private Vector3 _endPos;

    [SerializeField] private float _duration = 4;

    public override void Initialize(GameObject obj)
    {
        _instance = (Floor1Room1LevelManager)GameManager.Instance.CurrentLevelManager;
        _elevator  = _instance.Evelator;

        _startPos = _elevator.transform.position;
        _endPos = _startPos + Vector3.up * 4; 
    }

    public override IEnumerator StartSequence(Sequencer context)
    {

        float clock = 0;

        GameManager.Instance.Character.transform.parent = _elevator.transform;  

        while (clock < _duration)
        {
            clock += Time.deltaTime;
            float t = clock / _duration;
            _elevator.transform.position = Vector3.Lerp(_startPos,_endPos,t);
            yield return null;
        }

        _elevator.transform.position = _endPos;
        GameManager.Instance.Character.transform.parent = null;

    }

}