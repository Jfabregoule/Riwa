using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectToggle : MonoBehaviour
{
    [SerializeField] private bool _isPast;

    [Header("GameObjects")]
    [SerializeField] private List<GameObject> _pastGameObjects;
    [SerializeField] private List<GameObject> _presentGameObjects;

    private void Start()
    {
        SetGameObjects(GameManager.Instance.CurrentTemporality);
    }

    private void OnEnable()
    {
        GameManager.Instance.OnTimeChangeStarted += SetGameObjects;
    }

    private void OnDisable()
    {
        if (GameManager.Instance)
            GameManager.Instance.OnTimeChangeStarted -= SetGameObjects;
    }

    private void SetGameObjects(EnumTemporality temporality)
    {
        if (temporality == EnumTemporality.Present)
        {
            foreach (GameObject gameObject in _pastGameObjects)
            {
                gameObject.SetActive(false);
            }
            foreach (GameObject gameObject in _presentGameObjects)
            {
                gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject gameObject in _presentGameObjects)
            {
                gameObject.SetActive(false);
            }
            foreach (GameObject gameObject in _pastGameObjects)
            {
                gameObject.SetActive(true);
            }
        }
    }
}
