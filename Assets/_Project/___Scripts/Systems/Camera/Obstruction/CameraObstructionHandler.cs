using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraObstructionHandler : MonoBehaviour
{

    [SerializeField] private float _fadeDuration = 1.5f;
    [SerializeField] private string _colorPropertyName = "_Color";

    [Range(0f, 1f)]
    [SerializeField] private float _targetObstructedAlpha = 0.0f;

    private Transform _soul;

    private Dictionary<Renderer, Coroutine> _activeFades = new();
    private Dictionary<Renderer, float> _currentFadeTargets = new();
    private Dictionary<Renderer, Color[]> _originalColors = new();
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
                        for (int i = 0; i < colors.Length; i++)
                        {
                            rend.GetPropertyBlock(_propBlock, i);
                            Color col = _propBlock.GetColor(_colorPropertyName);
                            if (col == default)
                                col = rend.sharedMaterials[i].GetColor(_colorPropertyName);
                            colors[i] = col;
                        }
                        _originalColors[rend] = colors;
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

        int materialCount = rend.sharedMaterials.Length;
        Color[] startColors = new Color[materialCount];

        for (int i = 0; i < materialCount; i++)
        {
            rend.GetPropertyBlock(_propBlock, i);
            Color col = _propBlock.GetColor(_colorPropertyName);
            if (col == default) col = rend.sharedMaterials[i].GetColor(_colorPropertyName);
            startColors[i] = col;
        }

        float timer = 0f;
        while (timer < _fadeDuration)
        {
            float t = timer / _fadeDuration;
            for (int i = 0; i < materialCount; i++)
            {
                Color c = startColors[i];
                c.a = Mathf.Lerp(startColors[i].a, targetAlpha, t);
                _propBlock.SetColor(_colorPropertyName, c);
                rend.SetPropertyBlock(_propBlock, i);
            }
            timer += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < materialCount; i++)
        {
            Color finalColor = startColors[i];
            finalColor.a = targetAlpha;
            _propBlock.SetColor(_colorPropertyName, finalColor);
            rend.SetPropertyBlock(_propBlock, i);
        }

        _activeFades.Remove(rend);
    }
}
