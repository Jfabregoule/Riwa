using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraObstructionHandler : MonoBehaviour
{
    public Transform player;
    public float fadeDuration = 1.5f;
    public string obstructableTag = "Obstructable";
    public string colorPropertyName = "_Color";
    [Range(0f, 1f)]
    public float targetObstructedAlpha = 0.2f;

    private Dictionary<Renderer, Coroutine> activeFades = new();
    private Dictionary<Renderer, float> currentFadeTargets = new();
    private Dictionary<Renderer, Color[]> originalColors = new();
    private HashSet<Renderer> previousHits = new();
    private MaterialPropertyBlock propBlock;

    void Awake()
    {
        propBlock = new MaterialPropertyBlock();
    }

    void LateUpdate()
    {
        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;

        Ray ray = new Ray(transform.position, direction.normalized);
        RaycastHit[] hits = Physics.RaycastAll(ray, distance);

        HashSet<Renderer> currentHits = new();

        foreach (RaycastHit hit in hits)
        {
            if (!hit.collider.CompareTag(obstructableTag)) continue;

            Renderer rend = hit.collider.GetComponent<Renderer>();
            if (rend == null) continue;

            currentHits.Add(rend);

            if (!originalColors.ContainsKey(rend))
            {
                Color[] colors = new Color[rend.sharedMaterials.Length];
                for (int i = 0; i < colors.Length; i++)
                {
                    rend.GetPropertyBlock(propBlock, i);
                    Color col = propBlock.GetColor(colorPropertyName);
                    if (col == default)
                        col = rend.sharedMaterials[i].GetColor(colorPropertyName);
                    colors[i] = col;
                }
                originalColors[rend] = colors;
            }

            if (!currentFadeTargets.ContainsKey(rend) || Mathf.Abs(currentFadeTargets[rend] - targetObstructedAlpha) > 0.01f)
            {
                if (activeFades.TryGetValue(rend, out var running))
                {
                    StopCoroutine(running);
                    activeFades.Remove(rend);
                }

                Coroutine c = StartCoroutine(FadeAlpha(rend, targetObstructedAlpha));
                activeFades[rend] = c;
                currentFadeTargets[rend] = targetObstructedAlpha;
            }
        }

        foreach (Renderer rend in previousHits)
        {
            if (!currentHits.Contains(rend))
            {
                float originalAlpha = originalColors.ContainsKey(rend) ? originalColors[rend][0].a : 1f;

                if (!currentFadeTargets.ContainsKey(rend) || Mathf.Abs(currentFadeTargets[rend] - originalAlpha) > 0.01f)
                {
                    if (activeFades.TryGetValue(rend, out var running))
                    {
                        StopCoroutine(running);
                        activeFades.Remove(rend);
                    }

                    Coroutine c = StartCoroutine(FadeAlpha(rend, originalAlpha));
                    activeFades[rend] = c;
                    currentFadeTargets[rend] = originalAlpha;
                }
            }
        }

        previousHits = currentHits;
    }

    IEnumerator FadeAlpha(Renderer rend, float targetAlpha)
    {
        if (!originalColors.ContainsKey(rend)) yield break;

        int materialCount = rend.sharedMaterials.Length;
        Color[] startColors = new Color[materialCount];

        for (int i = 0; i < materialCount; i++)
        {
            rend.GetPropertyBlock(propBlock, i);
            Color col = propBlock.GetColor(colorPropertyName);
            if (col == default) col = rend.sharedMaterials[i].GetColor(colorPropertyName);
            startColors[i] = col;
        }

        float timer = 0f;
        while (timer < fadeDuration)
        {
            float t = timer / fadeDuration;
            for (int i = 0; i < materialCount; i++)
            {
                Color c = startColors[i];
                c.a = Mathf.Lerp(startColors[i].a, targetAlpha, t);
                propBlock.SetColor(colorPropertyName, c);
                rend.SetPropertyBlock(propBlock, i);
            }
            timer += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < materialCount; i++)
        {
            Color finalColor = startColors[i];
            finalColor.a = targetAlpha;
            propBlock.SetColor(colorPropertyName, finalColor);
            rend.SetPropertyBlock(propBlock, i);
        }

        activeFades.Remove(rend);
    }
}
