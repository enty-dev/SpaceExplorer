using UnityEngine;
using System.Collections;

/// <summary>
/// Central Audio Manager for all audio categories.
/// Supports Music, SFX, UI, and Dialogue with separate volume controls.
/// </summary>
public class AudioManager : MonoBehaviour
{
    // Audio Categories
    public enum AudioCategory { Music, SFX, UI, Dialogue }

    private static AudioManager instance;
    private AudioSource musicSourceA;
    private AudioSource musicSourceB;
    private AudioSource currentSource;
    private AudioSource nextSource;

    [Header("Music Clips")]
    public AudioClip stageMusic;
    public AudioClip bossMusic;

    [Header("Volume Settings")]
    public float musicVolume = 0.7f;
    public float sfxVolume = 0.8f;
    public float uiVolume = 0.6f;
    public float dialogueVolume = 0.9f;
    public float fadeDuration = 2f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance.gameObject); // ✅ Destroy old AudioManager
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // ✅ Persist this one

        musicSourceA = gameObject.AddComponent<AudioSource>();
        musicSourceB = gameObject.AddComponent<AudioSource>();

        musicSourceA.loop = true;
        musicSourceB.loop = true;

        musicSourceA.volume = 0f;
        musicSourceB.volume = 0f;

        musicSourceA.playOnAwake = false;
        musicSourceB.playOnAwake = false;

        currentSource = musicSourceA;
        nextSource = musicSourceB;

        if (stageMusic != null)
        {
            StartCoroutine(CrossfadeTo(stageMusic)); // ✅ Begin playback
        }
    }


    public static void PlayStageMusic()
    {
        if (instance != null && instance.stageMusic != null)
        {
            instance.StopAllCoroutines();
            AudioManager.ChangeMusic(instance.stageMusic, instance.musicVolume); // ✅ STATIC CALL
        }
    }



    private IEnumerator CrossfadeTo(AudioClip newClip)
    {
        nextSource.clip = newClip;
        nextSource.volume = 0f;
        nextSource.Play();

        float timer = 0f;
        while (timer < fadeDuration)
        {
            float t = timer / fadeDuration;
            currentSource.volume = Mathf.Lerp(musicVolume, 0, t);
            nextSource.volume = Mathf.Lerp(0, musicVolume, t);
            timer += Time.deltaTime;
            yield return null;
        }

        currentSource.Stop();
        AudioSource temp = currentSource;
        currentSource = nextSource;
        nextSource = temp;

        currentSource.volume = musicVolume;
    }

    /// <summary>
    /// Call this to fade from current music to the boss track.
    /// </summary>
    public static void TransitionToBossMusic()
    {
        if (instance != null && instance.bossMusic != null)
        {
            instance.StopAllCoroutines();
            instance.StartCoroutine(instance.CrossfadeTo(instance.bossMusic));
        }
    }

    /// <summary>
    /// Play 2D sound effect at position with optional volume.
    /// </summary>
    public static void PlayClip(AudioClip clip, Vector3 position, float volume = 1f, AudioCategory category = AudioCategory.SFX)
    {
        if (clip == null) return;
        if (instance == null) return;

        // Apply category volume multiplier
        float categoryVolume = GetCategoryVolume(category);
        float finalVolume = volume * categoryVolume;

        GameObject tempGO = new GameObject($"Audio_{category}");
        tempGO.transform.position = position;

        AudioSource source = tempGO.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = finalVolume;
        source.Play();

        Destroy(tempGO, clip.length);
    }

    /// <summary>
    /// Play a clip in the world with 3D audio.
    /// </summary>
    public static void PlayClip3D(AudioClip clip, Vector3 position, float volume = 1f, AudioCategory category = AudioCategory.SFX)
    {
        if (clip == null) return;
        if (instance == null) return;

        float categoryVolume = GetCategoryVolume(category);
        float finalVolume = volume * categoryVolume;

        GameObject tempGO = new GameObject($"Audio3D_{category}");
        tempGO.transform.position = position;

        AudioSource source = tempGO.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = finalVolume;
        source.spatialBlend = 1f; // 3D audio
        source.Play();

        Destroy(tempGO, clip.length);
    }

    /// <summary>
    /// Change music immediately (no fade).
    /// </summary>
    public static void ChangeMusic(AudioClip newMusic, float volume = 0.5f)
    {
        if (instance != null)
        {
            instance.StopAllCoroutines();
            instance.musicSourceA.Stop();
            instance.musicSourceB.Stop();

            instance.currentSource = instance.musicSourceA;
            instance.currentSource.clip = newMusic;
            instance.currentSource.loop = true;
            instance.musicVolume = volume;
            instance.currentSource.volume = volume;
            instance.currentSource.Play();
        }
    }

    /// <summary>
    /// Stop all music playback.
    /// </summary>
    public static void StopMusic()
    {
        if (instance != null)
        {
            instance.StopAllCoroutines();
            instance.musicSourceA.Stop();
            instance.musicSourceB.Stop();
        }
    }

    /// <summary>
    /// Set global music volume (affects fade-in/out target).
    /// </summary>
    public static void SetMusicVolume(float volume)
    {
        if (instance != null)
        {
            instance.musicVolume = Mathf.Clamp01(volume);
            instance.currentSource.volume = instance.musicVolume;
        }
    }

    /// <summary>
    /// Set SFX volume.
    /// </summary>
    public static void SetSFXVolume(float volume)
    {
        if (instance != null)
        {
            instance.sfxVolume = Mathf.Clamp01(volume);
        }
    }

    /// <summary>
    /// Set UI sound volume.
    /// </summary>
    public static void SetUIVolume(float volume)
    {
        if (instance != null)
        {
            instance.uiVolume = Mathf.Clamp01(volume);
        }
    }

    /// <summary>
    /// Set Dialogue volume.
    /// </summary>
    public static void SetDialogueVolume(float volume)
    {
        if (instance != null)
        {
            instance.dialogueVolume = Mathf.Clamp01(volume);
        }
    }

    /// <summary>
    /// Get volume for a specific category.
    /// </summary>
    public static float GetCategoryVolume(AudioCategory category)
    {
        if (instance == null) return 1f;

        return category switch
        {
            AudioCategory.Music => instance.musicVolume,
            AudioCategory.SFX => instance.sfxVolume,
            AudioCategory.UI => instance.uiVolume,
            AudioCategory.Dialogue => instance.dialogueVolume,
            _ => 1f
        };
    }

    public static float GetVolume()
    {
        return instance != null ? instance.musicVolume : 0.5f;
    }

    public static void DestroyInstance()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            instance = null;
        }
    }
}
