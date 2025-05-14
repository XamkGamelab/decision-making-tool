using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGAudioManager : GenericSingleton<BGAudioManager>
{
    public AudioClip BGMusic;

    private AudioSource AudioSourceBG;

    public void PlayAudioClip(AudioClip clip)
    {
        AudioSourceBG.clip = clip;
        AudioSourceBG.PlayOneShot(clip);

    }

    // Play background music and make it loop
    public void PlayBackgroundMusic()
    {
        AudioSourceBG.clip = BGMusic;
        AudioSourceBG.loop = true;  // Set to loop
        AudioSourceBG.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        AudioSourceBG = gameObject.AddComponent<AudioSource>();

        // Only start background music if the correct scene is active
        if (SceneManager.GetActiveScene().name == "Testi")
        {
            PlayBackgroundMusic();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Testi" && !AudioSourceBG.isPlaying)
        {
            PlayBackgroundMusic();
        }

        if (SceneManager.GetActiveScene().name == "Main Menu" && AudioSourceBG.isPlaying)
        {
            AudioSourceBG.Stop();
        }

    }
}
