using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


[CreateAssetMenu(fileName = "Change Camera Sequence", menuName = "Riwa/GenericAction/Change Camera")]
public class SequenceActionChangeCamera : SequencerAction
{
    [SerializeField] private bool _waitForEndOhPath = true;
    [SerializeField] private float _secondesBeforeSkip = 0f;
    [SerializeField] private float _duration = 1f;

    [SerializeField] private EnumCameraRoom _targetCamera;

    private CinemachineBrain _brain;
    private CinemachineVirtualCamera _currentCamera;
    private CinemachineBlendDefinition _defaultBlend;

    public override void Initialize(GameObject obj)
    {
        _brain = Helpers.Camera.GetComponent<CinemachineBrain>();
        _defaultBlend = _brain.m_DefaultBlend;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        _brain.m_DefaultBlend = new CinemachineBlendDefinition(
            CinemachineBlendDefinition.Style.EaseInOut,
            _duration
        );
        (_brain.ActiveVirtualCamera as CinemachineVirtualCamera).Priority = 10;
        BaseLevelManager.Instance.CameraDictionnary[_targetCamera].Priority = 20;

        if (_waitForEndOhPath)
        { 
            //On dirait que isBlending marche aps
            while (_brain.IsBlending)
            {
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(_secondesBeforeSkip);   
        }

        _brain.m_DefaultBlend = _defaultBlend;
    }
}
