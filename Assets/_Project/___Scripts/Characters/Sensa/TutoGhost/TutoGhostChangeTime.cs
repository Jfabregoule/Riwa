using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoGhostChangeTime : MonoBehaviour
{

    [SerializeField] private Vector3 _startPos;
    [SerializeField] private Vector3 _endPos;
    private Animator _animator;
    [SerializeField] private Material[] _materials;
    [SerializeField] private float _fadeTime;
    [SerializeField] private float _moveTime;
    [SerializeField] private ParticleSystem _ChangeTimeVFX;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        for (int i = 0; i < _materials.Length; i++)
        {
            UnityEngine.Color color = _materials[i].color;
            color.a = 0;
            _materials[i].color = color;
        }
        StartCoroutine(FadeAlpha(0.6f));
        StartCoroutine(Move());
    }



    private IEnumerator Move()
    {
        yield return new WaitForSeconds(_fadeTime);
        _animator.SetBool("Moving", true);
        float currentTime = 0f;
        while (currentTime < _moveTime)
        {
            currentTime += Time.deltaTime;
            float alpha = currentTime / _moveTime;
            alpha = Mathf.Clamp01(alpha);
            transform.position = Vector3.Lerp(_startPos, _endPos, alpha);

            yield return null;

        }
        _animator.SetBool("Moving", false);
        yield return new WaitForSeconds(0.5f);
        _ChangeTimeVFX.Play();
        yield return new WaitForSeconds(1f);
        StartCoroutine(FadeAlpha(0));
        StartCoroutine(ResetPos());

    }

    private IEnumerator ResetPos()
    {
        _animator.SetBool("Moving", false);
        yield return new WaitForSeconds(_fadeTime);
        transform.position = _startPos;
        StartCoroutine(FadeAlpha(0.6f));
        StartCoroutine(Move());

    }

    private IEnumerator FadeAlpha(float newAlpha)
    {

        float time = 0;

        UnityEngine.Color[] startColors = new UnityEngine.Color[_materials.Length];
        for (int i = 0; i < _materials.Length; i++)
        {
            startColors[i] = _materials[i].color;
        }

        while (time < _fadeTime)
        {
            time += Time.deltaTime;
            float t = time / _fadeTime;
            t = Mathf.Clamp01(t);
            for (int j = 0; j < startColors.Length; j++)
            {
                UnityEngine.Color c = startColors[j];
                c.a = Mathf.Lerp(c.a, newAlpha, t);

                _materials[j].color = c;
            }
            yield return null;
        }

        for (int i = 0; i < _materials.Length; i++)
        {
            UnityEngine.Color color = _materials[i].color;
            color.a = newAlpha;
            _materials[i].color = color;
        }

    }
}
