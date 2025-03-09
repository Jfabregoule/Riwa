using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-2)]
public class SoundSystem : Singleton<SoundSystem>
{

    #region Classes
    public class SoundFX
    {
        public string key;
        public List<AudioClip> clip;
    }

    public class Track
    {
        public string key;
        public AudioClip clip;
    }

    #endregion

    #region Fields

    [SerializeField] private string SFXFolderPath;
    [SerializeField] private string musicFolderPath;
    [SerializeField] private string ambianceFolderPath;

    private int _numberOfChannels;

    [SerializeField] private AudioMixerGroup _musicMixerGroup;
    [SerializeField] private AudioMixerGroup _ambianceMixerGroup;
    private AudioListener _audioListener;

    [SerializeField] private AudioClip _startingMusic;
    [SerializeField] private AudioClip _startingAmbianceSound;

    [SerializeField] private float _fadeInDuration;
    [SerializeField] private float _fadeOutDuration;

    private List<AudioSource> _audioSources;
    private AudioSource _currentMusicSource;
    private List<AudioSource> _currentAmbianceSources;

    [SerializeField] private AudioSource soundFXObject;

    private GameObject _player;

       

    private List<SoundFX> _SFXList = new List<SoundFX>();
    private List<Track> _musicList = new List<Track>();
    private List<Track> _ambianceList = new List<Track>();

    #endregion

    #region Main Functions

    protected override void Awake() {
        base.Awake();

        GenerateKeys();
        _audioSources = new List<AudioSource>();
        _currentMusicSource = null;
        _currentAmbianceSources = new List<AudioSource>();
        _numberOfChannels = GetComponents<AudioSource>().Length;

        AudioSource[] attachedAudioSources = GetComponents<AudioSource>();

        for (int i = 0; i < _numberOfChannels; i++) {
            _audioSources.Add(attachedAudioSources[i]);
        }

        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void GenerateKeys()
    {
        GenerateSFXKeys();
        GenerateMusicKeys();
        GenerateAmbianceKeys();
    }

    private void GenerateSFXKeys()
    {
        AudioClip[] audioClips = Resources.LoadAll<AudioClip>(SFXFolderPath);

        foreach (AudioClip audioClip in audioClips)
        {
            string[] words = audioClip.name.Split('_');

            if (words[0] != "SFX" || words.Length < 3)
            {
                Debug.LogWarning($"The audio clip {audioClip.name} has not the SFX_xxx_xxx format.");
            }

            string key = "";

            for (int i = 1; i < words.Length; i++)
            {
                key += char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower() + " ";
            }

            key = key.Trim();

            int index = key.Length - 1;

            while (char.IsDigit(key[index]))
                index--;

            key = key.Substring(0, index + 1);

            SoundFX existingSound = _SFXList.Find(sound => sound.key == key);

            if (existingSound != null)
            {
                existingSound.clip.Add(audioClip);
            }
            else
            {
                SoundFX newSound = new SoundFX
                {
                    key = key,
                    clip = new List<AudioClip> { audioClip }
                };
                _SFXList.Add(newSound);
            }
        }
    }

    private void GenerateMusicKeys()
    {
        AudioClip[] audioClips = Resources.LoadAll<AudioClip>(musicFolderPath);

        foreach (AudioClip audioClip in audioClips)
        {
            string[] words = audioClip.name.Split('_');

            if (words[0] != "MUSIC" || words.Length < 2)
            {
                Debug.LogWarning($"The audio clip {audioClip.name} has not the MUSIC_xxx format.");
            }

            string key = "";

            for (int i = 1; i < words.Length; i++)
            {
                key += char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower() + " ";
            }

            key = key.Trim();

            int index = key.Length - 1;

            while (char.IsDigit(key[index]))
                index--;

            key = key.Substring(0, index + 1);

                
            Track newTrack = new Track
            {
                key = key,
                clip = audioClip
            };

            _musicList.Add(newTrack);
        }
    }

    private void GenerateAmbianceKeys()
    {
        AudioClip[] audioClips = Resources.LoadAll<AudioClip>(ambianceFolderPath);

        foreach (AudioClip audioClip in audioClips)
        {
            string[] words = audioClip.name.Split('_');

            if (words[0] != "AMBIANCE" || words.Length < 2)
            {
                Debug.LogWarning($"The audio clip {audioClip.name} has not the AMBIANCE_xxx format.");
            }

            string key = "";

            for (int i = 1; i < words.Length; i++)
            {
                key += char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower() + " ";
            }

            key = key.Trim();

            int index = key.Length - 1;

            while (char.IsDigit(key[index]))
                index--;

            key = key.Substring(0, index + 1);


            Track newTrack = new Track
            {
                key = key,
                clip = audioClip
            };

            _ambianceList.Add(newTrack);
        }
    }

    public void SetAudioListener(AudioListener audioListener)
    {
        _audioListener = audioListener;
    }

    public void SetPlayer(GameObject player)
    {
        _player = player;
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            if (!audioSource.isPlaying)
            {
                return audioSource;
            }
        }

        _audioSources[0].Stop();
        return _audioSources[0];
    }

