using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSheetAnimator : MonoBehaviour {
    public Sprite[] sprites;
    public Image image;
    public float FPS = 30;
    public bool loop = true;
    [SerializeField, Disable]
    float currentFrame;
    [SerializeField, Disable]
    int currentFrameIndex;
    [SerializeField, Disable]
    int loopedCurrentFrameIndex;

    void OnEnable () {
        Reset();
    }

    public void Reset () {
        currentFrame = 0;
        currentFrameIndex = 0;
    }
    void Update() {
        currentFrame += Time.deltaTime * FPS;
        currentFrameIndex = Mathf.FloorToInt(currentFrame);
        if(loop) {
            loopedCurrentFrameIndex = currentFrameIndex % sprites.Length;
        } else {
            loopedCurrentFrameIndex = currentFrameIndex = Mathf.Min(currentFrameIndex, sprites.Length-1);
        }
        image.sprite = sprites[loopedCurrentFrameIndex];
    }

    void OnValidate () {
        image.sprite = sprites[0];
    }
}