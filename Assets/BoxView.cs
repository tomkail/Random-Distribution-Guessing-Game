using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxView : MonoBehaviour {
    public SLayout layout;
    public Distribution distribution;
    public Button button;
    public Image symbolImage;
    public Prototype coinPrototype;

    public void Init () {
        symbolImage.transform.eulerAngles = new Vector3(0,0,Random.Range(0,8)*45);
    }

    void Update () {
        button.interactable = !GameController.Instance.animating;
    }

    void OnEnable () {
        button.onClick.AddListener(OnClick);
    }

    void OnDisable () {
        button.onClick.RemoveListener(OnClick);
    }

    void OnClick () {
        GameController.Instance.animating = true;
        GameController.Instance.currentPlayer.turns++;
        var result = distribution.Sample();
        var delay = 0f;
        if(result == 0) {
            PickupAudio.Instance.PlayFail();
            layout.After(0.5f, () => {
                GameController.Instance.currentPlayerIndex++;
                GameController.Instance.animating = false;
            });
        } else {
            for(int i = 0; i < result; i++) {
                var coin = coinPrototype.Instantiate<SLayout>();
                coin.alpha = 0;
                var combo = i+1;
                coin.After(delay, () => {
                    GameController.Instance.currentPlayer.coins++;
                    PickupAudio.Instance.Play(combo);
                });
                coin.Animate(0.15f, delay, () => {
                    coin.alpha = 1;
                });
                coin.Animate(0.8f, delay, AnimationCurveX.easeOut, () => {
                    coin.center = new Vector2(coin.parentRect.width*0.5f + Random.Range(-30,30),150);
                });
                coin.After(delay+0.6f, () => {
                    coin.Animate(0.3f, () => {
                        coin.alpha = 0;
                    }).Then(() => {
                        coin.GetComponent<Prototype>().ReturnToPool();
                    });
                });
                delay += 0.4f-(i*0.025f);
            }
            
            layout.After(delay, () => {
                GameController.Instance.currentPlayerIndex++;
                GameController.Instance.animating = false;
            });

        }
    }
}
