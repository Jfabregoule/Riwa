using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineScript : MonoBehaviour
{
    public List<MeshRenderer> renderers;

    public float timeToGrow = 5;
    public float refreshRate = 0.05f;
    [Range(0, 1)]
    public float minGrow = 0.2f;
    [Range(0, 1)]
    public float maxGrow = 0.97f;

    private List<Material> materials = new List<Material>();
    private bool fullyGrown;
    public bool Activated;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < renderers.Count; i++) 
        {
            for (int j = 0; j < renderers[i].materials.Length; j++)
            {
                if (renderers[i].materials[j].HasProperty("_Grow"))
                {
                    renderers[i].materials[j].SetFloat("_Grow",minGrow);
                    materials.Add(renderers[i].materials[j]);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Activated)
        {
            for(int i = 0;i < materials.Count; i++)
            {
                StartCoroutine(GrowVine(materials[i]));
            }
        }
    }

    private IEnumerator GrowVine(Material mat)
    {
        float growValue = mat.GetFloat("_Grow");
        if (!fullyGrown)
        {
            while(growValue < maxGrow)
            {
                growValue += 1 / (timeToGrow / refreshRate);
                mat.SetFloat("_Grow", growValue);
                yield return new WaitForSeconds(refreshRate);
            }
        }
        else
        {
            while (growValue > minGrow)
            {
                growValue -= 1 / (timeToGrow / refreshRate);
                mat.SetFloat("_Grow", growValue);
                yield return new WaitForSeconds(refreshRate);
            }
        }

        if(growValue >= maxGrow)
        {
            fullyGrown = true;
        }
        else
        {
            fullyGrown= false;
        }
    }
}
