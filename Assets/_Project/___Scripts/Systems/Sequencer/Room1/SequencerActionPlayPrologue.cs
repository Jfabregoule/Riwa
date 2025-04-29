using Cinemachine;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Play Prologue", menuName = "Riwa/Room1/Play Prologue")]
public class SequencerActionPlayPrologue : SequencerAction
{
    private bool _isFirstCinematic;
    public override void Initialize(GameObject obj)
    {
        _isFirstCinematic = SaveSystem.Instance.LoadElement<bool>("FinishPrologue");
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        if (!_isFirstCinematic)
        {
            bool videoFinished = false;

            UnityEngine.Events.UnityAction onVideoEndedCallback = null;
            onVideoEndedCallback = () =>
            {
                videoFinished = true;
                RiwaCinematicSystem.Instance.OnVideoEnded.RemoveListener(onVideoEndedCallback);
            };

            RiwaCinematicSystem.Instance.OnVideoEnded.AddListener(onVideoEndedCallback);

            RiwaCinematicSystem.Instance.PlayVideoByKey("Starting Cinematic");
            SaveSystem.Instance.SaveElement("FinishPrologue", true);

            yield return new WaitUntil(() => videoFinished);

        }
    }
}
