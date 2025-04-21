using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Windows;

[DefaultExecutionOrder(-2)]
public class SoundSystem<T> : Singleton<T> where T : SoundSystem<T>
{

    #region Fields

    [Header("Files Paths")]
    [Space(10)]

    [SerializeField] private string _SFXFolderPath; // Chemin du dossier contenant les effets sonores.
    [SerializeField] private string _musicFolderPath; // Chemin du dossier contenant la musique.
    [SerializeField] private string _ambianceFolderPath; // Chemin du dossier contenant les sons d'ambiance.

    [Header("Mixer Groups")]
    [Space(10)]

    [SerializeField] protected AudioMixerGroup _musicMixerGroup; // Groupe de mixage pour la musique.
    [SerializeField] protected AudioMixerGroup _ambianceMixerGroup; // Groupe de mixage pour l'ambiance.
    [SerializeField] protected AudioMixerGroup _sfxMixerGroup; // Groupe de mixage pour les SFX.

    [Header("Starting Sounds")]
    [Space(10)]

    [SerializeField] private AudioClip _startingMusic; // Musique de départ.
    [SerializeField] private List<AudioClip> _startingAmbianceSounds; // Sons d'ambiance de départ.

    [Header("Blend Values")]
    [Space(10)]

    [SerializeField] protected float _fadeInDuration; // Durée de fondu d'entrée.
    [SerializeField] protected float _fadeOutDuration; // Durée de fondu de sortie.

    [Header("3D SFX GameObject")]
    [Space(10)]

    [SerializeField] protected AudioSource soundFXObject; // Objet préfabriqué pour jouer des effets sonores.

    protected List<AudioSource> _audioSources; // Liste des sources audio disponibles.

    private AudioSource _currentMusicSource; // Source audio actuellement utilisée pour la musique.
    private List<AudioSource> _currentAmbianceSources; // Liste des sources audio d'ambiance.

    protected Dictionary<string, AudioClip> _musicList = new Dictionary<string, AudioClip>(); // Liste des pistes musicales chargées.
    protected Dictionary<string, AudioClip> _ambianceList = new Dictionary<string, AudioClip>(); // Liste des sons d'ambiance chargés.
    protected Dictionary<string, List<AudioClip>> _SFXList = new Dictionary<string, List<AudioClip>>(); // Liste des effets sonores chargés.

    #endregion

    #region Main Functions

    /// <summary>
    /// Initialisation du système sonore et chargement des pistes audio.
    /// </summary>
    protected override void Awake() {
        base.Awake();

        GenerateKeys();
        _audioSources = new List<AudioSource>();
        _currentMusicSource = null;
        _currentAmbianceSources = new List<AudioSource>();
        _audioSources.AddRange(GetComponents<AudioSource>());
    }

    protected virtual void Start()
    {
        if (_startingMusic != null)
            ChangeMusic(_startingMusic);

        if(_startingAmbianceSounds.Count > 0)
            AddAmbianceSounds(_startingAmbianceSounds);
    }

    /// <summary>
    /// Génère les clés d'identification des sons en fonction de leur type (SFX, musique, ambiance).
    /// </summary>
    private void GenerateKeys()
    {
        GenerateSFXKeys();
        GenerateMusicKeys();
        GenerateAmbianceKeys();
    }

    /// <summary>
    /// Génère les clés d'identification des SFX.
    /// </summary>
    private void GenerateSFXKeys()
    {
        AudioClip[] audioClips = Resources.LoadAll<AudioClip>(_SFXFolderPath);

        foreach (AudioClip audioClip in audioClips)
        {
            string fileName = audioClip.name;
            string[] fileNameWords = fileName.Split(new char[] {'_', ' '}, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < fileNameWords.Length; i++)
            {
                fileNameWords[i] = char.ToUpper(fileNameWords[i][0]) + fileNameWords[i].Substring(1).ToLower();
            }

            string finalName = string.Join(" ", fileNameWords);

            string key = $"{finalName}";

            List<AudioClip> audioclips = null;
            if(_SFXList.TryGetValue(key, out audioclips))
            {
                audioclips.Add(audioClip);
            }
            else
            {
                _SFXList.Add(key, new List<AudioClip> { audioClip });
            }
        }
    }


