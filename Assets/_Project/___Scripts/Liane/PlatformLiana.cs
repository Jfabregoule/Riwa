using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLiana : MonoBehaviour
{
    private List<Vector3> _platformPositions;

    private float _platformSpeed;
    private int _currentIndex;
    private bool _canMove;
    void Start()
    {
        _canMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_platformPositions == null) return;
        if (!_canMove) return;

        transform.position = Vector3.MoveTowards(transform.position, _platformPositions[_currentIndex], _platformSpeed * Time.deltaTime );
        if (Vector3.Distance(_platformPositions[_currentIndex], transform.position) < 0.01f)
        {
            transform.position = _platformPositions[_currentIndex];
            _currentIndex++;
            _canMove=false;
        }
    }

    public void SetPlatformInfos(List<Vector3> positions, float speed)
    {
        _platformPositions = positions;
        _platformSpeed = speed;
        _currentIndex = 0;
        _canMove = true;
    }
}
