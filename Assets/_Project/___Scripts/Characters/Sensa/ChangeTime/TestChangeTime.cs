using UnityEngine;

[ExecuteInEditMode]

public class TestChangeTime : MonoBehaviour
{
    public float radius = 1f;
    public int PresentEnum = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalVector("_Position", transform.position);
        Shader.SetGlobalFloat("_Radius", radius);
        Shader.SetGlobalInt("_PresentEnum", PresentEnum);
    }
}