    /// <summary>
    /// Génère les clés d'identification des musiques.
    /// </summary>
    private void GenerateMusicKeys()
    {
        AudioClip[] audioClips = Resources.LoadAll<AudioClip>(_musicFolderPath);

        foreach (AudioClip audioClip in audioClips)
        {
            string fileName = audioClip.name;
            string[] fileNameWords = fileName.Split(new char[] { '_', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < fileNameWords.Length; i++)
            {
                fileNameWords[i] = char.ToUpper(fileNameWords[i][0]) + fileNameWords[i].Substring(1).ToLower();
            }

            string finalName = string.Join(" ", fileNameWords);

            string key = $"{finalName}";

            _musicList.Add(key, audioClip);
        }
    }

    /// <summary>
    /// Génère les clés d'identification des sons d'ambiance.
    /// </summary>
    private void GenerateAmbianceKeys()
    {
        AudioClip[] audioClips = Resources.LoadAll<AudioClip>(_ambianceFolderPath);

        foreach (AudioClip audioClip in audioClips)
        {
            string fileName = audioClip.name;
            string[] fileNameWords = fileName.Split(new char[] { '_', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < fileNameWords.Length; i++)
            {
                fileNameWords[i] = char.ToUpper(fileNameWords[i][0]) + fileNameWords[i].Substring(1).ToLower();
            }

            string finalName = string.Join(" ", fileNameWords);

            string key = $"{finalName}";

            _ambianceList.Add(key, audioClip);
        }
    }

    /// <summary>
    /// Récupérer une audio source disponible.
    /// </summary>
    /// <returns>Audio source disponible retournée.</returns>
    protected AudioSource GetAvailableAudioSource()
    {
        AudioSource oldestAudioSource = null;
        float oldestTime = float.MaxValue;

        foreach (AudioSource audioSource in _audioSources)
        {
            if (!audioSource.isPlaying)
            {
                return audioSource;
            }

            float elapsedTime = Time.time - audioSource.time;
            if (elapsedTime < oldestTime)
            {
                oldestTime = elapsedTime;
                oldestAudioSource = audioSource;
            }
        }

        oldestAudioSource.Stop();
        return oldestAudioSource;
    }

    /// <summary>
    /// Récupérer un SFX grace à sa clé.
    /// </summary>
    /// <param name="key">Clé du son recherché.</param>
    /// <returns>Audio clip trouvé retourné ou null si inéxistant.</returns>
    protected AudioClip GetSFXByKey(string key)
    {
        List<AudioClip> audioClips = null;

        if (!_SFXList.TryGetValue(key, out audioClips))
        {
            Debug.LogWarning($"SFX not found for key: {key}");
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, audioClips.Count);
        return audioClips[randomIndex];
    }

    /// <summary>
    /// Récupérer une musique grace à sa clé.
    /// </summary>
    /// <param name="key">Clé de la musique recherchée.</param>
    /// <returns>Audio clip trouvé retourné ou null si inéxistant.</returns>
    protected AudioClip GetMusicByKey(string key)
    {
        AudioClip audioClip = null;
        if (!_musicList.TryGetValue(key, out audioClip))
        {
            Debug.LogWarning($"Music not found for key: {key}");
            return null;
        }
        return audioClip;
    }

    /// <summary>
    /// Récupérer un son d'ambiance grace à sa clé.
    /// </summary>
    /// <param name="key">Clé du son d'ambiance recherché.</param>
    /// <returns>Audio clip trouvé retourné ou null si inéxistant.</returns>
    protected AudioClip GetAmbianceByKey(string key)
    {
        AudioClip audioClip = null;
        if (!_ambianceList.TryGetValue(key, out audioClip))
        {
            Debug.LogWarning($"Ambiance sound not found for key: {key}");
            return null;
        }
        return audioClip;
    }

    #endregion

    #region Music

    /// <summary>
    /// Change la musique actuelle avec la musique donnée.
    /// </summary>
    /// <param name="audioClip">Audioclip de la nouvelle musique.</param>
    private void ChangeMusic(AudioClip audioClip, float volume = 1.0f)
    {
        StartCoroutine(FadeOutInMusic(audioClip, volume));
    }

    /// <summary>
    /// Change la musique actuelle avec la musique trouvée avec la clé donnée.
    /// </summary>
    /// <param name="key">Clé de la nouvelle musique.</param>
    public void ChangeMusicByKey(string key, float volume = 1.0f)
    {
        ChangeMusic(GetMusicByKey(key), volume);
    }

    /// <summary>
    /// Stop la musique en cours.
    /// </summary>
    public void StopMusic()
    {
        if (_currentMusicSource)
        {
            StartCoroutine(FadeOutAudio(_currentMusicSource, _fadeOutDuration));
            _currentMusicSource = null;
        }
    }

    #endregion

    #region Ambiances

    /// <summary>
    /// Stop les sons d'ambiances en cours.
    /// </summary>
    public void StopAllAmbianceSources() 
    {
        foreach (AudioSource audioSource in _currentAmbianceSources) 
        {
            StartCoroutine(FadeOutAudio(audioSource, _fadeOutDuration));
        }

        _currentAmbianceSources.Clear();
    }

    /// <summary>
    /// Ajoute un nouveau son d'ambiance.
    /// </summary>
    /// <param name="audioClip">Clip audio d'ambiance à ajouter.</param>
    private void AddAmbianceSound(AudioClip audioClip, float volume = 1.0f)
    {
        AudioSource audioSource = GetAvailableAudioSource();
        audioSource.outputAudioMixerGroup = _ambianceMixerGroup;
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.volume = volume;
        audioSource.Play();
        StartCoroutine(FadeInAudio(audioSource, _fadeInDuration, volume));
        _currentAmbianceSources.Add(audioSource);
    }

    /// <summary>
    /// Ajoute des nouveaux sons d'ambiance.
    /// </summary>
    /// <param name="audioClips">Clips audio d'ambiance à ajouter.</param>
    private void AddAmbianceSounds(List<AudioClip> audioClips, float volume = 1.0f)
    {
        foreach(AudioClip audioClip in audioClips)
        {
            AudioSource audioSource = GetAvailableAudioSource();
            audioSource.outputAudioMixerGroup = _ambianceMixerGroup;
            audioSource.clip = audioClip;
            audioSource.loop = true;
            audioSource.volume = volume;
            audioSource.Play();
            StartCoroutine(FadeInAudio(audioSource, _fadeInDuration, volume));
            _currentAmbianceSources.Add(audioSource);
        }
    }

    /// <summary>
    /// Ajoute un nouveau son d'ambiance.
    /// </summary>
    /// <param name="key">Clé du son d'ambiance à ajouter.</param>
    public AudioSource AddAmbianceSoundByKey(string key)
    {
        AudioClip audioClip = GetAmbianceByKey(key);
        if (audioClip != null)
        {
            AddAmbianceSound(audioClip);
            return _currentAmbianceSources.ElementAt(_currentAmbianceSources.Count - 1);
        }
        return null;
    }

    /// <summary>
    /// Arrête un nouveau son d'ambiance.
    /// </summary>
    /// <param name="key">Clé du son d'ambiance à arrêter.</param>
    public void StopAmbianceSoundByKey(string key)
    {
        List<AudioSource> sourcesToRemove = _currentAmbianceSources
            .Where(source => source.clip.name.Split(" ")[1] == key)
            .ToList();

        foreach (AudioSource source in sourcesToRemove)
        {
            StartCoroutine(FadeOutAudio(source, _fadeOutDuration));
            _currentAmbianceSources.Remove(source);
        }
    }

    #endregion

    #region SoundFX

    /// <summary>
    /// Permet de faire spawn une prefab contenant une audiosource à une position avec un volume pour jouer un son spécifique ponctuel.
    /// </summary>
    /// <param name="audioClip">Audioclip à jouer.</param>
    /// <param name="spawnPosition">Position ou jouer le son.</param>
    /// <param name="volume">Volume du son à jouer.</param>
    private AudioSource PlaySoundFXClip(AudioClip audioClip, Vector3 spawnPosition, float volume = 1.0f) 
    {
        AudioSource audioSource = CreateSoundFXSource(spawnPosition);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        PlayAndDestroy(audioSource, audioClip.length);
        return audioSource;
    }

    /// <summary>
    /// Permet de faire spawn une prefab contenant une audiosource à une position avec un volume pour jouer un son spécifique ponctuel.
    /// </summary>
    /// <param name="key">Clé du son à jouer.</param>
    /// <param name="spawnPosition">Position ou jouer le son.</param>
    /// <param name="volume">Volume du son à jouer.</param>
    public AudioSource PlaySoundFXClipByKey(string key, Vector3 spawnPosition, float volume = 1.0f)
    {
        var audioClip = GetSFXByKey(key);
        if (audioClip != null)
        {
            return (PlaySoundFXClip(audioClip, spawnPosition, volume));
        }
        return null;
    }

    /// <summary>
    /// Permet de jouer un SFX en 2D sur les available audio sources.
    /// </summary>
    /// <param name="key">Clé du son à jouer.</param>
    /// <param name="volume">Volume du son à jouer.</param>
    public AudioSource PlaySoundFXClipByKey(string key, float volume = 1.0f)
    {
        AudioClip audioClip = GetSFXByKey(key);
        if (audioClip != null)
        {
            AudioSource audioSource = GetAvailableAudioSource();
            audioSource.clip = audioClip;
            audioSource.volume = volume;
            audioSource.loop = false;
            audioSource.outputAudioMixerGroup = _sfxMixerGroup;
            audioSource.Play();
            return audioSource;
        }

        return null;
    }

    /// <summary>
    /// Permet de faire spawn des prefabs contenant une audiosource à une position avec un volume pour jouer plusieurs sons spécifique ponctuel.
    /// </summary>
    /// <param name="keys">Clés des sons à jouer.</param>
    /// <param name="spawnPosition">Position ou jouer le son.</param>
    /// <param name="volume">Volume du son à jouer.</param>
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
            int rand = UnityEngine.Random.Range(0, clips.Count);
            PlaySoundFXClip(clips[rand], spawnPosition, volume);
        }
    }

