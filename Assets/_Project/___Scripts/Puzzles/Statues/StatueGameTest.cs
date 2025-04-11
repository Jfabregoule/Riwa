using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;

public class StatueGameTest : MonoBehaviour
{
    [Header("Statues")]
    [SerializeField] private List<Statue> _statues;
    private Statue _activeStatue;
    private int _currentStatueIndex;

    private void Awake()
    {
        //_activeStatue = _statues.Find(statue => !statue.IsLocked);
        _currentStatueIndex = _statues.IndexOf(_activeStatue);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) _activeStatue.Move(Vector2.up);
        if (Input.GetKeyDown(KeyCode.S)) _activeStatue.Move(Vector2.down);
        if (Input.GetKeyDown(KeyCode.A)) _activeStatue.Move(Vector2.left);
        if (Input.GetKeyDown(KeyCode.D)) _activeStatue.Move(Vector2.right);
        if (Input.GetKeyDown(KeyCode.E)) _activeStatue.Rotate(45);
        if (Input.GetKeyDown(KeyCode.Tab)) SwitchStatue();
    }

    private void SwitchStatue()
    {
        _currentStatueIndex++;
        if (_currentStatueIndex >= _statues.Count) _currentStatueIndex = 0;
        Statue oldActiveStatue = _activeStatue;
        Statue newActiveStatue = _statues[_currentStatueIndex];
        _activeStatue = _statues[_currentStatueIndex];
        //oldActiveStatue.IsLocked = true;
        //newActiveStatue.IsLocked = false;
        Debug.Log("Active Statue: " + _activeStatue.name);
    }
}
