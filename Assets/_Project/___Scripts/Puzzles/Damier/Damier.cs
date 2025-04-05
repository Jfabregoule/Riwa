using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damier : MonoBehaviour
{
    /// <summary>
    /// TO DO :
    /// Each cell should know if its breakable or not
    /// Also when the player is on it, should break if breakable
    /// Generate a good path each Start (or Awake)
    /// </summary>

    // Basic test damier generation (visualisation)
    [ContextMenu("Generate Damier")]
    public void Generate()
    {
        int size = 6;
        float cellSize = 1f;
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cell.transform.position = new Vector3(x * cellSize, 0, y * cellSize);
                cell.transform.localScale = new Vector3(cellSize, 0.1f, cellSize);
                cell.transform.parent = transform;
            }
        }
    }
}
