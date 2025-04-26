using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.Audio;

[DefaultExecutionOrder(-1)]
public class CinematicSystem<T> : Singleton<T> where T : CinematicSystem<T>
{
    private const string CINEMATIC_TAG = "Cinematic";

    [Header("Video Settings")]
    [SerializeField] private string _videoFolderPath = "Cinematics";
    [SerializeField] private RenderTexture _targetTexture;

    [Header("Events")]
    public UnityEvent OnVideoStarted;
    public UnityEvent OnVideoEnded;

    [Header("Audio Settigs")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private List<string> _audioMixerGroupName;

    private CanvasGroup _cinematicCanvasGroup;
    private VideoPlayer _videoPlayer;
    private Dictionary<string, VideoClip> _videoClips = new Dictionary<string, VideoClip>();
    private Action _onEndCallback;

    private CanvasGroup _skipCanvasGroup;
    private HoldButton _holdButton;

    private float _appearTimer = 2.0f;

    protected override void Awake()
    {
        base.Awake();
        LoadAllVideos();
    }

    private void Start()
    {
        _videoPlayer = GetComponent<VideoPlayer>();

        if (_videoPlayer == null)
        {
            _videoPlayer = gameObject.AddComponent<VideoPlayer>();
        }

        InitializePlayer();
    }

    private void InitializePlayer()
    {
        _videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        _videoPlayer.targetTexture = _targetTexture;
        _videoPlayer.loopPointReached += OnVideoFinished;
    }

    private void LoadAllVideos()
    {
        VideoClip[] clips = Resources.LoadAll<VideoClip>(_videoFolderPath);
        foreach (VideoClip clip in clips)
        {
            _videoClips[clip.name] = clip;
        }
    }

    public void PlayVideoByKey(string key, bool loop = false, Action onEnd = null)
    {
        if (!_cinematicCanvasGroup) return;
        Helpers.EnabledCanvasGroup(_cinematicCanvasGroup);
        if (_videoClips.TryGetValue(key, out VideoClip clip))
        {
            _videoPlayer.clip = clip;
            _videoPlayer.isLooping = loop;
            _onEndCallback = onEnd;
            SetVolume(true);
            _videoPlayer.Play();
            OnVideoStarted?.Invoke();
            StartCoroutine(WaitBeforeAppear());
        }
        else
        {
            Debug.LogWarning($"Video with key '{key}' not found.");
        }
    }


    private void OnVideoFinished(VideoPlayer vp)
    {
        Helpers.DisabledCanvasGroup(_cinematicCanvasGroup);
        SetVolume(false);
        OnVideoEnded?.Invoke();
        _onEndCallback?.Invoke();
        _onEndCallback = null;
    }

    public void StopVideo()
    {
        if (_videoPlayer.isPlaying)
        {
            _videoPlayer.Stop();
            SetVolume(false);
            
            OnVideoEnded?.Invoke();
            _onEndCallback?.Invoke();
            _onEndCallback = null;
        }
    }

    public void SetCanvasGroup()
    {
        _cinematicCanvasGroup = GameObject.FindGameObjectWithTag(CINEMATIC_TAG).GetComponent<CanvasGroup>();
        _holdButton = _cinematicCanvasGroup.gameObject.transform.GetChild(3).GetComponent<HoldButton>();
        _skipCanvasGroup = _cinematicCanvasGroup.gameObject.transform.transform.GetChild(3).GetComponent<CanvasGroup>();
        _holdButton.OnHoldComplete += SkipCinematic;
    }
    public bool IsPlaying() => _videoPlayer.isPlaying;

    private void SkipCinematic()
    {
        _videoPlayer.Stop();
        OnVideoFinished(_videoPlayer);
    }

    private IEnumerator WaitBeforeAppear()
    {
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(AppearButton());

    }

    private IEnumerator AppearButton()
    {
        float timer = 0;
        while (timer < _appearTimer)
        {
            timer += Time.deltaTime;
            _skipCanvasGroup.alpha = Mathf.Clamp01(timer / _appearTimer);
            yield return null;
        }
        Helpers.EnabledCanvasGroup(_skipCanvasGroup);
    }

    private void SetVolume(bool isVideoEnable)
    {
        if (isVideoEnable)
        {
            for (int i = 0; i < _audioMixerGroupName.Count; i++)
            {
                _audioMixer.SetFloat(_audioMixerGroupName[i], -80);
            }
        }
        else
        {
            for (int i = 0; i < _audioMixerGroupName.Count; i++)
            {
                _audioMixer.SetFloat(_audioMixerGroupName[i], SaveSystem.Instance.LoadElement<float>(_audioMixerGroupName[i], true));
            }
        }
    }
}