    private AudioClip GetSFXByKey(string key)
    {
        foreach (var sound in _SFXList)
        {
            if (sound.key == key)
            {
                if (sound.clip.Count > 1)
                {
                    int randomIndex = Random.Range(0, sound.clip.Count);
                    return sound.clip[randomIndex];
                }
                else
                {
                    return sound.clip[0];
                }    
            }
        }
        Debug.LogWarning($"SFX not found for key: {key}");
        return null;
    }

    private AudioClip GetMusicByKey(string key)
    {
        foreach (var sound in _musicList)
        {
            if (sound.key == key)
            {
                return sound.clip;
            }
        }
        Debug.LogWarning($"Music not found for key: {key}");
        return null;
    }

    private AudioClip GetAmbianceByKey(string key)
    {
        foreach (var sound in _ambianceList)
        {
            if (sound.key == key)
            {
                return sound.clip;
            }
        }
        Debug.LogWarning($"Ambiance not found for key: {key}");
        return null;
    }

    #endregion

    #region Music

    private void ChangeMusic(AudioClip audioClip)
    {
        StartCoroutine(FadeOutInMusic(audioClip));
    }

    public void ChangeMusicByKey(string key)
    {
        ChangeMusic(GetMusicByKey(key));
    }

    #endregion

    #region Ambiances

    public void StopAmbianceSources() {
        foreach (AudioSource audioSource in _currentAmbianceSources) {
            StartCoroutine(FadeOutAudio(audioSource, _fadeOutDuration));
        }

        _currentAmbianceSources.Clear();
    }

    private void AddAmbianceSound(AudioClip audioClip) {
        AudioSource audioSource = GetAvailableAudioSource();
        audioSource.outputAudioMixerGroup = _ambianceMixerGroup;
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.volume = 1.0f;
        audioSource.Play();
        StartCoroutine(FadeInAudio(audioSource, _fadeInDuration));
        _currentAmbianceSources.Add(audioSource);
    }

    public void AddAmbianceSoundByKey(string key)
    {
        var audioClip = GetAmbianceByKey(key);
        if (audioClip != null)
        {
            AddAmbianceSound(audioClip);
        }
    }

    #endregion

    #region SoundFX

    private void PlaySoundFXClip(AudioClip audioClip, Vector3 spawnPosition, float volume = 1.0f) {
        AudioSource audioSource = CreateSoundFXSource(spawnPosition);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        PlayAndDestroy(audioSource, audioClip.length);
    }

    public void PlaySoundFXClipByKey(string key, Vector3 spawnPosition, float volume = 1.0f)
    {
        var audioClip = GetSFXByKey(key);
        if (audioClip != null)
        {
            PlaySoundFXClip(audioClip, spawnPosition, volume);
        }
    }

    public void PlaySoundFXClipByKey(string key, float volume = 1.0f)
    {
        PlaySoundFXClipByKey(key, _player.transform.position, volume);
    }

    public void PlayRandomSoundFXClipByKeys(string[] keys, Vector3 spawnPosition, float volume = 1.0f) {
        List<AudioClip> clips = new List<AudioClip>();
        foreach (var key in keys)
        {
            var clip = GetSFXByKey(key);
            if (clip != null)
            {
                clips.Add(clip);
            }
        }

        if (clips.Count > 0) {
            int rand = Random.Range(0, clips.Count);
            PlaySoundFXClip(clips[rand], spawnPosition, volume);
        }
    }

    private AudioSource CreateSoundFXSource(Vector3 spawnPosition) {
        AudioSource audioSource = Instantiate(soundFXObject, spawnPosition, Quaternion.identity);
        return audioSource;
    }

    private void PlayAndDestroy(AudioSource audioSource, float clipLength) {
        audioSource.Play();
        Destroy(audioSource.gameObject, clipLength);
    }

    #endregion

    #region Coroutines
    private IEnumerator FadeOutInMusic(AudioClip newClip) {
        if (_currentMusicSource != null) {
            yield return StartCoroutine(FadeOutAudio(_currentMusicSource, _fadeOutDuration));
        }

        AudioSource newMusicSource = GetAvailableAudioSource();
        newMusicSource.outputAudioMixerGroup = _musicMixerGroup;
        newMusicSource.clip = newClip;
        newMusicSource.loop = true;
        newMusicSource.volume = 0.0f;
        newMusicSource.Play();

        _currentMusicSource = newMusicSource;

        yield return StartCoroutine(FadeInAudio(newMusicSource, _fadeInDuration));
    }

    public IEnumerator FadeOutAudio(AudioSource audioSource, float duration) {
        float currentTime = 0f;
        float startVolume = audioSource.volume;

        while (currentTime < duration) {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0.0f, currentTime / duration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public IEnumerator FadeInAudio(AudioSource audioSource, float duration) {
        float currentTime = 0f;
        audioSource.volume = 0.0f;

        while (currentTime < duration) {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0.0f, 1.0f, currentTime / duration);
            yield return null;
        }

        audioSource.volume = 1.0f;
    }
    #endregion
}
