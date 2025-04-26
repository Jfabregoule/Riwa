using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enable Button", menuName = "Riwa/GenericAction/Enable Button")]
public class SequenceActionEnableGameObject : SequencerAction
{
    [SerializeField] private GameObject _goToEnable;
    public override void Initialize(GameObject obj)
    {
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        _goToEnable.SetActive(true);
        yield return null; 
    }

}
