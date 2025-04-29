using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SetPlayerCollider", menuName = "Riwa/GenericAction/SetPlayerCollider")]
public class SequenceActionPlayerCollider : SequencerAction
{
    [SerializeField] private bool _isPlayerColliderActive;
    private CapsuleCollider _characterCollider;

    public override void Initialize(GameObject obj)
    {
        _characterCollider = GameManager.Instance.Character.GetComponent<CapsuleCollider>();
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        _characterCollider.enabled = _characterCollider;
        yield return null;
    }

}