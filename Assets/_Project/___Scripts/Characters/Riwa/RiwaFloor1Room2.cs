using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class RiwaFloor1Room2 : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _distance;
    private GameObject _mode;
    private GameObject _trail;
    private GameObject _sensa;

    private bool _isOut;

    private delegate void RiwaEvent();
    private event RiwaEvent _onMoveToFinsihed;
    void Start()
    {
        _sensa = GameManager.Instance.Character.gameObject;
        _mode = transform.GetChild(0).gameObject;
        _trail = transform.GetChild(1).gameObject;
        GoInSensa();
    }

    private void Update()
    {
        if (_isOut)
        {
            transform.LookAt(_sensa.transform.position);
        }
    }

    public void MoveToSensa()
    {
        _onMoveToFinsihed += GoInSensa;
        StartCoroutine(CoroutineMoveTo(_sensa.transform.position));
    }

    public void MoveOutSensa()
    {
        _isOut = true;
        transform.parent = null;
        _mode.SetActive(true);
        _trail.SetActive(true);
        _onMoveToFinsihed += GetOutSensa;
        StartCoroutine(CoroutineMoveTo(_sensa.transform.position + _sensa.transform.forward * _distance));
    }

    private void GoInSensa()
    {
        _isOut = false;
        transform.position = _sensa.transform.position;
        transform.SetParent(_sensa.transform);
        _mode.SetActive(false);
        _trail.SetActive(false);
    }

    private void GetOutSensa()
    {
        transform.SetParent(null, true);
    }

    private IEnumerator CoroutineMoveTo(Vector3 targetPos, bool endRotate = true)
    {
        Vector3 startPos = transform.position;
        targetPos.y = startPos.y;

        Vector3 direction = (targetPos - startPos).normalized;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        float distance = Vector3.Distance(startPos, targetPos);
        float duration = distance / _speed;
        float clock = 0f;

        while (clock < duration)
        {
            float t = clock / duration;

            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, Mathf.Clamp01(t * 3)); 

            clock += Time.deltaTime;

            yield return null;
        }

        clock = 0;
        startRotation = transform.rotation;
        Vector3 lookDir = transform.position - targetPos;
        lookDir.y = 0f;
        if (lookDir != Vector3.zero)
            targetRotation = Quaternion.LookRotation(lookDir);

        if (!endRotate)
        {
            clock = 1.1f;
        }

        while (clock < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, Mathf.Clamp01(clock * 8));

            clock += Time.deltaTime;

            yield return null;
        }

        _onMoveToFinsihed?.Invoke();
    }
}
