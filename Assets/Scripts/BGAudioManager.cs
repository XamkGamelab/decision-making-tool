using UnityEngine;
using UnityEngine.SceneManagement;

namespace JAS.MediDeci
{
    /// <summary>
    /// Background music manager that plays appropriate music based on active scene.
    /// </summary>
    public class BGAudioManager : GenericSingleton<BGAudioManager>
    {
        [Header("Music Clips")]
        [Tooltip("Music for the main menu scene.")]
        public AudioClip mainMenuMusic;

        [Tooltip("Music for the game scene.")]
        public AudioClip gameMusic;
        private AudioSource _audioSource;

        // Optional: Expose current music clip for inspection/debugging
        public AudioClip CurrentClip => _audioSource != null ? _audioSource.clip : null;

        protected override void Awake()
        {
            base.Awake();

            EnsureAudioSource();

            // Clean up previous subscriptions to avoid duplicates
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void EnsureAudioSource()
        {
            if (_audioSource == null)
            {
                if (!TryGetComponent(out _audioSource))
                    _audioSource = gameObject.AddComponent<AudioSource>();

                _audioSource.loop = true;
                _audioSource.playOnAwake = false;
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Map scene name to audio clips
            switch (scene.name)
            {
                case "Main Menu":
                    PlayMusicSafe(mainMenuMusic);
                    break;

                case "Testi": // Replace with actual gameplay scene name(s)
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
            {
                Debug.LogWarning("[BGAudioManager] Attempted to play a null clip.");
                return;
            }

            EnsureAudioSource();

            // Avoid restarting same clip
            if (_audioSource.clip == clip && _audioSource.isPlaying)
                return;

            _audioSource.clip = clip;
            _audioSource.Play();
        }

        public void StopMusic()
        {
            if (_audioSource == null || !_audioSource.isPlaying)
                return;

            _audioSource.Stop();
            _audioSource.clip = null;
        }

        /// <summary>
        /// Static safe wrapper to call music from other scripts.
        /// </summary>
        public static void PlayMusicSafe(AudioClip clip)
        {
            if (Instance != null)
                Instance.PlayMusic(clip);
            else
                Debug.LogWarning("[BGAudioManager] Instance missing. Cannot play music.");
        }

        protected override void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}