using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum UIPulseEnum
{
    ChangeTime, 
    Interact
}

[System.Serializable]
public struct UIPulse
{
    public UIPulseEnum Enum;
    public MonoBehaviour Pulsable;
}

[DefaultExecutionOrder(-1)]
public class UIManager : MonoBehaviour
{
    [SerializeField] private Control _control;
    [SerializeField] private Navbar _navbar;
    [SerializeField] private BlackScreen _blackScreen;
    [SerializeField] private List<UIPulse> _pulsableList;

    private Dictionary<UIPulseEnum, IPulsable> _pulses;

    public Control Control { get { return _control; } }
    public BlackScreen BlackScreen { get { return _blackScreen; } }
    
    public Navbar Navbar { get { return _navbar; } } 

    private void Awake()
    {
        _pulses = new Dictionary<UIPulseEnum, IPulsable>();
        foreach (var pair in _pulsableList)
        {
            _pulses[pair.Enum] = (IPulsable)pair.Pulsable;
        }
    }
    void Start()
    {
    }

    private void OnDestroy()
    {
    }

    void Update()
    {
    }

    public void StartPulse(UIPulseEnum pulseEnum)
    {
        _pulses[pulseEnum].StartPulsing();
    }
    public void StopPulse(UIPulseEnum pulseEnum)
    {
        _pulses[pulseEnum].StopPulsing();
    }

    public void StartHighlight(UIPulseEnum pulseEnum) => _blackScreen.HighlightButton(_pulses[pulseEnum]);
    public void StopHighlight() => _blackScreen.ResetHighlighButton();

}
