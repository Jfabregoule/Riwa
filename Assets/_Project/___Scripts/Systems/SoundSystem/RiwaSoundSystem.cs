using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class RiwaSoundSystem : SoundSystem<RiwaSoundSystem>
{
    #region Fields

    [Header("Temporality Mixer Groups")]
    [Space(10)]

    [SerializeField] private AudioMixerGroup _musicPastMixerGroup; // Groupe de mixage pour la musique du passé.
    [SerializeField] private AudioMixerGroup _musicPresentMixerGroup; // Groupe de mixage pour la musique du présent.
    [SerializeField] private AudioMixerGroup _ambiancePastMixerGroup; // Groupe de mixage pour l'ambiance du passé.
    [SerializeField] private AudioMixerGroup _ambiancePresentMixerGroup; // Groupe de mixage pour l'ambiance du present.
    [SerializeField] private AudioMixerGroup _sfxPastMixerGroup; // Groupe de mixage pour les SFX du passé.
    [SerializeField] private AudioMixerGroup _sfxPresentMixerGroup; // Groupe de mixage pour les SFX du présent.

    private Dictionary<Temporality, AudioMixerGroup> _musicMixerGroups; // Dictionnaire des musiques groups contenant les groupes de chaque temporalité associé à leur clé.
    private Dictionary<Temporality, AudioMixerGroup> _ambianceMixerGroups; // Dictionnaire des ambiance groups contenant les groupes de chaque temporalité associé à leur clé.
    private Dictionary<Temporality, AudioMixerGroup> _sfxMixerGroups; // Dictionnaire des sfx groups contenant les groupes de chaque temporalité associé à leur clé.

    [Header("Temporality Starting Sounds")]
    [Space(10)]

    [SerializeField] private AudioClip _startingPastMusic; // Musique de départ du passé.
    [SerializeField] private AudioClip _startingPresentMusic; // Musique de départ du présent.
    [SerializeField] private List<AudioClip> _startingPastAmbianceSounds; // Sons d'ambiance de départ.
    [SerializeField] private List<AudioClip> _startingPresentAmbianceSounds; // Sons d'ambiance de départ.

    private AudioSource _currentPastMusicSource; // Source audio actuellement utilisée pour la musique du passé.
    private AudioSource _currentPresentMusicSource; // Source audio actuellement utilisée pour la musique du présent.
    private List<AudioSource> _currentPastAmbianceSources; // Liste des sources audio d'ambiance du passé.
    private List<AudioSource> _currentPresentAmbianceSources; // Liste des sources audio d'ambiance du présent.

    private Dictionary<Temporality, AudioSource> _currentMusicSources;
    private Dictionary<Temporality, List<AudioSource>> _currentAmbianceSources;

    #endregion

    #region Main Functions

    /// <summary>
    /// Initialisation du système sonore et chargement des pistes audio.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        _currentPastMusicSource = null;
        _currentPresentMusicSource = null;
        _currentPastAmbianceSources = new List<AudioSource>();
        _currentPresentAmbianceSources = new List<AudioSource>();

        _musicMixerGroups = new Dictionary<Temporality, AudioMixerGroup>
    {
        { Temporality.Past, _musicPastMixerGroup },
        { Temporality.Present, _musicPresentMixerGroup }
    };

        _ambianceMixerGroups = new Dictionary<Temporality, AudioMixerGroup>
    {
        { Temporality.Past, _ambiancePastMixerGroup },
        { Temporality.Present, _ambiancePresentMixerGroup }
    };

        _currentMusicSources = new Dictionary<Temporality, AudioSource>
    {
        { Temporality.Past, _currentPastMusicSource },
        { Temporality.Present, _currentPresentMusicSource }
    };

        _currentAmbianceSources = new Dictionary<Temporality, List<AudioSource>>
    {
        { Temporality.Past, _currentPastAmbianceSources },
        { Temporality.Present, _currentPresentAmbianceSources }
    };

    }

    protected override void Start()
    {
        base.Start();

        if (_startingPastMusic != null)
            ChangeMusic(_startingPastMusic, Temporality.Past);
        if (_startingPresentMusic != null)
            ChangeMusic(_startingPresentMusic, Temporality.Present);

        //if (_startingAmbianceSounds.Count > 0)
        //    AddAmbianceSounds(_startingAmbianceSounds);
    }

    #endregion

    #region Music

    /// <summary>
    /// Change la musique actuelle avec la musique donnée.
    /// </summary>
    /// <param name="audioClip">Audioclip de la nouvelle musique.</param>
    private void ChangeMusic(AudioClip audioClip, Temporality temporality, float volume = 1.0f)
    {
        StartCoroutine(FadeOutInMusic(audioClip, temporality, volume));
    }

    /// <summary>
    /// Change la musique actuelle avec la musique trouvée avec la clé donnée pour une temporalité donnée.
    /// </summary>
    /// <param name="key">Clé de la nouvelle musique.</param>
    /// <param name="temporality">Temporalité affectée.</param>
    public void ChangeMusicByKey(string key, Temporality temporality, float volume = 1.0f)
    {
        ChangeMusic(GetMusicByKey(key), temporality, volume);
    }

    /// <summary>
    /// Stop la musique en cours d'une temporalité donnée.
    /// </summary>
    /// <param name="temporality">Temporalité affectée.</param>
    public void StopMusic(Temporality temporality)
    {
        if (_currentMusicSources[temporality])
        {
            StartCoroutine(FadeOutAudio(_currentMusicSources[temporality], _fadeOutDuration));
            _currentMusicSources[temporality] = null;
        }
    }

    /// <summary>
    /// Stop toutes les musiques en cours.
    /// </summary>
    public void StopAllMusics()
    {
        foreach (Temporality temporality in System.Enum.GetValues(typeof(Temporality)))
        {
            if (_currentMusicSources[temporality])
            {
                StartCoroutine(FadeOutAudio(_currentMusicSources[temporality], _fadeOutDuration));
                _currentMusicSources[temporality] = null;
            }
        }
    }

    #endregion

    #region Ambiances

    /// <summary>
    /// Stop les sons d'ambiances en cours d'une temporalité donnée.
    /// </summary>
    public void StopAllAmbianceSources(Temporality temporality)
    {
        foreach (AudioSource audioSource in _currentAmbianceSources[temporality])
        {
            StartCoroutine(FadeOutAudio(audioSource, _fadeOutDuration));
        }

        _currentAmbianceSources[temporality].Clear();
    }

    /// <summary>
    /// Ajoute un nouveau son d'ambiance pour une temporalité donnée.
    /// </summary>
    private void AddAmbianceSound(AudioClip audioClip, Temporality temporality, float volume = 1.0f)
    {
        AudioSource audioSource = GetAvailableAudioSource();
        audioSource.outputAudioMixerGroup = _ambianceMixerGroups[temporality];
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.volume = volume;
        audioSource.Play();
        StartCoroutine(FadeInAudio(audioSource, _fadeInDuration, volume));
        _currentAmbianceSources[temporality].Add(audioSource);
    }

    /// <summary>
    /// Ajoute plusieurs sons d'ambiance pour une temporalité donnée.
    /// </summary>
    private void AddAmbianceSounds(List<AudioClip> audioClips, Temporality temporality, float volume = 1.0f)
    {
        foreach (AudioClip audioClip in audioClips)
        {
            AddAmbianceSound(audioClip, temporality, volume);
        }
    }

    /// <summary>
    /// Ajoute un nouveau son d'ambiance via sa clé pour une temporalité donnée.
    /// </summary>
    public AudioSource AddAmbianceSoundByKey(string key, Temporality temporality)
    {
        AudioClip audioClip = GetAmbianceByKey(key);
        if (audioClip != null)
        {
            AddAmbianceSound(audioClip, temporality);
            return _currentAmbianceSources[temporality].Last();
        }
        return null;
    }

    /// <summary>
    /// Arrête un son d'ambiance via sa clé pour une temporalité donnée.
    /// </summary>
    public void StopAmbianceSoundByKey(string key, Temporality temporality)
    {
        List<AudioSource> sources = _currentAmbianceSources[temporality];

        for (int i = sources.Count - 1; i >= 0; i--)
        {
            AudioSource audioSource = sources[i];
            string audioClipKey = audioSource.clip.name.Split(" ")[1]; // <-- attention à ce parsing
            if (audioClipKey == key)
            {
                StartCoroutine(FadeOutAudio(audioSource, _fadeOutDuration));
                sources.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Stop tous les sons d'ambiance dans toutes les temporalités.
    /// </summary>
    public void StopAllAmbiances()
    {
        foreach (Temporality temporality in System.Enum.GetValues(typeof(Temporality)))
        {
            StopAllAmbianceSources(temporality);
        }
    }

    #endregion

    #region SoundFX

    /// <summary>
    /// Joue un son ponctuel à une position donnée, pour une temporalité.
    /// </summary>
    private AudioSource PlaySoundFXClip(AudioClip audioClip, Temporality temporality, Vector3 spawnPosition, float volume = 1.0f)
    {
        AudioSource audioSource = CreateSoundFXSource(spawnPosition);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.outputAudioMixerGroup = _sfxMixerGroups[temporality];
        PlayAndDestroy(audioSource, audioClip.length);
        return audioSource;
    }

    /// <summary>
    /// Joue un son ponctuel par clé à une position donnée, pour une temporalité.
    /// </summary>
    public AudioSource PlaySoundFXClipByKey(string key, Temporality temporality, Vector3 spawnPosition, float volume = 1.0f)
    {
        var audioClip = GetSFXByKey(key);
        if (audioClip != null)
        {
            return PlaySoundFXClip(audioClip, temporality, spawnPosition, volume);
        }
        return null;
    }

    /// <summary>
    /// Joue un SFX 2D sur un AudioSource libre, pour une temporalité.
    /// </summary>
    public AudioSource PlaySoundFXClipByKey(string key, Temporality temporality, float volume = 1.0f)
    {
        AudioClip audioClip = GetSFXByKey(key);
        if (audioClip != null)
        {
            AudioSource audioSource = GetAvailableAudioSource();
            audioSource.volume = volume;
            audioSource.loop = false;
            audioSource.outputAudioMixerGroup = _sfxMixerGroups[temporality];
            audioSource.clip = audioClip;
            audioSource.Play();
            return audioSource;
        }

        return null;
    }

    /// <summary>
    /// Joue un son aléatoire parmi une liste, pour une temporalité.
    /// </summary>
    public void PlayRandomSoundFXClipByKeys(string[] keys, Temporality temporality, Vector3 spawnPosition, float volume = 1.0f)
    {
        List<AudioClip> clips = new List<AudioClip>();
        foreach (var key in keys)
        {
            var clip = GetSFXByKey(key);
            if (clip != null)
            {
                clips.Add(clip);
            }
        }

        if (clips.Count > 0)
        {
            int rand = UnityEngine.Random.Range(0, clips.Count);
            PlaySoundFXClip(clips[rand], temporality, spawnPosition, volume);
        }
    }

    /// <summary>
    /// Crée une AudioSource à la position donnée.
    /// </summary>
    private AudioSource CreateSoundFXSource(Vector3 spawnPosition)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnPosition, Quaternion.identity);
        return audioSource;
    }

    /// <summary>
    /// Joue une source audio et la détruit à la fin du clip.
    /// </summary>
    private void PlayAndDestroy(AudioSource audioSource, float clipLength)
    {
        audioSource.Play();
        Destroy(audioSource.gameObject, clipLength);
    }

    #endregion

    #region Coroutines

    /// <summary>
    /// Fait une transition douce entre la musique en cours et une musique donnée d'une temporalité donnée.
    /// </summary>
    /// <param name="newClip">Nouvelle musique.</param>
    /// <param name="temporality">Temporalité affectée.</param>
    private IEnumerator FadeOutInMusic(AudioClip newClip, Temporality temporality, float volume = 1.0f)
    {
        if (_currentMusicSources[temporality] != null)
        {
            yield return StartCoroutine(FadeOutAudio(_currentMusicSources[temporality], _fadeOutDuration));
        }

        AudioSource newMusicSource = GetAvailableAudioSource();
        newMusicSource.outputAudioMixerGroup = _musicMixerGroups[temporality];
        newMusicSource.clip = newClip;
        newMusicSource.loop = true;
        newMusicSource.volume = 0.0f;
        newMusicSource.Play();

        _currentMusicSources[temporality] = newMusicSource;

        yield return StartCoroutine(FadeInAudio(newMusicSource, _fadeInDuration, volume));
    }

    #endregion
}
