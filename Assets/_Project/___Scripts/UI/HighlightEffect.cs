using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class HighlightEffect : MonoBehaviour
{
    private BlackScreen _blackScreen;

    private GameObject _tempoButtonParent;
    private GameObject _tempoButton;
    void Start()
    {
        StartCoroutine(Helpers.WaitMonoBeheviour(() => GameManager.Instance.UIManager, WaitUIManager));
        
    }

    public void StartHighlight()
    {
        _blackScreen.FadeIn(0.3f);

        _tempoButton = gameObject;
        _tempoButtonParent = gameObject.transform.parent.gameObject;
        _tempoButton.transform.parent = transform.parent.transform;
        _tempoButton.transform.SetSiblingIndex(0);
    }

    public void StopHighlight()
    {
        _blackScreen.FadeOut(0.3f);

        if (_tempoButton == null) return;

        if (_tempoButtonParent != null)
        {
            _tempoButton.transform.parent = _tempoButtonParent.transform;
            _tempoButtonParent = null;
            _tempoButton = null;
        }

    }

    private void WaitUIManager(UIManager manager)
    {
        if (manager != null)
        {
            _blackScreen = manager.BlackScreen;
        }
    }
}
