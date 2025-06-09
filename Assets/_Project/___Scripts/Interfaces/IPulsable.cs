using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public interface IPulsable 
{
    // Start is called before the first frame update
    void StartPulsing();

    void StopPulsing();
}
