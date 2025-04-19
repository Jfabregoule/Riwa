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

    [SerializeField] private AudioMixerGroup _musicPastMixerGroup; // Groupe de mixage pour la musique du pass�.
    [SerializeField] private AudioMixerGroup _musicPresentMixerGroup; // Groupe de mixage pour la musique du pr�sent.
    [SerializeField] private AudioMixerGroup _ambiancePastMixerGroup; // Groupe de mixage pour l'ambiance du pass�.
    [SerializeField] private AudioMixerGroup _ambiancePresentMixerGroup; // Groupe de mixage pour l'ambiance du present.
    [SerializeField] private AudioMixerGroup _sfxPastMixerGroup; // Groupe de mixage pour les SFX du pass�.
    [SerializeField] private AudioMixerGroup _sfxPresentMixerGroup; // Groupe de mixage pour les SFX du pr�sent.

    private Dictionary<Temporality, AudioMixerGroup> _musicMixerGroups; // Dictionnaire des musiques groups contenant les groupes de chaque temporalit� associ� � leur cl�.
    private Dictionary<Temporality, AudioMixerGroup> _ambianceMixerGroups; // Dictionnaire des ambiance groups contenant les groupes de chaque temporalit� associ� � leur cl�.
    private Dictionary<Temporality, AudioMixerGroup> _sfxMixerGroups; // Dictionnaire des sfx groups contenant les groupes de chaque temporalit� associ� � leur cl�.

    [Header("Temporality Starting Sounds")]
    [Space(10)]

    [SerializeField] private AudioClip _startingPastMusic; // Musique de d�part du pass�.
    [SerializeField] private AudioClip _startingPresentMusic; // Musique de d�part du pr�sent.
    [SerializeField] private List<AudioClip> _startingPastAmbianceSounds; // Sons d'ambiance de d�part.
    [SerializeField] private List<AudioClip> _startingPresentAmbianceSounds; // Sons d'ambiance de d�part.

    private AudioSource _currentPastMusicSource; // Source audio actuellement utilis�e pour la musique du pass�.
    private AudioSource _currentPresentMusicSource; // Source audio actuellement utilis�e pour la musique du pr�sent.
    private List<AudioSource> _currentPastAmbianceSources; // Liste des sources audio d'ambiance du pass�.
    private List<AudioSource> _currentPresentAmbianceSources; // Liste des sources audio d'ambiance du pr�sent.

    private Dictionary<Temporality, AudioSource> _currentMusicSources;
    private Dictionary<Temporality, List<AudioSource>> _currentAmbianceSources;

    #endregion

    #region Main Functions

    /// <summary>
    /// Initialisation du syst�me sonore et chargement des pistes audio.
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
    /// Change la musique actuelle avec la musique donn�e.
    /// </summary>
    /// <param name="audioClip">Audioclip de la nouvelle musique.</param>
    private void ChangeMusic(AudioClip audioClip, Temporality temporality, float volume = 1.0f)
    {
        StartCoroutine(FadeOutInMusic(audioClip, temporality, volume));
    }

    /// <summary>
    /// Change la musique actuelle avec la musique trouv�e avec la cl� donn�e pour une temporalit� donn�e.
    /// </summary>
    /// <param name="key">Cl� de la nouvelle musique.</param>
    /// <param name="temporality">Temporalit� affect�e.</param>
    public void ChangeMusicByKey(string key, Temporality temporality, float volume = 1.0f)
    {
        ChangeMusic(GetMusicByKey(key), temporality, volume);
    }

    /// <summary>
    /// Stop la musique en cours d'une temporalit� donn�e.
    /// </summary>
    /// <param name="temporality">Temporalit� affect�e.</param>
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
    /// Stop les sons d'ambiances en cours d'une temporalit� donn�e.
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
    /// Ajoute un nouveau son d'ambiance pour une temporalit� donn�e.
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
    /// Ajoute plusieurs sons d'ambiance pour une temporalit� donn�e.
    /// </summary>
    private void AddAmbianceSounds(List<AudioClip> audioClips, Temporality temporality, float volume = 1.0f)
    {
        foreach (AudioClip audioClip in audioClips)
        {
            AddAmbianceSound(audioClip, temporality, volume);
        }
    }

    /// <summary>
    /// Ajoute un nouveau son d'ambiance via sa cl� pour une temporalit� donn�e.
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
    /// Arr�te un son d'ambiance via sa cl� pour une temporalit� donn�e.
    /// </summary>
    public void StopAmbianceSoundByKey(string key, Temporality temporality)
    {
        List<AudioSource> sources = _currentAmbianceSources[temporality];

        for (int i = sources.Count - 1; i >= 0; i--)
        {
            AudioSource audioSource = sources[i];
            string audioClipKey = audioSource.clip.name.Split(" ")[1]; // <-- attention � ce parsing
            if (audioClipKey == key)
            {
                StartCoroutine(FadeOutAudio(audioSource, _fadeOutDuration));
                sources.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Stop tous les sons d'ambiance dans toutes les temporalit�s.
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
    /// Joue un son ponctuel � une position donn�e, pour une temporalit�.
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
    /// Joue un son ponctuel par cl� � une position donn�e, pour une temporalit�.
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
    /// Joue un SFX 2D sur un AudioSource libre, pour une temporalit�.
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
    /// Joue un son al�atoire parmi une liste, pour une temporalit�.
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
    /// Cr�e une AudioSource � la position donn�e.
    /// </summary>
    private AudioSource CreateSoundFXSource(Vector3 spawnPosition)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnPosition, Quaternion.identity);
        return audioSource;
    }

    /// <summary>
    /// Joue une source audio et la d�truit � la fin du clip.
    /// </summary>
    private void PlayAndDestroy(AudioSource audioSource, float clipLength)
    {
        audioSource.Play();
        Destroy(audioSource.gameObject, clipLength);
    }

    #endregion

    #region Coroutines

    /// <summary>
    /// Fait une transition douce entre la musique en cours et une musique donn�e d'une temporalit� donn�e.
    /// </summary>
    /// <param name="newClip">Nouvelle musique.</param>
    /// <param name="temporality">Temporalit� affect�e.</param>
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