    /// <summary>
    /// Permet de faire spawn la prefab contenant une audiosource à une position.
    /// </summary>
    /// <param name="spawnPosition">Position ou spawn la prefab.</param>
    /// <returns> AudioSource spawn.</returns>
    private AudioSource CreateSoundFXSource(Vector3 spawnPosition) 
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnPosition, Quaternion.identity);
        return audioSource;
    }

    /// <summary>
    /// Active une audiosource et la destroy à la fin du son qu'elle joue.
    /// </summary>
    /// <param name="audioSource">AudioSource à activer.</param>
    /// <param name="clipLength">Durée du son.</param>
    private void PlayAndDestroy(AudioSource audioSource, float clipLength) 
    {
        audioSource.Play();
        Destroy(audioSource.gameObject, clipLength);
    }

    public void StopAudioSource(AudioSource audioSource)
    {
        audioSource.Stop();
        if(!audioSource.transform.parent == this)
        {
            Destroy(audioSource);
        }
    }

    #endregion

    #region Coroutines

    /// <summary>
    /// Fait une transition douce entre la musique en cours et une musique donnée.
    /// </summary>
    /// <param name="newClip">Nouvelle musique.</param>
    private IEnumerator FadeOutInMusic(AudioClip newClip, float volume = 1.0f) 
    {
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

        yield return StartCoroutine(FadeInAudio(newMusicSource, _fadeInDuration, volume));
    }

    /// <summary>
    /// Diminue lentement le son d'une musique jusqu'à 0.
    /// </summary>
    /// <param name="audioSource">AudioSource à couper.</param>
    /// <param name="duration">Durée avant d'atteindre 0.</param>
    public IEnumerator FadeOutAudio(AudioSource audioSource, float duration) 
    {
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

    /// <summary>
    /// Augmente lentement le son d'une musique jusqu'à 100.
    /// </summary>
    /// <param name="audioSource">AudioSource à augmenter.</param>
    /// <param name="duration">Durée avant d'atteindre 100.</param>
    public IEnumerator FadeInAudio(AudioSource audioSource, float duration, float maxVolume) 
    {
        float currentTime = 0f;
        audioSource.volume = 0.0f;

        while (currentTime < duration) {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0.0f, maxVolume, currentTime / duration);
            yield return null;
        }

        audioSource.volume = maxVolume;
    }

    #endregion
}
