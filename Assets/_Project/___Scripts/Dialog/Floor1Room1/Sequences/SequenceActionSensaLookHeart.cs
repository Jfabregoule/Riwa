using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sensa Look Heart", menuName = "Riwa/Room1/Sensa Look Heart")]
public class SequenceActionSensaLookHeart : SequencerAction
{

    private Floor1Room1LevelManager _instance;

    public override void Initialize(GameObject obj)
    {
        _instance = (Floor1Room1LevelManager)Floor1Room1LevelManager.Instance;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        Vector3 lookDirection = _instance.RiwaHeart.transform.position - GameManager.Instance.Character.transform.position;
        lookDirection.y = 0f;

        Quaternion lookDir = Quaternion.LookRotation(lookDirection);

        float elapsedTime = 0f;
        float lerpTime = 2f;

        while(elapsedTime < lerpTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / lerpTime;
            GameManager.Instance.Character.transform.rotation = Quaternion.Slerp(GameManager.Instance.Character.transform.rotation, lookDir, t);
            yield return null;
        }

        GameManager.Instance.Character.transform.rotation = lookDir;
    }
}
