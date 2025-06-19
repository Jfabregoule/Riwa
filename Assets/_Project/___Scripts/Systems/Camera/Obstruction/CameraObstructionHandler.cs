using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraObstructionHandler : MonoBehaviour
{

    [SerializeField] private float _fadeDuration = 1.5f;
    [SerializeField] private string _colorPropertyName = "_Color";
    [SerializeField] private string _emissiveColorPropertyName = "_EmissionColor";

    private float _targetObstructedAlpha = 0.15f;

    private Transform _soul;

    private Dictionary<Renderer, Coroutine> _activeFades = new();
    private Dictionary<Renderer, float> _currentFadeTargets = new();
    private Dictionary<Renderer, Color[]> _originalColors = new();
    private Dictionary<Renderer, Color[]> _originalEmissiveColors = new();
    private HashSet<Renderer> _previousHits = new();
    private MaterialPropertyBlock _propBlock;

    void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
    }

    private void Start()
    {
        _soul = GameManager.Instance.Character.Soul.transform;
    }

    void LateUpdate()
    {
        Vector3 direction = _soul.position - transform.position;
        float distance = direction.magnitude;

        Ray ray = new Ray(transform.position, direction.normalized);
        RaycastHit[] hits = Physics.RaycastAll(ray, distance);

        HashSet<Renderer> currentHits = new();

        foreach (RaycastHit hit in hits)
        {
            ObstructionGroup group = hit.collider.GetComponentInParent<ObstructionGroup>();

            if (group == null) continue;

            _targetObstructedAlpha = group._targetObstructedAlpha;

            foreach (GameObject obj in group.objectsToFade)
            {
                if (obj == null) continue;

                Renderer[] renderers = obj.GetComponentsInChildren<Renderer>(true);
                foreach (Renderer rend in renderers)
                {
                    currentHits.Add(rend);
                    if (!_originalColors.ContainsKey(rend))
                    {
                        Color[] colors = new Color[rend.sharedMaterials.Length];
                        Color[] emissiveColors = new Color[rend.sharedMaterials.Length];
                        for (int i = 0; i < colors.Length; i++)
                        {
                            rend.GetPropertyBlock(_propBlock, i);
                            Color col = _propBlock.GetColor(_colorPropertyName);
                            Color eCol = _propBlock.GetColor(_emissiveColorPropertyName);
                            if (col == default)
                            {
                                col = rend.sharedMaterials[i].GetColor(_colorPropertyName);
                                eCol = rend.sharedMaterials[i].GetColor(_emissiveColorPropertyName);
                            }
                                
                            colors[i] = col;
                            emissiveColors[i] = eCol;
                        }
                        _originalColors[rend] = colors;
                        _originalEmissiveColors[rend] = emissiveColors;
                    }

                    if (!_currentFadeTargets.ContainsKey(rend) || Mathf.Abs(_currentFadeTargets[rend] - _targetObstructedAlpha) > 0.01f)
                    {
                        if (_activeFades.TryGetValue(rend, out var running))
                        {
                            StopCoroutine(running);
                            _activeFades.Remove(rend);
                        }

                        Coroutine c = StartCoroutine(FadeAlpha(rend, _targetObstructedAlpha));
                        _activeFades[rend] = c;
                        _currentFadeTargets[rend] = _targetObstructedAlpha;
                    }
                }
            }
        }

        foreach (Renderer rend in _previousHits)
        {
            if (!currentHits.Contains(rend))
            {
                float originalAlpha = _originalColors.ContainsKey(rend) ? _originalColors[rend][0].a : 1f;

                if (!_currentFadeTargets.ContainsKey(rend) || Mathf.Abs(_currentFadeTargets[rend] - originalAlpha) > 0.01f)
                {
                    if (_activeFades.TryGetValue(rend, out var running))
                    {
                        StopCoroutine(running);
                        _activeFades.Remove(rend);
                    }
                    
                    Coroutine c = StartCoroutine(FadeAlpha(rend, originalAlpha));
                    _activeFades[rend] = c;
                    _currentFadeTargets[rend] = originalAlpha;
                }
            }
        }

        _previousHits = currentHits;
    }

    IEnumerator FadeAlpha(Renderer rend, float targetAlpha)
    {
        if (!_originalColors.ContainsKey(rend)) yield break;
        if (!_originalEmissiveColors.ContainsKey(rend)) yield break;

        int materialCount = rend.materials.Length;
        Color[] startColors = new Color[materialCount];
        Color[] startEColors = new Color[materialCount];

        Material[] materials = rend.materials;

        for (int i = 0; i < materialCount; i++)
        {
            rend.GetPropertyBlock(_propBlock, i);
            Color currentColor = _propBlock.GetColor(_colorPropertyName);
            if (currentColor == default)
                currentColor = materials[i].GetColor(_colorPropertyName);
            startColors[i] = currentColor;

            Color currentEColor = materials[i].GetColor(_emissiveColorPropertyName);
            startEColors[i] = currentEColor;

            // Activer l’émission si besoin
            if (!materials[i].IsKeywordEnabled("_EMISSION"))
                materials[i].EnableKeyword("_EMISSION");
        }

        // Déterminer les couleurs cibles
        Color[] targetColors = new Color[materialCount];
        Color[] targetEColors = new Color[materialCount];

        for (int i = 0; i < materialCount; i++)
        {
            targetColors[i] = _originalColors[rend][i];
            targetColors[i].a = targetAlpha;

            targetEColors[i] = targetAlpha < 1f
                ? Color.black // Fade out : vers noir
                : _originalEmissiveColors[rend][i]; // Fade in : vers couleur originale
        }

        float timer = 0f;
        while (timer < _fadeDuration)
        {
            float t = timer / _fadeDuration;
            for (int i = 0; i < materialCount; i++)
            {
                Color col = Color.Lerp(startColors[i], targetColors[i], t);
                Color eCol = Color.Lerp(startEColors[i], targetEColors[i], t);

                _propBlock.SetColor(_colorPropertyName, col);
                rend.SetPropertyBlock(_propBlock, i);

                materials[i].SetColor(_emissiveColorPropertyName, eCol);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < materialCount; i++)
        {
            _propBlock.SetColor(_colorPropertyName, targetColors[i]);
            rend.SetPropertyBlock(_propBlock, i);
            materials[i].SetColor(_emissiveColorPropertyName, targetEColors[i]);
        }

        _activeFades.Remove(rend);
    }
}
