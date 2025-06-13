using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightEffect : MonoBehaviour
{
    private BlackScreen _blackScreen;

    private GameObject _tempoButtonParent;
    private GameObject _tempoButton;
    void Start()
    {
        _blackScreen = GameManager.Instance.UIManager.BlackScreen;
    }

    public void StartHighlight()
    {
        _blackScreen.GrayIn();

        _tempoButton = gameObject;
        _tempoButtonParent = gameObject.transform.parent.gameObject;
        _tempoButton.transform.parent = transform.parent.transform;
        //_tempoButton.transform.SetSiblingIndex(0);
    }

    public void StopHighlight()
    {
        _blackScreen.GrayOut();

        if (_tempoButton == null) return;

        if (_tempoButtonParent != null)
        {
            _tempoButton.transform.parent = _tempoButtonParent.transform;
            _tempoButtonParent = null;
            _tempoButton = null;
        }

    }
}
