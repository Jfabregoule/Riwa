using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlInteract : MonoBehaviour
{

    private CanvasGroup _canvasGroup;
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    private void OnEnable()
    {
        GameManager.Instance.OnShowMoveInputEvent += ShowInteract;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowInteract()
    {
        Helpers.EnabledCanvasGroup(_canvasGroup);
        StartCoroutine(WaitSeconds(5f));
    }

    private IEnumerator WaitSeconds(float duration)
    {
        yield return new WaitForSeconds(duration);
        Helpers.DisabledCanvasGroup(_canvasGroup);
    }
}
