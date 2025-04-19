using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEventRegistry
{
    private Dictionary<WaitDialogueEventType, Action> _events = new();

    public void Register(WaitDialogueEventType type, Action action)
    {
        if (_events.ContainsKey(type))
            _events[type] += action;
        else
            _events[type] = action;
    }

    public void Unregister(WaitDialogueEventType type, Action action)
    {
        if (_events.ContainsKey(type))
            _events[type] -= action;
    }

    public void Invoke(WaitDialogueEventType type)
    {
        if (_events.TryGetValue(type, out var action))
            action?.Invoke();
    }
}
