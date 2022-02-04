using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAudio : MonoSingleton<PickupAudio> {
    public AudioSource audioSource;
    public AudioClip win;
    public AudioClip fail;
    public Prototype audioSourcePrototype;
    public void Play(int combo) {
        audioSource.clip = win;
        var pitch = 1f;
        for(int i = 0; i < combo; i++) 
            pitch *= 1.122324159021407f;

        audioSource.pitch = pitch;
        audioSource.Play();
    }
    public void PlayFail() {
        audioSource.clip = fail;
        audioSource.pitch = 1;
        audioSource.Play();
    }
}