using UnityEngine;

/// <summary>
/// Centralized manager for UI and SFX audio. Uses a singleton pattern.
/// </summary>
namespace JAS.MediDeci
{
    public class AudioManager : GenericSingleton<AudioManager>
    {
        [Header("Audio Clips")]
        [Tooltip("Click sound for buttons.")]
        public AudioClip clickButtonSound;
        private AudioSource _audioSource;

        // Override to prevent persistence if needed
        protected override bool ShouldPersist => true;

        protected override void Awake()
        {
            base.Awake();

            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
                _audioSource.playOnAwake = false;
            }
        }

        /// <summary>
        /// Plays a one-shot audio clip. Logs warnings if clip is null.
        /// </summary>
        public void PlayAudioClip(AudioClip clip)
        {
            if (_audioSource == null)
            {
                Debug.LogWarning("[AudioManager] AudioSource missing.");
                return;
            }

            if (clip == null)
            {
                Debug.LogWarning("[AudioManager] Tried to play a null clip.");
                return;
            }

            _audioSource.PlayOneShot(clip);
        }

        /// <summary>
        /// Convenience method to play the default button click sound.
        /// </summary>
        public void PlayButtonClick()
        {
            PlayAudioClip(clickButtonSound);
        }
    }
}