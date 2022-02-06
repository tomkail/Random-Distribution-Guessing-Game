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
    public Prototype skullPrototype;

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
            PickupAudio.Instance.PlayNothing();
            layout.After(0.5f, () => {
                GameController.Instance.currentPlayerIndex++;
                GameController.Instance.animating = false;
            });
        } else if(result > 0) {
            for(int i = 0; i < result; i++) {
                var coin = coinPrototype.Instantiate<SLayout>();
                coin.alpha = 0;
                var combo = i+1;
                coin.After(delay, () => {
                    GameController.Instance.currentPlayer.coins++;
                    PickupAudio.Instance.PlayGain(combo);
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
        } else if(result < 0) {
            for(int i = 0; i < Mathf.Abs(result); i++) {
                var coin = skullPrototype.Instantiate<SLayout>();
                coin.alpha = 0;
                coin.scale = 1f;
                var combo = -i-1;
                coin.After(delay, () => {
                    GameController.Instance.currentPlayer.coins--;
                    PickupAudio.Instance.PlayLose(combo);
                });
                coin.Animate(0.1f, delay, () => {
                    coin.alpha = 1;
                });
                coin.Animate(1, delay, () => {
                    coin.scale = 1.5f;
                });
                coin.center = new Vector2(coin.parentRect.width*0.5f, coin.parentRect.height*0.5f) + Random.insideUnitCircle * 50;
                coin.Animate(0.8f, delay, AnimationCurveX.easeOut, () => {
                    // coin.center += new Vector2(0,40);
                });
                coin.After(delay+0.6f, () => {
                    coin.Animate(0.2f, () => {
                        coin.alpha = 0;
                    }).Then(() => {
                        coin.GetComponent<Prototype>().ReturnToPool();
                    });
                });
                delay += 0.4f+(i*0.09f);
            }
            
            layout.After(delay, () => {
                GameController.Instance.currentPlayerIndex++;
                GameController.Instance.animating = false;
            });
        }
    }
}
