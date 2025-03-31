using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteAlways]
public class Labeller : MonoBehaviour
{
    TMP_Text label;
    Vector2Int cords;
    StatuePuzzle gridManager;

    private void Awake()
    {
        gridManager = FindObjectOfType<StatuePuzzle>();
        label = GetComponentInChildren<TMP_Text>();

        DisplayCords();
    }

    private void Update()
    {
        DisplayCords();
        transform.name = cords.ToString();
    }

    private void DisplayCords()
    {
        if (!gridManager || !label) { return; }

        Vector3 relativePosition = transform.position - gridManager.Origin;
        cords.x = Mathf.RoundToInt(relativePosition.x / gridManager.UnitGridSize);
        cords.y = Mathf.RoundToInt(relativePosition.z / gridManager.UnitGridSize);

        label.text = $"{cords.x}, {cords.y}";
    }

}
