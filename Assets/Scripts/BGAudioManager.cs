using UnityEngine;
using UnityEngine.SceneManagement;

public class BGAudioManager : GenericSingleton<BGAudioManager>
{
    public AudioClip mainMenuMusic;
    public AudioClip gameMusic;

    private AudioSource audioSource;

    public override void Awake()
    {
        base.Awake();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
        }

        // Register scene loaded callback ONLY if not already
        SceneManager.sceneLoaded -= OnSceneLoaded; // remove first, in case it's been registered before
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Main Menu":
                PlayMusicSafe(mainMenuMusic);
                break;

            case "Testi": // your game scene
                PlayMusicSafe(gameMusic);
                break;

            default:
                StopMusic();
                break;
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null)
            return;

        if (audioSource == null)
        {
            Debug.LogWarning("audioSource is null. Reinitializing.");
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
        }

        // Avoid restarting if the same clip is already playing
        if (audioSource.clip == clip && audioSource.isPlaying)
            return;

        audioSource.clip = clip;
        audioSource.Play();
    }

    public void StopMusic()
    {
        if (audioSource == null)
            return;

        audioSource.Stop();
        audioSource.clip = null;
    }

    public static void PlayMusicSafe(AudioClip clip)
    {
        if (instance != null && instance.gameObject != null)
        {
            instance.PlayMusic(clip);
        }
        else
        {
            Debug.LogWarning("BGAudioManager instance is null or destroyed. Cannot play music.");
        }
    }
}