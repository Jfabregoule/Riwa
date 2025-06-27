using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class TutorialRoom3Manager : MonoBehaviour
{

    [Header("Dialogues")]
    [SerializeField] private List<DialogueAsset> _damierDialogue;
    [SerializeField] private DialogueAsset _lianaDialogue;

    private Floor1Room3LevelManager _instance;
    private bool _chawaAlreadySpawned = false;
    private Vector3 _damierDiscussionPosition;
    private int _currentIndex = 0;
    private int _previousIndex = -1;

    public Vector3 DamierDiscussionPosition { get => _damierDiscussionPosition; set => _damierDiscussionPosition = value; }
    public List<DialogueAsset> Room3Dialogue { get => _damierDialogue; }

    #region Events

    public Action RiwaHiddingIntoSensa;
    public Action WaitEndOfLianaPathTravel;

    #endregion

    private void Start()
    {
        _instance = (Floor1Room3LevelManager)Floor1Room3LevelManager.Instance;
        _instance.OnLevelEnter += Init;
        DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.WaitEndOfLianaPathTravel, WaitEndOfLianaPathTravel);
        DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.RiwaHiddingIntoSensa, RiwaHiddingIntoSensa);
        DialogueSystem.Instance.OnDialogueEvent += DispatchEventOnDialogueEvent;
    }

    private void Init()
    {
        DialogueSystem.Instance.BeginDialogue(_damierDialogue[0]);
    }

    private void DispatchEventOnDialogueEvent(DialogueEventType dialogueEvent)
    {
        switch (dialogueEvent)
        {
            case DialogueEventType.RiwaSensaDamierDiscussionRoom3:
                StartCoroutine(SensaChawaDiscuss(0));
                break;
            case DialogueEventType.RiwaSensaLianaDiscussionRoom3:
                StartCoroutine(SensaChawaDiscuss(1));
                break;
            case DialogueEventType.RiwaShowDamierPath:
                _instance.RiwaSensaCamera[0].Priority = 0;
                _instance.Chawa.SetActive(true);
                _instance.InvokeOnRiwaShowingPath();
                break;
            case DialogueEventType.RiwaEndShowingPath:
                _instance.ChawaPathTriggerZone.enabled = true;
                _instance.RiwaSensaCamera[0].Priority = 0;
                GameManager.Instance.UIManager.StartPulse(UIElementEnum.ChangeTime);
                break;
            case DialogueEventType.ShowLianaPath:
                StartCoroutine(SwitchLianaCamera());
                break;
        }
    }

    private void SwitchToCamera(int index)
    {
        if(_previousIndex >= 0 && _previousIndex < _instance.VinesCameras.Count)
            _instance.VinesCameras[_previousIndex].Priority = 0;

        _instance.VinesCameras[index].Priority = 20;
    }

    #region Coroutine

    #region Sensa & Chawa discussion

    public IEnumerator SensaChawaDiscuss(int cameraID)
    {
        _instance.RiwaSensaCamera[cameraID].Priority = 20;
        if (_chawaAlreadySpawned == true) yield break;
        _instance.Chawa.SetActive(true);

        Vector3 initialChawaPos = _instance.Chawa.transform.position;
        Vector3 sensaPosition = GameManager.Instance.Character.transform.position;
        Vector3 targetPosition = new Vector3(sensaPosition.x - 1f, 0.5f, initialChawaPos.z + 1f);
        Vector3 finalScale = new Vector3(0.5f, 0.5f, 0.5f);

        Quaternion initialChawaRot = _instance.Chawa.transform.rotation;
        Quaternion chawaTargetRotation = Quaternion.Euler(0f, 90f, 0f);
        Quaternion sensaRotateTowardRiwa = Quaternion.Euler(0f, -90f, 0f);

        _damierDiscussionPosition = targetPosition;

        float elapsedTime = 0f;
        float lerpTime = 2f;

        while (elapsedTime < lerpTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / lerpTime);
            GameManager.Instance.Character.transform.rotation = Quaternion.Slerp(GameManager.Instance.Character.transform.rotation, sensaRotateTowardRiwa, t);
            _instance.Chawa.transform.position = Vector3.Lerp(initialChawaPos, targetPosition, t);
            _instance.Chawa.transform.rotation = Quaternion.Slerp(initialChawaRot, chawaTargetRotation, t);
            _instance.Chawa.transform.localScale = Vector3.Lerp(_instance.Chawa.transform.localScale, finalScale, t);
            yield return null;
        }

        GameManager.Instance.Character.transform.rotation = sensaRotateTowardRiwa;
        _instance.Chawa.transform.position = targetPosition;
        _instance.Chawa.transform.rotation = chawaTargetRotation;
        _instance.Chawa.transform.localScale = finalScale;
        _instance.Chawa.transform.SetParent(null);
        _chawaAlreadySpawned = true;
    }

    public IEnumerator HideRiwa()
    {
        Vector3 initialPos = _instance.Chawa.transform.position;
        Vector3 targetPos = GameManager.Instance.Character.transform.position;
        targetPos.y = 0.5f;
        Vector3 finalScale = new Vector3(0f, 0f, 0f);
        Quaternion finalRotation = Quaternion.Euler(0f, 0f, 0f);

        float elapsedTime = 0f;

        while (elapsedTime < 1.5f)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / 1.5f);
            _instance.Chawa.transform.position = Vector3.Lerp(initialPos, targetPos, t);
            _instance.Chawa.transform.localScale = Vector3.Lerp(_instance.Chawa.transform.localScale, finalScale, t);
            _instance.Chawa.transform.rotation = Quaternion.Slerp(_instance.Chawa.transform.rotation, finalRotation, t);
            GameManager.Instance.Character.transform.rotation = Quaternion.Slerp(GameManager.Instance.Character.transform.rotation, finalRotation, t);
            yield return null;
        }

        _instance.Chawa.transform.position = targetPos;
        _instance.Chawa.transform.localScale = finalScale;
        _instance.Chawa.transform.rotation = finalRotation;
        _instance.Chawa.SetActive(false);
        GameManager.Instance.Character.transform.rotation = finalRotation;

        yield return new WaitForSeconds(1.5f);
        DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.RiwaHiddingIntoSensa);
        _instance.RiwaSensaCamera[1].Priority = 0;
    }

    public void RiwaEndShowDamierPath()
    {
        DialogueSystem.Instance.BeginDialogue(_damierDialogue[1]);
    }

    #endregion

    #region Liana CameraSwitch Coroutine

    private IEnumerator SwitchLianaCamera()
    {
        _instance.RiwaSensaCamera[1].Priority = 0;

        while (_currentIndex < _instance.VinesCameras.Count)
        {
            SwitchToCamera(_currentIndex);
            _previousIndex = _currentIndex;
            _currentIndex++;
            yield return new WaitForSeconds(2.5f);
        }

        _instance.VinesCameras[_previousIndex].Priority = 0;
        DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.WaitEndOfLianaPathTravel);
        _instance.RiwaSensaCamera[1].Priority = 20;
        yield return new WaitForSeconds(1.5f);
        _instance.TreeStumpTest.enabled = true;
        _instance.TreeStumpTest.CanInteract = true;
        _instance.ChawaTrail.gameObject.SetActive(false);
        StartCoroutine(HideRiwa());
    }
    
    public IEnumerator MoveAndOrientChawaToLiana()
    {
        Vector3 start = _instance.Chawa.transform.position;
        Vector3 end = _instance.ChawaLianaPosition.position;
        Vector3 controlBefore = start - (_instance.Chawa.transform.forward * 2f);
        Vector3 controlAfter = end + (_instance.Chawa.transform.forward * 2f);
        
        Vector3 sensaPos = GameManager.Instance.Character.transform.position;
        Vector3 direction = (sensaPos - end).normalized;
        Quaternion targetRot = Quaternion.LookRotation(direction);
        
        Vector3 directionToTarget = (end - start).normalized;
        _instance.Chawa.transform.rotation = Quaternion.LookRotation(directionToTarget);
        
        Quaternion startRot = _instance.Chawa.transform.rotation;

        float duration = 3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            Vector3 pos = Helpers.InterpolationHermite(controlBefore, start, end, controlAfter, t);
            _instance.Chawa.transform.position = pos;

            yield return null;
        }

        _instance.Chawa.transform.position = end;
        elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            
            _instance.Chawa.transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        _instance.Chawa.transform.rotation = targetRot;

        DialogueSystem.Instance.BeginDialogue(_lianaDialogue);
    }

    #endregion

    #endregion
}
