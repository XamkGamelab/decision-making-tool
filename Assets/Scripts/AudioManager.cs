using UnityEngine;
using UnityEngine.Audio;

namespace JAS.MediDeci
{
    public class AudioManager : GenericSingleton<AudioManager>
    {
        [Header("Audio Clips")]
        public AudioClip clickButtonSound;

        [Header("Mixer")]
        public AudioMixerGroup soundMixerGroup; // assign Sound group here

        private AudioSource _audioSource;

        protected override bool ShouldPersist => true;

        protected override void Awake()
        {
            base.Awake();

            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
                _audioSource = gameObject.AddComponent<AudioSource>();

            _audioSource.playOnAwake = false;

            // Ensure output is routed to Sound mixer
            if (soundMixerGroup != null)
                _audioSource.outputAudioMixerGroup = soundMixerGroup;

            // Apply saved volume immediately
            ApplySavedVolume();
        }

        public void PlayAudioClip(AudioClip clip)
        {
            if (_audioSource == null || clip == null)
                return;

            _audioSource.PlayOneShot(clip);
        }

        public void PlayButtonClick()
        {
            PlayAudioClip(clickButtonSound);
        }

        /// <summary>
        /// Apply saved SoundVolume from PlayerPrefs to the mixer
        /// </summary>
        private void ApplySavedVolume()
        {
            int savedSound = PlayerPrefs.GetInt("SoundVolume", 10);
            float dB = (savedSound == 0) ? -80f : Mathf.Lerp(-30f, 0f, savedSound / 10f);
            if (soundMixerGroup != null && soundMixerGroup.audioMixer != null)
                soundMixerGroup.audioMixer.SetFloat("SoundVolume", dB);
        }
    }
}