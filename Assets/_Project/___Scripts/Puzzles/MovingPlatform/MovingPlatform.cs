using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] MonoBehaviour[] _activableComponents;
    [SerializeField] List<Transform> _points;
    [SerializeField] float _moveSpeed = 2f;

    private int _currentIndex = 0;
    private int _direction = 1;
    private List<Vector3> _targetPositions = new List<Vector3>();

    private void Start()
    {
        _targetPositions.Add(transform.position);

        foreach (Transform point in _points)
        {
            if (point != null)
                _targetPositions.Add(point.position);
        }

        if(_activableComponents.Length > 0)
        {
            foreach(var activable in  _activableComponents)
            {
                if(activable.TryGetComponent(out IActivable act))
                {
                    act.OnActivated += StartMoving;
                }
            }
        }

        if (_targetPositions.Count > 1 && _activableComponents.Length == 0)
            StartCoroutine(MovePlatform());
    }

    private void StartMoving()
    {
        OnMovingPlatformStateUptated(true);
        if (_targetPositions.Count > 1)
            StartCoroutine(MovePlatform());
    }

    private IEnumerator MovePlatform()
    {
        while (true)
        {
            Vector3 target = _targetPositions[_currentIndex];
            while (Vector3.Distance(transform.position, target) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, _moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = target;

            yield return new WaitForSeconds(2f);

            if (_currentIndex == _targetPositions.Count - 1)
                _direction = -1;
            else if (_currentIndex == 0)
                _direction = 1;

            _currentIndex += _direction;

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.transform.IsChildOf(transform))
        {
            other.transform.SetParent(null, true);
        }
    }

    public void OnMovingPlatformStateUptated(bool isActive)
    {
        float shaderIsActiveFloatValue = 0;
        if (isActive) shaderIsActiveFloatValue = 1;

        Renderer renderer = GetComponent<Renderer>();

        if (renderer.material.HasProperty("_IsActivated"))
            renderer.material.SetFloat("_IsActivated", shaderIsActiveFloatValue);

        if (transform.parent.gameObject.TryGetComponent(out Renderer parentRenderer))
        {
            if (parentRenderer.material.HasProperty("_IsActivated"))
                parentRenderer.material.SetFloat("_IsActivated", shaderIsActiveFloatValue);
        }
    }
}
