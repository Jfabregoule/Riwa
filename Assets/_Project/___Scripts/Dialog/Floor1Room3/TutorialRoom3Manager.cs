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
    private int currentIndex = 0;
    private int previousIndex = -1;

    public Vector3 DamierDiscussionPosition { get => _damierDiscussionPosition; set => _damierDiscussionPosition = value; }
    public List<DialogueAsset> Room3Dialogue { get => _damierDialogue; }
    public DialogueAsset LianaDialogue { get => _lianaDialogue; }

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
                break;
            case DialogueEventType.ShowLianaPath:
                StartCoroutine(SwitchLianaCamera());
                break;
        }
    }

    private void SwitchToCamera(int index)
    {
        if(previousIndex >= 0 && previousIndex < _instance.VinesCameras.Count)
            _instance.VinesCameras[previousIndex].Priority = 0;

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
        //_instance.Chawa.transform.SetParent(GameManager.Instance.Character.transform);
        //if (showDamierDialogue) DialogueSystem.Instance.BeginDialogue(_damierDialogue[1]);
        Vector3 initialPos = _instance.Chawa.transform.position;
        Vector3 targetPos = GameManager.Instance.Character.transform.position;
        targetPos.y = 0.5f;
        Vector3 finalScale = new Vector3(0f, 0f, 0f);
        Quaternion finalRotation = Quaternion.Euler(0f, 0f, 0f);

        float elapsedTime = 0f;

        while (elapsedTime < 2f)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / 2f);
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
        //if (!showDamierDialogue)
        //{
        //    //_instance.RiwaSensaCamera[1].Priority = 0;
        //    //GameManager.Instance.Character.InputManager.EnableGameplayControls();
        //}
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

        while (currentIndex < _instance.VinesCameras.Count)
        {
            SwitchToCamera(currentIndex);
            previousIndex = currentIndex;
            currentIndex++;
            yield return new WaitForSeconds(2.5f);
        }

        _instance.VinesCameras[previousIndex].Priority = 0;
        DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.WaitEndOfLianaPathTravel);
        _instance.RiwaSensaCamera[1].Priority = 20;
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(HideRiwa());
    }

    public IEnumerator BringRiwaToLiana()
    {
        Vector3 initialPos = _instance.Chawa.transform.position;
        Vector3 targetPos = _instance.ChawaLianaPosition.position;

        float elapsedTime = 0f;
        float lerpTime = 3f;

        while (elapsedTime < lerpTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / lerpTime);
            _instance.Chawa.transform.position = Vector3.Lerp(initialPos, targetPos, t);
            yield return null;
        }
        _instance.Chawa.transform.position = targetPos;
        StartCoroutine(OrientChawaTowardSensa());
    }

    public IEnumerator OrientChawaTowardSensa()
    {
        DialogueSystem.Instance.BeginDialogue(_lianaDialogue);
        Vector3 chawaPosition = _instance.Chawa.transform.position;
        Vector3 sensaPosition = GameManager.Instance.Character.transform.position;
        Vector3 chawaDirecrion = sensaPosition - chawaPosition;

        Quaternion startRotation = _instance.Chawa.transform.rotation;
        Quaternion lookDirection = Quaternion.identity;
        lookDirection = Quaternion.LookRotation(chawaDirecrion);

        float elapsedTime = 0f;
        float lerpTime = 2f;

        while(elapsedTime < lerpTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / lerpTime);
            _instance.Chawa.transform.rotation = Quaternion.Slerp(startRotation, lookDirection, t);
            yield return null;
        }

        _instance.Chawa.transform.rotation = lookDirection;
    }

    #endregion

    #endregion
}
