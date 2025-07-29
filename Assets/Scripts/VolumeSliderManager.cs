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
            musicSlider.value = PlayerPrefs.GetInt("MusicVolume", 10);
            soundSlider.value = PlayerPrefs.GetInt("SoundVolume", 10);

            musicLabel.text = PlayerPrefs.GetInt("MusicVolume").ToString();
            soundLabel.text = PlayerPrefs.GetInt("SoundVolume").ToString();

            ApplyVolume(Mathf.RoundToInt(musicSlider.value), musicParameter);
            ApplyVolume(Mathf.RoundToInt(soundSlider.value), soundParameter);

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
            });
        }

        private void ApplyVolume(int sliderValue, string parameter)
        {
            // Convert 0-10 scale to decibels (-80 dB = mute, 0 dB = full)
            float dB = (sliderValue == 0) ? -80f : Mathf.Lerp(-30f, 0f, sliderValue / 10f);
            audioMixer.SetFloat(parameter, dB);
        }

        private void UpdateLabel(TextMeshProUGUI label, int value)
        {
            label.text = value.ToString();
        }
    }
}