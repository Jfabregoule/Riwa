using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Windows;

[DefaultExecutionOrder(-2)]
public class SoundSystem : Singleton<SoundSystem>
{

    #region Classes
    /// <summary>
    /// Représente un effet sonore avec une clé d'identification et une liste de clips audio associés.
    /// </summary>
    public class SoundFX
    {
        public string key; // Clé unique de l'effet sonore.
        public List<AudioClip> clip; // Liste des clips audio associés à la clé.
    }

    /// <summary>
    /// Représente une piste musicale ou d'ambiance avec une clé d'identification et un clip audio associé.
    /// </summary>
    public class Track
    {
        public string key; // Clé unique de la piste musicale ou ambiance.
        public AudioClip clip; // Clip audio associé.
    }

    #endregion

    #region Fields

    [SerializeField] private string _SFXFolderPath; // Chemin du dossier contenant les effets sonores.
    [SerializeField] private string _musicFolderPath; // Chemin du dossier contenant la musique.
    [SerializeField] private string _ambianceFolderPath; // Chemin du dossier contenant les sons d'ambiance.

    [SerializeField] private AudioMixerGroup _musicMixerGroup; // Groupe de mixage pour la musique.
    [SerializeField] private AudioMixerGroup _ambianceMixerGroup; // Groupe de mixage pour l'ambiance.
    private AudioListener _audioListener; // Référence à l'AudioListener.

    [SerializeField] private AudioClip _startingMusic; // Musique de départ.
    [SerializeField] private AudioClip _startingAmbianceSound; // Son d'ambiance de départ.

    [SerializeField] private float _fadeInDuration; // Durée de fondu d'entrée.
    [SerializeField] private float _fadeOutDuration; // Durée de fondu de sortie.

    private List<AudioSource> _audioSources; // Liste des sources audio disponibles.
    private AudioSource _currentMusicSource; // Source audio actuellement utilisée pour la musique.
    private List<AudioSource> _currentAmbianceSources; // Liste des sources audio d'ambiance.

    [SerializeField] private AudioSource soundFXObject; // Objet préfabriqué pour jouer des effets sonores.

    private GameObject _player; // Référence au joueur.

    private List<SoundFX> _SFXList = new List<SoundFX>(); // Liste des effets sonores chargés.
    private List<Track> _musicList = new List<Track>(); // Liste des pistes musicales chargées.
    private List<Track> _ambianceList = new List<Track>(); // Liste des sons d'ambiance chargés.

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

        _player = GameObject.FindGameObjectWithTag("Player");
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

            Track newTrack = new Track
            {
                key = key,
                clip = audioClip
            };

            _musicList.Add(newTrack);
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

            Track newTrack = new Track
            {
                key = key,
                clip = audioClip
            };

            _ambianceList.Add(newTrack);
        }
    }

    /// <summary>
    /// Définie le nouvel audioListerner.
    /// </summary>
    /// <param name="audioListener">Nouveau audioListener.</param>
    public void SetAudioListener(AudioListener audioListener)
    {
        _audioListener = audioListener;
    }

    /// <summary>
    /// Définie le nouveau Player.
    /// </summary>
    /// <param name="player">Nouveau Player.</param>
    public void SetPlayer(GameObject player)
    {
        _player = player;
    }

    /// <summary>
    /// Récupérer une audio source disponible.
    /// </summary>
    /// <returns>Audio source disponible retournée.</returns>
    private AudioSource GetAvailableAudioSource()
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
    private AudioClip GetSFXByKey(string key)
    {
        foreach (var sound in _SFXList)
        {
            if (sound.key == key)
            {
                if (sound.clip.Count > 1)
                {
                    int randomIndex = UnityEngine.Random.Range(0, sound.clip.Count);
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

    /// <summary>
    /// Récupérer une musique grace à sa clé.
    /// </summary>
    /// <param name="key">Clé de la musique recherchée.</param>
    /// <returns>Audio clip trouvé retourné ou null si inéxistant.</returns>
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

    /// <summary>
    /// Récupérer un son d'ambiance grace à sa clé.
    /// </summary>
    /// <param name="key">Clé du son d'ambiance recherché.</param>
    /// <returns>Audio clip trouvé retourné ou null si inéxistant.</returns>
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
        foreach(AudioSource audioSource in _currentAmbianceSources)
        {
            string audioClipKey = audioSource.clip.name.Split(" ")[1];
            if (audioClipKey == key)
            {
                StartCoroutine(FadeOutAudio(audioSource, _fadeOutDuration));
                _currentAmbianceSources.Remove(audioSource);
            }
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
    /// Permet de faire spawn une prefab contenant une audiosource sur le joueur avec un volume pour jouer un son spécifique ponctuel.
    /// </summary>
    /// <param name="key">Clé du son à jouer.</param>
    /// <param name="volume">Volume du son à jouer.</param>
    public AudioSource PlaySoundFXClipByKey(string key, float volume = 1.0f)
    {
        AudioClip audioClip = GetSFXByKey(key);
        if (audioClip != null)
        {
            AudioSource audioSource = GetAvailableAudioSource();
            audioSource.volume = volume;
            audioSource.loop = false;
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
            audioSource.volume = Mathf.Lerp(0.0f, 1.0f, currentTime / duration);
            yield return null;
        }

        audioSource.volume = 1.0f;
    }

    #endregion
}
