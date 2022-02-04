using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour {
    public int playerIndex;
    public SLayout layout;
    public SLayout scoreLayout;
    public SLayout turnsLayout;

    void Update () {
        
        if(GameController.Instance.players.ContainsIndex(playerIndex)) {
            layout.color = GameController.Instance.currentPlayer.playerIndex == playerIndex ? Color.black : Color.black.WithAlpha(0.2f);
            layout.groupAlpha = GameController.Instance.currentPlayer.playerIndex == playerIndex ? 1 : 0.5f;
            turnsLayout.textMeshPro.text = "Turns: "+GameController.Instance.players[playerIndex].turns;
            scoreLayout.textMeshPro.text = "Points: "+GameController.Instance.players[playerIndex].coins;
        } else {
            layout.groupAlpha = 0;
        }
    }
}
