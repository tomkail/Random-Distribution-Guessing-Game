using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerModel {
    public int playerIndex;
    public int turns;
    public int coins;

    public PlayerModel (int playerIndex) {
        this.playerIndex = playerIndex;
    }
}