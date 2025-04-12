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
    /// Repr�sente un effet sonore avec une cl� d'identification et une liste de clips audio associ�s.
    /// </summary>
    public class SoundFX
    {
        public string key; // Cl� unique de l'effet sonore.
        public List<AudioClip> clip; // Liste des clips audio associ�s � la cl�.
    }

    /// <summary>
    /// Repr�sente une piste musicale ou d'ambiance avec une cl� d'identification et un clip audio associ�.
    /// </summary>
    public class Track
    {
        public string key; // Cl� unique de la piste musicale ou ambiance.
        public AudioClip clip; // Clip audio associ�.
    }

    #endregion

    #region Fields

    [SerializeField] private string _SFXFolderPath; // Chemin du dossier contenant les effets sonores.
    [SerializeField] private string _musicFolderPath; // Chemin du dossier contenant la musique.
    [SerializeField] private string _ambianceFolderPath; // Chemin du dossier contenant les sons d'ambiance.

    [SerializeField] private AudioMixerGroup _musicMixerGroup; // Groupe de mixage pour la musique.
    [SerializeField] private AudioMixerGroup _ambianceMixerGroup; // Groupe de mixage pour l'ambiance.
    private AudioListener _audioListener; // R�f�rence � l'AudioListener.

    [SerializeField] private AudioClip _startingMusic; // Musique de d�part.
    [SerializeField] private AudioClip _startingAmbianceSound; // Son d'ambiance de d�part.

    [SerializeField] private float _fadeInDuration; // Dur�e de fondu d'entr�e.
    [SerializeField] private float _fadeOutDuration; // Dur�e de fondu de sortie.

    private List<AudioSource> _audioSources; // Liste des sources audio disponibles.
    private AudioSource _currentMusicSource; // Source audio actuellement utilis�e pour la musique.
    private List<AudioSource> _currentAmbianceSources; // Liste des sources audio d'ambiance.

    [SerializeField] private AudioSource soundFXObject; // Objet pr�fabriqu� pour jouer des effets sonores.

    private GameObject _player; // R�f�rence au joueur.

    private List<SoundFX> _SFXList = new List<SoundFX>(); // Liste des effets sonores charg�s.
    private List<Track> _musicList = new List<Track>(); // Liste des pistes musicales charg�es.
    private List<Track> _ambianceList = new List<Track>(); // Liste des sons d'ambiance charg�s.

    #endregion

    #region Main Functions

    /// <summary>
    /// Initialisation du syst�me sonore et chargement des pistes audio.
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
    /// G�n�re les cl�s d'identification des sons en fonction de leur type (SFX, musique, ambiance).
    /// </summary>
    private void GenerateKeys()
    {
        GenerateSFXKeys();
        GenerateMusicKeys();
        GenerateAmbianceKeys();
    }

    /// <summary>
    /// G�n�re les cl�s d'identification des SFX.
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
    /// G�n�re les cl�s d'identification des musiques.
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
    /// G�n�re les cl�s d'identification des sons d'ambiance.
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
    /// D�finie le nouvel audioListerner.
    /// </summary>
    /// <param name="audioListener">Nouveau audioListener.</param>
    public void SetAudioListener(AudioListener audioListener)
    {
        _audioListener = audioListener;
    }

    /// <summary>
    /// D�finie le nouveau Player.
    /// </summary>
    /// <param name="player">Nouveau Player.</param>
    public void SetPlayer(GameObject player)
    {
        _player = player;
    }

    /// <summary>
    /// R�cup�rer une audio source disponible.
    /// </summary>
    /// <returns>Audio source disponible retourn�e.</returns>
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
    /// R�cup�rer un SFX grace � sa cl�.
    /// </summary>
    /// <param name="key">Cl� du son recherch�.</param>
    /// <returns>Audio clip trouv� retourn� ou null si in�xistant.</returns>
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
    /// R�cup�rer une musique grace � sa cl�.
    /// </summary>
    /// <param name="key">Cl� de la musique recherch�e.</param>
    /// <returns>Audio clip trouv� retourn� ou null si in�xistant.</returns>
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
    /// R�cup�rer un son d'ambiance grace � sa cl�.
    /// </summary>
    /// <param name="key">Cl� du son d'ambiance recherch�.</param>
    /// <returns>Audio clip trouv� retourn� ou null si in�xistant.</returns>
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
    /// Change la musique actuelle avec la musique donn�e.
    /// </summary>
    /// <param name="audioClip">Audioclip de la nouvelle musique.</param>
    private void ChangeMusic(AudioClip audioClip, float volume = 1.0f)
    {
        StartCoroutine(FadeOutInMusic(audioClip, volume));
    }

    /// <summary>
    /// Change la musique actuelle avec la musique trouv�e avec la cl� donn�e.
    /// </summary>
    /// <param name="key">Cl� de la nouvelle musique.</param>
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
    /// <param name="audioClip">Clip audio d'ambiance � ajouter.</param>
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
    /// <param name="key">Cl� du son d'ambiance � ajouter.</param>
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
    /// Arr�te un nouveau son d'ambiance.
    /// </summary>
    /// <param name="key">Cl� du son d'ambiance � arr�ter.</param>
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
    /// Permet de faire spawn une prefab contenant une audiosource � une position avec un volume pour jouer un son sp�cifique ponctuel.
    /// </summary>
    /// <param name="audioClip">Audioclip � jouer.</param>
    /// <param name="spawnPosition">Position ou jouer le son.</param>
    /// <param name="volume">Volume du son � jouer.</param>
    private AudioSource PlaySoundFXClip(AudioClip audioClip, Vector3 spawnPosition, float volume = 1.0f) 
    {
        AudioSource audioSource = CreateSoundFXSource(spawnPosition);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        PlayAndDestroy(audioSource, audioClip.length);
        return audioSource;
    }

    /// <summary>
    /// Permet de faire spawn une prefab contenant une audiosource � une position avec un volume pour jouer un son sp�cifique ponctuel.
    /// </summary>
    /// <param name="key">Cl� du son � jouer.</param>
    /// <param name="spawnPosition">Position ou jouer le son.</param>
    /// <param name="volume">Volume du son � jouer.</param>
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
    /// Permet de faire spawn une prefab contenant une audiosource sur le joueur avec un volume pour jouer un son sp�cifique ponctuel.
    /// </summary>
    /// <param name="key">Cl� du son � jouer.</param>
    /// <param name="volume">Volume du son � jouer.</param>
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
    /// Permet de faire spawn des prefabs contenant une audiosource � une position avec un volume pour jouer plusieurs sons sp�cifique ponctuel.
    /// </summary>
    /// <param name="keys">Cl�s des sons � jouer.</param>
    /// <param name="spawnPosition">Position ou jouer le son.</param>
    /// <param name="volume">Volume du son � jouer.</param>
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
    /// Permet de faire spawn la prefab contenant une audiosource � une position.
    /// </summary>
    /// <param name="spawnPosition">Position ou spawn la prefab.</param>
    /// <returns> AudioSource spawn.</returns>
    private AudioSource CreateSoundFXSource(Vector3 spawnPosition) 
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnPosition, Quaternion.identity);
        return audioSource;
    }

    /// <summary>
    /// Active une audiosource et la destroy � la fin du son qu'elle joue.
    /// </summary>
    /// <param name="audioSource">AudioSource � activer.</param>
    /// <param name="clipLength">Dur�e du son.</param>
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
    /// Fait une transition douce entre la musique en cours et une musique donn�e.
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
    /// Diminue lentement le son d'une musique jusqu'� 0.
    /// </summary>
    /// <param name="audioSource">AudioSource � couper.</param>
    /// <param name="duration">Dur�e avant d'atteindre 0.</param>
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
    /// Augmente lentement le son d'une musique jusqu'� 100.
    /// </summary>
    /// <param name="audioSource">AudioSource � augmenter.</param>
    /// <param name="duration">Dur�e avant d'atteindre 100.</param>
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
