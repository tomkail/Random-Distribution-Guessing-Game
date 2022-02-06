using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour {
    public int playerIndex;
    public SLayout layout;
    public SLayout scoreLayout;
    public SLayout turnsLayout;
    public GameObject strikethrough;

    void Update () {
        if(GameController.Instance.players.ContainsIndex(playerIndex)) {
            var player = GameController.Instance.players[playerIndex];
            var isCurrentPlayer = GameController.Instance.currentPlayer == player;
            layout.color = isCurrentPlayer ? Color.black : Color.black.WithAlpha(0.2f);
            if(player.alive) {
                layout.groupAlpha = isCurrentPlayer ? 1 : 0.5f;
                strikethrough.SetActive(false);
            }
            else {
                layout.groupAlpha = 0.2f;
                strikethrough.SetActive(true);
            }
            turnsLayout.textMeshPro.text = "Lives: "+player.lives;
            scoreLayout.textMeshPro.text = "Points: "+player.coins;
        } else {
            layout.groupAlpha = 0;
        }
    }
}
