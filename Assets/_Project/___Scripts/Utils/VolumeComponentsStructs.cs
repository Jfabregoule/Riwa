
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

#region Structs

    [System.Serializable]
    public struct VolumeSettings
    {
        public TonemappingSettings tonemappingSettings;
        public BloomSettings bloomSettings;
        public VignetteSettings vignetteSettings;
        public LiftGammaGainSettings liftGammaGainSettings;
        public ShadowsMidtonesHighlightsSettings shadowsMidtonesHighlightSettings;
        public ColorCurvesSettings colorCurvesSettings;
        public WhiteBalanceSettings whiteBalanceSettings;
        public DepthOfFieldSettings depthOfFieldSettings;
        public MotionBlurSettings motionBlurSettings;
    }

    [System.Serializable]
    public struct TonemappingSettings
    {
        public bool active;
        public TonemappingMode mode;
    }

    [System.Serializable]
    public struct BloomSettings
    {
        public bool active;
        public float intensity;
        public float threshold;
        public float clamp;
        public Color tint;
        public bool highQualityFiltering;
        public Texture dirtTexture;
        public float dirtIntensity;
    }

    [System.Serializable]
    public struct VignetteSettings
    {
        public bool active;
        public Color color;
        public Vector2 center;
        public float intensity;
        public float smoothness;
        public bool rounded;
    }

    [System.Serializable]
    public struct LiftGammaGainSettings
    {
        public bool active;
        public Vector4 lift;
        public Vector4 gamma;
        public Vector4 gain;
    }

    [System.Serializable]
    public struct ShadowsMidtonesHighlightsSettings
    {
        public bool active;
        public Vector4 shadows;
        public Vector4 midtones;
        public Vector4 highlights;
        public float shadowsStart;
        public float shadowsEnd;
        public float highlightsStart;
        public float highlightsEnd;
    }

    [System.Serializable]
    public struct ColorCurvesSettings
    {
        public bool active;
        public TextureCurve red;
        public TextureCurve green;
        public TextureCurve blue;
        public TextureCurve hueVsHue;
        public TextureCurve hueVsSat;
        public TextureCurve satVsSat;
        public TextureCurve lumVsSat;
    }

    [System.Serializable]
    public struct WhiteBalanceSettings
    {
        public bool active;
        public float temperature;
        public float tint;
    }

    [System.Serializable]
    public struct DepthOfFieldSettings
    {
        public bool active;
        public DepthOfFieldMode mode;
        public float focusDistance;
        public float gaussianStart;
        public float gaussianEnd;
        public float gaussianMaxRadius;
        public bool highQualitySampling;
    }

    [System.Serializable]
    public struct MotionBlurSettings
    {
        public bool active;
        public float intensity;
        public float clamp;
    }

    #endregion
