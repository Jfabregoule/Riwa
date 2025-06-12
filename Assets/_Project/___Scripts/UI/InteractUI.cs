using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class InteractUI : MonoBehaviour
{
    private CanvasGroup _leftInteractCanvas;
    private CanvasGroup _rightInteractCanvas;

    void Start()
    {
        _rightInteractCanvas = transform.GetChild(0).GetComponent<CanvasGroup>();
        _leftInteractCanvas = transform.GetChild(1).GetComponent<CanvasGroup>();

        StartCoroutine(Helpers.WaitMonoBeheviour(() => GameManager.Instance, SubscribeToGameManager));
    
    }

    private void SubscribeToGameManager(GameManager manager)
    {
        if (manager == null) return;
        manager.OnRoomChange += CharacterHolding;
    }

    private void CharacterHolding()
    {
        ACharacter _character = GameManager.Instance.Character;
        _character.OnHoldingStart += SetCanvasGroup;
        _character.OnHoldingEnd += DisableCanvasGroup;
    }

    private void SetCanvasGroup()
    {
        Helpers.EnabledCanvasGroup(GameManager.Instance.UIManager.IsRightHanded ? _rightInteractCanvas : _leftInteractCanvas);
    }

    private void DisableCanvasGroup()
    {
        Helpers.DisabledCanvasGroup(GameManager.Instance.UIManager.IsRightHanded ?  _rightInteractCanvas : _leftInteractCanvas);
    }

}
