using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

namespace JAS.MediDeci
{
    public class VolumeSliderManager : MonoBehaviour
    {
        [Header("UI Sliders")]
        public Slider musicSlider;
        public Slider soundSlider;
        [Space]
        public TextMeshProUGUI musicLabel;
        public TextMeshProUGUI soundLabel;

        [Header("Audio Mixer")]
        public AudioMixer audioMixer;
        public string musicParameter = "MusicVolume";
        public string soundParameter = "SoundVolume";

        private const int minVolume = 0;
        private const int maxVolume = 10;

        private void Start()
        {
            // Enforce snapping
            musicSlider.wholeNumbers = true;
            musicSlider.minValue = minVolume;
            musicSlider.maxValue = maxVolume;

            soundSlider.wholeNumbers = true;
            soundSlider.minValue = minVolume;
            soundSlider.maxValue = maxVolume;

            // Load saved values
            int savedMusic = PlayerPrefs.GetInt("MusicVolume", 10);
            int savedSound = PlayerPrefs.GetInt("SoundVolume", 10);

            savedMusic = Mathf.Clamp(savedMusic, minVolume, maxVolume);
            savedSound = Mathf.Clamp(savedSound, minVolume, maxVolume);

            musicSlider.value = savedMusic;
            soundSlider.value = savedSound;

            musicLabel.text = savedMusic.ToString();
            soundLabel.text = savedSound.ToString();

            // Apply saved volume immediately
            ApplyVolume(savedMusic, musicParameter);
            ApplyVolume(savedSound, soundParameter);

            // Add listeners
            musicSlider.onValueChanged.AddListener((value) =>
            {
                int intValue = Mathf.RoundToInt(value);
                ApplyVolume(intValue, musicParameter);
                UpdateLabel(musicLabel, intValue);
                PlayerPrefs.SetInt("MusicVolume", intValue);
            });

            soundSlider.onValueChanged.AddListener((value) =>
            {
                int intValue = Mathf.RoundToInt(value);
                ApplyVolume(intValue, soundParameter);
                UpdateLabel(soundLabel, intValue);
                PlayerPrefs.SetInt("SoundVolume", intValue);

                // Also immediately update AudioManager output
                if (AudioManager.Instance != null && AudioManager.Instance.soundMixerGroup != null)
                {
                    float dB = (intValue == 0) ? -80f : Mathf.Lerp(-30f, 0f, intValue / 10f);
                    AudioManager.Instance.soundMixerGroup.audioMixer.SetFloat("SoundVolume", dB);
                }
            });
        }

        private void ApplyVolume(int sliderValue, string parameter)
        {
            float dB = (sliderValue == 0) ? -80f : Mathf.Lerp(-30f, 0f, sliderValue / 10f);
            audioMixer.SetFloat(parameter, dB);
        }

        private void UpdateLabel(TextMeshProUGUI label, int value)
        {
            label.text = value.ToString();
        }
    }
}