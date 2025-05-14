using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : GenericSingleton<AudioManager>
{
    //public static AudioManager Instance { get; private set; }

    public AudioClip ClickButtonSound;


    private AudioSource AudioSource;

    public void PlayAudioClip(AudioClip clip)
    {
        AudioSource.clip = clip;
        AudioSource.PlayOneShot(clip);

    }

    // Start is called before the first frame update
    void Start()
    {
        AudioSource = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
