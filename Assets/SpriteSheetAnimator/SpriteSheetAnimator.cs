using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSheetAnimator : MonoBehaviour {
    public Sprite[] sprites;
    public Image image;
    public float FPS = 30;
    [SerializeField, Disable]
    float currentFrame;
    [SerializeField, Disable]
    int currentFrameIndex;
    [SerializeField, Disable]
    int loopedCurrentFrameIndex;

    public void Stop () {
        currentFrame = 0;
        currentFrameIndex = 0;
    }
    void Update() {
        currentFrame += Time.deltaTime * FPS;
        currentFrameIndex = Mathf.FloorToInt(currentFrame);
        loopedCurrentFrameIndex = currentFrameIndex % sprites.Length;
        image.sprite = sprites[loopedCurrentFrameIndex];
    }

    void OnValidate () {
        image.sprite = sprites[0];
    }
}