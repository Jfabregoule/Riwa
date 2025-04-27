using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heart Taken", menuName = "Riwa/Room1/Heart Taken")]
public class SequenceActionHeartTaken : SequencerAction
{

    private Floor1Room1LevelManager _instance;

    public override void Initialize(GameObject obj)
    {
        _instance = (Floor1Room1LevelManager)Floor1Room1LevelManager.Instance;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        Vector3 initialScale = _instance.RiwaHeart.transform.localScale;
        Vector3 targetScale = Vector3.zero;

        Vector3 initialPos = _instance.RiwaHeart.transform.position;
        Vector3 targetPos = GameManager.Instance.Character.transform.position;
        targetPos.y += 0.3f;

        List<Vector3> initialParticleScale = new List<Vector3>();
        for(int i = 0; i < _instance.RiwaHeartPS.Count; i++)
        {
            initialParticleScale.Add(_instance.RiwaHeartPS[i].transform.localScale);
        }

        float elapsedTime = 0f;
        float lerpTime = 3f;

        _instance.RiwaHeart.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("Taken");

        while(elapsedTime < lerpTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / lerpTime);
            _instance.RiwaHeart.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            _instance.RiwaHeart.transform.position = Vector3.Lerp(initialPos, targetPos, t);
            for(int i = 0; i < _instance.RiwaHeartPS.Count; i++)
            {
                _instance.RiwaHeartPS[i].transform.localScale = Vector3.Lerp(initialParticleScale[i], targetScale, t);
            }
            yield return null;
        }

        Destroy(_instance.RiwaHeart);
    }
}
