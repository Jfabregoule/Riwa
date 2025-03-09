using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeManager : Singleton<VolumeManager>
{
    [SerializeField] private Volume _globalVolume;

    #region SavedSettings

    private VolumeSettings _savedGlobalVolumeSettings;

    private TonemappingSettings _savedTonemappingSettings;
    private BloomSettings _savedBloomSettings;
    private VignetteSettings _savedVignetteSettings;
    private LiftGammaGainSettings _savedLiftGammaGainSettings;
    private ShadowsMidtonesHighlightsSettings _savedShadowsMidtonesHighlightsSettings;
    private ColorCurvesSettings _savedColorCurvesSettings;
    private WhiteBalanceSettings _savedWhiteBalanceSettings;
    private DepthOfFieldSettings _savedDepthOfFieldSettings;
    private MotionBlurSettings _savedMotionBlurSettings;

    #endregion

    public void Start()
    {
        SaveAll();

        _savedGlobalVolumeSettings.tonemappingSettings = _savedTonemappingSettings;
        _savedGlobalVolumeSettings.bloomSettings = _savedBloomSettings;
        _savedGlobalVolumeSettings.vignetteSettings = _savedVignetteSettings;
        _savedGlobalVolumeSettings.liftGammaGainSettings = _savedLiftGammaGainSettings;
        _savedGlobalVolumeSettings.shadowsMidtonesHighlightSettings = _savedShadowsMidtonesHighlightsSettings;
        _savedGlobalVolumeSettings.colorCurvesSettings = _savedColorCurvesSettings;
        _savedGlobalVolumeSettings.whiteBalanceSettings = _savedWhiteBalanceSettings;
        _savedGlobalVolumeSettings.depthOfFieldSettings = _savedDepthOfFieldSettings;
        _savedGlobalVolumeSettings.motionBlurSettings = _savedMotionBlurSettings;
    }

    public void SaveAll()
    {
        SaveTonemapping();
        SaveBloom();
        SaveVignette();
        SaveLiftGammaGain();
        SaveShadowsMidtonesHighlights();
        SaveColorCurves();
        SaveWhiteBalance();
        SaveDepthOfField();
        SaveMotionBlur();
    }

    public void LoadAll(VolumeSettings volumeSettings)
    {
        LoadTonemapping(volumeSettings.tonemappingSettings);
        LoadBloom(volumeSettings.bloomSettings);
        LoadVignette(volumeSettings.vignetteSettings);
        LoadLiftGammaGain(volumeSettings.liftGammaGainSettings);
        LoadShadowsMidtonesHighlights(volumeSettings.shadowsMidtonesHighlightSettings);
        LoadColorCurves(volumeSettings.colorCurvesSettings);
        LoadWhiteBalance(volumeSettings.whiteBalanceSettings);
        LoadDepthOfField(volumeSettings.depthOfFieldSettings);
        LoadMotionBlur(volumeSettings.motionBlurSettings);
    }

    public void LoadAll()
    {
        LoadTonemapping();
        LoadBloom();
        LoadVignette();
        LoadLiftGammaGain();
        LoadShadowsMidtonesHighlights();
        LoadColorCurves();
        LoadWhiteBalance();
        LoadDepthOfField();
        LoadMotionBlur();
    }

    #region Tonemapping

    public void SaveTonemapping()
    {
        if (_globalVolume.profile.TryGet(out Tonemapping currentTonemapping))
        {
            _savedTonemappingSettings = new TonemappingSettings
            {
                active = currentTonemapping.active,
                mode = currentTonemapping.mode.value
            };
        }
    }

    public void LoadTonemapping(TonemappingSettings tonemappingSettings)
    {
        if (_globalVolume.profile.TryGet<Tonemapping>(out Tonemapping tonemapping))
        {
            tonemapping.active = tonemappingSettings.active;
            tonemapping.mode.value = tonemappingSettings.mode;
        }
    }

    public void LoadTonemapping()
    {
        LoadTonemapping(_savedTonemappingSettings);
    }

    #endregion

    #region Bloom

    public void SaveBloom()
    {
        if (_globalVolume.profile.TryGet(out Bloom currentBloom))
        {
            _savedBloomSettings = new BloomSettings
            {
                active = currentBloom.active,
                intensity = currentBloom.intensity.value,
                threshold = currentBloom.threshold.value,
                clamp = currentBloom.clamp.value,
                tint = currentBloom.tint.value,
                highQualityFiltering = currentBloom.highQualityFiltering.value,
                dirtTexture = currentBloom.dirtTexture.value,
                dirtIntensity = currentBloom.dirtIntensity.value
            };
        }
    }

    public void LoadBloom(BloomSettings bloomSettings)
    {
        if (_globalVolume.profile.TryGet<Bloom>(out Bloom bloom))
        {
            bloom.active = bloomSettings.active;
            bloom.intensity.value = bloomSettings.intensity;
            bloom.threshold.value = bloomSettings.threshold;
            bloom.clamp.value = bloomSettings.clamp;
            bloom.tint.value = bloomSettings.tint;
            bloom.highQualityFiltering.value = bloomSettings.highQualityFiltering;
            bloom.dirtTexture.value = bloomSettings.dirtTexture;
            bloom.dirtIntensity.value = bloomSettings.dirtIntensity;
        }
    }

    public void LoadBloom()
    {
        LoadBloom(_savedBloomSettings);
    }

    #endregion

    #region Vignette

    public void SaveVignette()
    {
        if (_globalVolume.profile.TryGet(out Vignette currentVignette))
        {
            _savedVignetteSettings = new VignetteSettings
            {
                active = currentVignette.active,
                color = currentVignette.color.value,
                center = currentVignette.center.value,
                intensity = currentVignette.intensity.value,
                smoothness = currentVignette.smoothness.value,
                rounded = currentVignette.rounded.value
            };
        }
    }

    public void LoadVignette(VignetteSettings vignetteSettings)
    {
        if (_globalVolume.profile.TryGet<Vignette>(out Vignette vignette))
        {
            vignette.active = vignetteSettings.active;
            vignette.color.value = vignetteSettings.color;
            vignette.center.value = vignetteSettings.center;
            vignette.intensity.value = vignetteSettings.intensity;
            vignette.smoothness.value = vignetteSettings.smoothness;
            vignette.rounded.value = vignetteSettings.rounded;
        }
    }

    public void LoadVignette()
    {
        LoadVignette(_savedVignetteSettings);
    }

    #endregion

    #region Lift Gamma Gain

    public void SaveLiftGammaGain()
    {
        if (_globalVolume.profile.TryGet(out LiftGammaGain currentLiftGammaGain))
        {
            _savedLiftGammaGainSettings = new LiftGammaGainSettings
            {
                active = currentLiftGammaGain.active,
                lift = currentLiftGammaGain.lift.value,
                gamma = currentLiftGammaGain.gamma.value,
                gain = currentLiftGammaGain.gain.value
            };
        }
    }

    public void LoadLiftGammaGain(LiftGammaGainSettings liftGammaGainSettings)
    {
        if (_globalVolume.profile.TryGet<LiftGammaGain>(out LiftGammaGain liftGammaGain))
        {
            liftGammaGain.active = liftGammaGainSettings.active;
            liftGammaGain.lift.value = liftGammaGainSettings.lift;
            liftGammaGain.gamma.value = liftGammaGainSettings.gamma;
            liftGammaGain.gain.value = liftGammaGainSettings.gain;
        }
    }

    public void LoadLiftGammaGain()
    {
        LoadLiftGammaGain(_savedLiftGammaGainSettings);
    }

    #endregion

    #region Shadows Midtones Highlights

    public void SaveShadowsMidtonesHighlights()
    {
        if (_globalVolume.profile.TryGet(out ShadowsMidtonesHighlights currentShadowsMidtonesHighlights))
        {
            _savedShadowsMidtonesHighlightsSettings = new ShadowsMidtonesHighlightsSettings
            {
                active = currentShadowsMidtonesHighlights.active,
                shadows = currentShadowsMidtonesHighlights.shadows.value,
                midtones = currentShadowsMidtonesHighlights.midtones.value,
                highlights = currentShadowsMidtonesHighlights.highlights.value,
                shadowsStart = currentShadowsMidtonesHighlights.shadowsStart.value,
                shadowsEnd = currentShadowsMidtonesHighlights.shadowsEnd.value,
                highlightsStart = currentShadowsMidtonesHighlights.highlightsStart.value,
                highlightsEnd = currentShadowsMidtonesHighlights.highlightsEnd.value
            };
        }
    }

    public void LoadShadowsMidtonesHighlights(ShadowsMidtonesHighlightsSettings shadowsMidtonesHighlightsSettings)
    {
        if (_globalVolume.profile.TryGet<ShadowsMidtonesHighlights>(out ShadowsMidtonesHighlights shadowsMidtonesHighlights))
        {
            shadowsMidtonesHighlights.active = shadowsMidtonesHighlightsSettings.active;
            shadowsMidtonesHighlights.shadows.value = shadowsMidtonesHighlightsSettings.shadows;
            shadowsMidtonesHighlights.midtones.value = shadowsMidtonesHighlightsSettings.midtones;
            shadowsMidtonesHighlights.highlights.value = shadowsMidtonesHighlightsSettings.highlights;
            shadowsMidtonesHighlights.shadowsStart.value = shadowsMidtonesHighlightsSettings.shadowsStart;
            shadowsMidtonesHighlights.shadowsEnd.value = shadowsMidtonesHighlightsSettings.shadowsEnd;
            shadowsMidtonesHighlights.highlightsStart.value = shadowsMidtonesHighlightsSettings.highlightsStart;
            shadowsMidtonesHighlights.highlightsEnd.value = shadowsMidtonesHighlightsSettings.highlightsEnd;
        }
    }

    public void LoadShadowsMidtonesHighlights()
    {
        LoadShadowsMidtonesHighlights(_savedShadowsMidtonesHighlightsSettings);
    }

    #endregion

    #region Color Curves

    public void SaveColorCurves()
    {
        if (_globalVolume.profile.TryGet(out ColorCurves currentColorCurves))
        {
            _savedColorCurvesSettings = new ColorCurvesSettings
            {
                active = currentColorCurves.active,
                red = currentColorCurves.red.value,
                green = currentColorCurves.green.value,
                blue = currentColorCurves.blue.value,
                hueVsHue = currentColorCurves.hueVsHue.value,
                hueVsSat = currentColorCurves.hueVsSat.value,
                satVsSat = currentColorCurves.satVsSat.value,
                lumVsSat = currentColorCurves.lumVsSat.value
            };
        }
    }

    public void LoadColorCurves(ColorCurvesSettings colorCurvesSettings)
    {
        if (_globalVolume.profile.TryGet<ColorCurves>(out ColorCurves colorCurves))
        {
            colorCurves.active = colorCurvesSettings.active;
            colorCurves.red.value = colorCurvesSettings.red;
            colorCurves.green.value = colorCurvesSettings.green;
            colorCurves.blue.value = colorCurvesSettings.blue;
            colorCurves.hueVsHue.value = colorCurvesSettings.hueVsHue;
            colorCurves.hueVsSat.value = colorCurvesSettings.hueVsSat;
            colorCurves.satVsSat.value = colorCurvesSettings.satVsSat;
            colorCurves.lumVsSat.value = colorCurvesSettings.lumVsSat;
        }
    }

    public void LoadColorCurves()
    {
        LoadColorCurves(_savedColorCurvesSettings);
    }

    #endregion

    #region White Balance

    public void SaveWhiteBalance()
    {
        if (_globalVolume.profile.TryGet(out WhiteBalance currentWhiteBalance))
        {
            _savedWhiteBalanceSettings = new WhiteBalanceSettings
            {
                active = currentWhiteBalance.active,
                temperature = currentWhiteBalance.temperature.value,
                tint = currentWhiteBalance.tint.value
            };
        }
    }

    public void LoadWhiteBalance(WhiteBalanceSettings whiteBalanceSettings)
    {
        if (_globalVolume.profile.TryGet<WhiteBalance>(out WhiteBalance whiteBalance))
        {
            whiteBalance.active = whiteBalanceSettings.active;
            whiteBalance.temperature.value = whiteBalanceSettings.temperature;
            whiteBalance.tint.value = whiteBalanceSettings.tint;
        }
    }

    public void LoadWhiteBalance()
    {
        LoadWhiteBalance(_savedWhiteBalanceSettings);
    }

    #endregion

    #region DepthOfField

    public void SaveDepthOfField()
    {
        if (_globalVolume.profile.TryGet(out DepthOfField currentDepthOfField))
        {
            _savedDepthOfFieldSettings = new DepthOfFieldSettings
            {
                active = currentDepthOfField.active,
                mode = currentDepthOfField.mode.value,
                focusDistance = currentDepthOfField.focusDistance.value,
                gaussianStart = currentDepthOfField.gaussianStart.value,
                gaussianEnd = currentDepthOfField.gaussianEnd.value,
                gaussianMaxRadius = currentDepthOfField.gaussianMaxRadius.value,
                highQualitySampling = currentDepthOfField.highQualitySampling.value
            };
        }
    }

    public void LoadDepthOfField(DepthOfFieldSettings depthOfFieldSettings)
    {
        if (_globalVolume.profile.TryGet<DepthOfField>(out DepthOfField depthOfField))
        {
            depthOfField.active = depthOfFieldSettings.active;
            depthOfField.mode.value = depthOfFieldSettings.mode;
            depthOfField.focusDistance.value = depthOfFieldSettings.focusDistance;
            depthOfField.gaussianStart.value = depthOfFieldSettings.gaussianStart;
            depthOfField.gaussianEnd.value = depthOfFieldSettings.gaussianEnd;
            depthOfField.gaussianMaxRadius.value = depthOfFieldSettings.gaussianMaxRadius;
            depthOfField.highQualitySampling.value = depthOfFieldSettings.highQualitySampling;
        }
    }

    public void LoadDepthOfField()
    {
        LoadDepthOfField(_savedDepthOfFieldSettings);
    }

    public void Blur()
    {
        if (_globalVolume.profile.TryGet<DepthOfField>(out DepthOfField depthOfField))
        {
            depthOfField.active = true;
            depthOfField.mode.value = DepthOfFieldMode.Bokeh;
            depthOfField.focusDistance.value = 0.1f;
            depthOfField.highQualitySampling.value = true;
        }
    }

    public void UnBlur()
    {
        LoadDepthOfField();
    }

    #endregion

    #region Motion Blur

    public void SaveMotionBlur()
    {
        if (_globalVolume.profile.TryGet(out MotionBlur currentMotionBlur))
        {
            _savedMotionBlurSettings = new MotionBlurSettings
            {
                active = currentMotionBlur.active,
                intensity = currentMotionBlur.intensity.value,
                clamp = currentMotionBlur.clamp.value
            };
        }
    }

    public void LoadMotionBlur(MotionBlurSettings motionBlurSettings)
    {
        if (_globalVolume.profile.TryGet<MotionBlur>(out MotionBlur motionBlur))
        {
            motionBlur.active = motionBlurSettings.active;
            motionBlur.intensity.value = motionBlurSettings.intensity;
            motionBlur.clamp.value = motionBlurSettings.clamp;
        }
    }

    public void LoadMotionBlur()
    {
        LoadMotionBlur(_savedMotionBlurSettings);
    }

    #endregion
}
