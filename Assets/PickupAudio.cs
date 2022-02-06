using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAudio : MonoSingleton<PickupAudio> {
    public AudioSource audioSource;
    public AudioClip win;
    public AudioClip lose;
    public AudioClip nothing;
    public Prototype audioSourcePrototype;
    public void PlayGain(int combo) {
        audioSource.clip = win;
        var pitch = GetPitchForCombo(combo);
        audioSource.pitch = pitch;
        audioSource.Play();
    }
    public void PlayLose(int combo) {
        audioSource.clip = lose;
        var pitch = GetPitchForCombo(combo);
        audioSource.pitch = pitch;
        audioSource.Play();
    }
    public void PlayNothing() {
        audioSource.clip = nothing;
        audioSource.pitch = 1;
        audioSource.Play();
    }

    public static float GetPitchForCombo (int combo) {
        if(combo == 0 || combo == 1) return 1;
        return Mathf.Pow(1.122324159021407f, combo);
    }
}