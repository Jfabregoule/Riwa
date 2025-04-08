using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineManager : Singleton<VineManager>
{

    public readonly List<VineScript> TriggerVines = new List<VineScript>();
    public delegate void VineChange();
    public event VineChange OnVineChange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InvokeVineChange()
    {
        OnVineChange?.Invoke();
    }
}
