using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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

    private int _numberOfChannels; // Nombre de canaux audio disponibles.

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
        _numberOfChannels = GetComponents<AudioSource>().Length;

        AudioSource[] attachedAudioSources = GetComponents<AudioSource>();

        for (int i = 0; i < _numberOfChannels; i++) {
            _audioSources.Add(attachedAudioSources[i]);
        }

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

    /// <summary>
    /// Génère les clés d'identification des musiques.
    /// </summary>
    private void GenerateMusicKeys()
    {
        AudioClip[] audioClips = Resources.LoadAll<AudioClip>(_musicFolderPath);

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

    /// <summary>
    /// Génère les clés d'identification des sons d'ambiance.
    /// </summary>
    private void GenerateAmbianceKeys()
    {
        AudioClip[] audioClips = Resources.LoadAll<AudioClip>(_ambianceFolderPath);

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
    private void ChangeMusic(AudioClip audioClip)
    {
        StartCoroutine(FadeOutInMusic(audioClip));
    }

    /// <summary>
    /// Change la musique actuelle avec la musique trouvée avec la clé donnée.
    /// </summary>
    /// <param name="key">Clé de la nouvelle musique.</param>
    public void ChangeMusicByKey(string key)
    {
        ChangeMusic(GetMusicByKey(key));
    }

    #endregion

    #region Ambiances

    /// <summary>
    /// Stop les sons d'ambiances en cours.
    /// </summary>
    public void StopAmbianceSources() 
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

    /// <summary>
    /// Ajoute un nouveau son d'ambiance.
    /// </summary>
    /// <param name="key">Clé du son d'ambiance à ajouter.</param>
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

    /// <summary>
    /// Permet de faire spawn une prefab contenant une audiosource à une position avec un volume pour jouer un son spécifique ponctuel.
    /// </summary>
    /// <param name="audioClip">Audioclip à jouer.</param>
    /// <param name="spawnPosition">Position ou jouer le son.</param>
    /// <param name="volume">Volume du son à jouer.</param>
    private void PlaySoundFXClip(AudioClip audioClip, Vector3 spawnPosition, float volume = 1.0f) {
        AudioSource audioSource = CreateSoundFXSource(spawnPosition);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        PlayAndDestroy(audioSource, audioClip.length);
    }

    /// <summary>
    /// Permet de faire spawn une prefab contenant une audiosource à une position avec un volume pour jouer un son spécifique ponctuel.
    /// </summary>
    /// <param name="key">Clé du son à jouer.</param>
    /// <param name="spawnPosition">Position ou jouer le son.</param>
    /// <param name="volume">Volume du son à jouer.</param>
    public void PlaySoundFXClipByKey(string key, Vector3 spawnPosition, float volume = 1.0f)
    {
        var audioClip = GetSFXByKey(key);
        if (audioClip != null)
        {
            PlaySoundFXClip(audioClip, spawnPosition, volume);
        }
    }

    /// <summary>
    /// Permet de faire spawn une prefab contenant une audiosource sur le joueur avec un volume pour jouer un son spécifique ponctuel.
    /// </summary>
    /// <param name="key">Clé du son à jouer.</param>
    /// <param name="volume">Volume du son à jouer.</param>
    public void PlaySoundFXClipByKey(string key, float volume = 1.0f)
    {
        PlaySoundFXClipByKey(key, _player.transform.position, volume);
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
            int rand = Random.Range(0, clips.Count);
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

    #endregion

    #region Coroutines

    /// <summary>
    /// Fait une transition douce entre la musique en cours et une musique donnée.
    /// </summary>
    /// <param name="newClip">Nouvelle musique.</param>
    private IEnumerator FadeOutInMusic(AudioClip newClip) 
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

        yield return StartCoroutine(FadeInAudio(newMusicSource, _fadeInDuration));
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
    public IEnumerator FadeInAudio(AudioSource audioSource, float duration) 
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
