using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

[DefaultExecutionOrder(-1)]
public class CinematicSystem<T> : Singleton<T> where T : CinematicSystem<T>
{
    [Header("Video Settings")]
    [SerializeField] private string _videoFolderPath = "Cinematics";
    [SerializeField] private RenderTexture _targetTexture;
    private AudioSource _audioSource;

    [Header("Events")]
    public UnityEvent OnVideoStarted;
    public UnityEvent OnVideoEnded;

    private VideoPlayer _videoPlayer;
    private Dictionary<string, VideoClip> _videoClips = new Dictionary<string, VideoClip>();
    private Action _onEndCallback;

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

        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        InitializePlayer();
    }

    private void InitializePlayer()
    {
        _videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        _videoPlayer.targetTexture = _targetTexture;
        _videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        _videoPlayer.SetTargetAudioSource(0, _audioSource);
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
        if (_videoClips.TryGetValue(key, out VideoClip clip))
        {
            _videoPlayer.clip = clip;
            _videoPlayer.isLooping = loop;
            _onEndCallback = onEnd;

            _videoPlayer.Play();
            OnVideoStarted?.Invoke();
        }
        else
        {
            Debug.LogWarning($"Video with key '{key}' not found.");
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        OnVideoEnded?.Invoke();
        _onEndCallback?.Invoke();
        _onEndCallback = null;
    }

    public void StopVideo()
    {
        if (_videoPlayer.isPlaying)
        {
            _videoPlayer.Stop();
            OnVideoEnded?.Invoke();
            _onEndCallback?.Invoke();
            _onEndCallback = null;
        }
    }

    public bool IsPlaying() => _videoPlayer.isPlaying;
}
