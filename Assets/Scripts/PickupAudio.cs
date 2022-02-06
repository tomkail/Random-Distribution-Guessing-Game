using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAudio : MonoSingleton<PickupAudio> {
    public AudioSource audioSource;
    public AudioClip win;
    public AudioClip lose;
    public AudioClip nothing;
    public Prototype audioSourcePrototype;

    void Update () {
        int num = 0;
        if(KeyboardInput.TryGetNumberKey(ref num)) {
            if(Input.GetKey(KeyCode.LeftShift)) num *= -1;
            audioSource.clip = win;
            var pitch = MusicUtils.GetPitchForMajorScale(num);
            audioSource.pitch = pitch;
            audioSource.Play();
        }
    }
    public void PlayGain(int combo) {
        audioSource.clip = win;
        var pitch = MusicUtils.GetPitchForMajorScale(combo);
        audioSource.pitch = pitch;
        audioSource.Play();
    }
    public void PlayLose(int combo) {
        audioSource.clip = lose;
        var pitch = MusicUtils.GetPitchForMinorScale(combo);
        audioSource.pitch = pitch;
        audioSource.Play();
    }
    public void PlayNothing() {
        audioSource.clip = nothing;
        audioSource.pitch = 1;
        audioSource.Play();
    }
}

static class MusicUtils {
    //12th root of 2
    static float semiToneFrequency = Mathf.Pow(2, 1f/12f);
    static int[] majorScaleIntervals = new int[] {2,2,1,2,2,2,1};
    static int[] minorScaleIntervals = new int[] {2,1,2,2,1,2,2};

    public static float GetPitchForChromaticScale (int indexInScale) {
        return GetPitchForScale(indexInScale);
    }
    public static float GetPitchForMajorScale (int indexInScale) {
        return GetPitchForScale(indexInScale, majorScaleIntervals);
    }
    public static float GetPitchForMinorScale (int indexInScale) {
        return GetPitchForScale(indexInScale, minorScaleIntervals);
    }

    // indexInScale is not degree - the 1st note in a scale has an index of 0. 7th note would be the 6th index.
    public static float GetPitchForScale (int indexInScale, int[] intervals = null) {
        var semiTones = GetNumSemiTonesInScale(0, indexInScale, intervals);
        return Mathf.Pow(semiToneFrequency, semiTones);
    }

    // Gets the number of semitones between notes in a scale.
    // If start index was 0, a degree of 0 would be the 1st, a degree of 6 would 
    public static int GetNumSemiTonesInScale (int startIndex, int steps, int[] intervals = null) {
        int semiTones = 0;
        var ascending = Mathf.Sign(steps) > 0;
        var pitch = 1f;
        for(int si = 0; si < Mathf.Abs(steps)-1; si++) {
            var intervalIndex = startIndex + (ascending ? si : (majorScaleIntervals.Length-1)-si);
            var interval = intervals.GetRepeating(intervalIndex);
            for(int ii = 0; ii < interval; ii++) {
                semiTones += ascending ? 1 : -1;
            }
            Debug.Log(si+" "+pitch+" "+steps +" "+intervalIndex+" "+interval+" "+semiToneFrequency);
        }
        return semiTones;
    }
}