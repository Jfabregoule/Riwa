using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DynamicResolutionSystem : MonoBehaviour
{
    [SerializeField] private float _checkInterval = 2f;
    [SerializeField] private float _targetFPS = 60f;
    [SerializeField] private float _scaleStep = 0.1f;
    [SerializeField] private float _minRenderScale = 0.5f;
    [SerializeField] private float _maxRenderScale = 1.0f;

    private float _timeElapsed = 0f;
    private int _framesCounted = 0;

    private UniversalRenderPipelineAsset urpAsset;

    void Start()
    {
        Application.targetFrameRate = (int)_targetFPS + 10;
        urpAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;

        if (urpAsset == null)
        {
            Debug.LogError("Not using URP.");
            enabled = false;
        }
    }

    void Update()
    {
        _timeElapsed += Time.unscaledDeltaTime;
        _framesCounted++;

        if (_timeElapsed >= _checkInterval)
        {
            float avgFPS = _framesCounted / _timeElapsed;

            AdjustRenderScale(avgFPS);

            _timeElapsed = 0f;
            _framesCounted = 0;
        }
    }

    void AdjustRenderScale(float avgFPS)
    {
        float currentScale = urpAsset.renderScale;

        if (avgFPS < _targetFPS - 5f && currentScale > _minRenderScale)
        {
            currentScale = Mathf.Max(_minRenderScale, currentScale - _scaleStep);
            urpAsset.renderScale = currentScale;
        }
        else if (avgFPS > _targetFPS + 5f && currentScale < _maxRenderScale)
        {
            currentScale = Mathf.Min(_maxRenderScale, currentScale + _scaleStep);
            urpAsset.renderScale = currentScale;
        }
    }
}
