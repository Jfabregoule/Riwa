using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Play Prologue", menuName = "Riwa/Room1/Play Prologue")]
public class SequencerActionPlayPrologue : SequencerAction
{
    public override void Initialize(GameObject obj)
    {
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        bool isFirstCinematic = SaveSystem.Instance.LoadElement<bool>("FinishPrologue");
        if (!isFirstCinematic)
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
