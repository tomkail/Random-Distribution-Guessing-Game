using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerModel {
    public int playerIndex;
    public int lives;
    public bool alive => lives > 0;
    int _coins;
    public int coins {
        get {
            return _coins;
        } set {
            _coins = value;
            if(coins >= GameController.Instance.settings.lifeCost) BuyLife();
        }
    }

    public PlayerModel (int playerIndex, int lives) {
        this.playerIndex = playerIndex;
        this.lives = lives;
    }

    void BuyLife () {
        lives++;
        coins -= GameController.Instance.settings.lifeCost;
    }
}