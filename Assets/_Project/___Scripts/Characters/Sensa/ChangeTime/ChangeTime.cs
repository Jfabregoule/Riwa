using System.Collections;
using UnityEngine;
using Cinemachine;

[ExecuteInEditMode]
public class ChangeTime : MonoBehaviour
{
    #region Fields

    private float _radius;
    private float _alpha;
    private bool _particleActivated;
    private int _past;
    private int _present;

    [Header("Time Configuration")]
    [SerializeField] private float _baseRadius = 0f;
    [SerializeField] private float _maxRadius = 50f;
    [SerializeField] private float _cancelRadius = 50f;

    [Header("Particle Systems")]
    [SerializeField] private ParticleSystem _sphere;
    [SerializeField] private ParticleSystem _sphereElectricity;
    [SerializeField] private ParticleSystem _ambianceParticles;
    [SerializeField] private ParticleSystem _flash;
    [SerializeField] private ParticleSystem _cancelSphere;

    [Header("Cinemachine Impulse")]
    [SerializeField] private CinemachineImpulseSource _impulseSource;

    private ParticleSystem.ShapeModule _shapeModule;
    private ParticleSystem.EmissionModule _emissionModule;
    private ParticleSystem.ShapeModule _ambianceShapeModule;
    private ParticleSystem.EmissionModule _ambianceEmissionModule;

    public delegate void TimeEvent(bool isPast);
    public event TimeEvent OnTimeChangeEnd;

    #endregion

    #region Methods

    private void Start()
    {
        _shapeModule = _sphereElectricity.shape;
        _emissionModule = _sphereElectricity.emission;
        _ambianceShapeModule = _ambianceParticles.shape;
        _ambianceEmissionModule = _ambianceParticles.emission;
        _radius = 0;

        Shader.SetGlobalFloat("_Radius", _radius);
        Shader.SetGlobalInt("_PresentEnum", 1);
        Shader.SetGlobalInt("_PastEnum", 0);
        _present = Shader.GetGlobalInt("_PresentEnum");
        _past = Shader.GetGlobalInt("_PastEnum");
    }

    public void AbortChangeTime()
    {
        StartCoroutine(AbortTimeChangeCoroutine());
    }

    private IEnumerator AbortTimeChangeCoroutine()
    {
        if (!_particleActivated)
        {
            _cancelSphere.Play();
            //_sphereElectricity.Play();
            _ambianceParticles.Play();
            _flash.Play();
            _impulseSource.GenerateImpulse();
            _particleActivated = true;
        }

        _alpha = 0f;
        while (_alpha < 0.5f)
        {
            _alpha += Time.deltaTime / 1.5f;
            _alpha = Mathf.Clamp01(_alpha);
            _radius = Mathf.Lerp(_baseRadius, _cancelRadius, _alpha);

            _shapeModule.radius = _radius;
            _ambianceShapeModule.radius = _radius;
            _emissionModule.rateOverTimeMultiplier = Mathf.Lerp(2000, 10000, _alpha);
            _ambianceEmissionModule.rateOverTimeMultiplier = Mathf.Lerp(20, 40, _alpha);

            Shader.SetGlobalVector("_Position", transform.position);
            Shader.SetGlobalFloat("_Radius", _radius);

            yield return null;
        }

        while (_alpha > 0f)
        {
            _alpha -= Time.deltaTime / 2f;
            _alpha = Mathf.Clamp01(_alpha);
            _radius = Mathf.Lerp(_baseRadius, _cancelRadius, _alpha);

            _shapeModule.radius = _radius;
            _ambianceShapeModule.radius = _radius;
            _emissionModule.rateOverTimeMultiplier = Mathf.Lerp(2000, 10000, _alpha);
            _ambianceEmissionModule.rateOverTimeMultiplier = Mathf.Lerp(20, 40, _alpha);

            Shader.SetGlobalVector("_Position", transform.position);
            Shader.SetGlobalFloat("_Radius", _radius);

            yield return null;
        }

        _particleActivated = false;
        OnTimeChangeEnd?.Invoke(_past == 1);
    }

    public void StartTimeChange()
    {
        StartCoroutine(ProcessTimeChangeCoroutine());
    }

    private IEnumerator ProcessTimeChangeCoroutine()
    {
        if (!_particleActivated)
        {
            _sphere.Play();
            _sphereElectricity.Play();
            _ambianceParticles.Play();
            _flash.Play();
            _impulseSource.GenerateImpulse();
            _particleActivated = true;
        }

        _alpha = 0f;
        while (_alpha < 1f)
        {
            _alpha += Time.deltaTime / 4f;
            _alpha = Mathf.Clamp01(_alpha);
            _radius = Mathf.Lerp(_baseRadius, _maxRadius, _alpha);

            _shapeModule.radius = _radius;
            _ambianceShapeModule.radius = _radius;
            _emissionModule.rateOverTimeMultiplier = Mathf.Lerp(2000, 10000, _alpha);
            _ambianceEmissionModule.rateOverTimeMultiplier = Mathf.Lerp(20, 40, _alpha);

            Shader.SetGlobalVector("_Position", transform.position);
            Shader.SetGlobalFloat("_Radius", _radius);

            yield return null;
        }

        UpdateShaders();
        OnTimeChangeEnd?.Invoke(_past == 1);
    }

    private void UpdateShaders()
    {
        Shader.SetGlobalInt("_PresentEnum", 1 - _present);
        Shader.SetGlobalInt("_PastEnum", 1 - _past);
        _past = 1 - _past;
        _present = 1 - _present;
        _radius = 0;
        Shader.SetGlobalFloat("_Radius", _radius);
        _particleActivated = false;
        _alpha = 0;

    }

    #endregion
}