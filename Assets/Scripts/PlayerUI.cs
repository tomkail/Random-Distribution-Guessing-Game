using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour {
    public int playerIndex;
    public SLayout layout;
    public SLayout scoreLayout;
    public SLayout turnsLayout;
    public SLayout livesLayout;
    public Prototype lifePrototype;
    public List<LifeUI> lifeUIs;
    public GameObject strikethrough;

    void Update () {
        if(GameController.Instance.players.ContainsIndex(playerIndex)) {
            if(GameController.Instance.settings.initialLives != lifePrototype.instances.Count()) {
                // CreateLives();
            }

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
            // turnsLayout.textMeshPro.text = "Turns: "+player.turn;
            scoreLayout.textMeshPro.text = "Points: "+player.coins;

            int i = 0;
            foreach(var _lifeUI in lifePrototype.instances) {
                var lifeUI = _lifeUI.GetComponent<LifeUI>();
                lifeUI.SetFilled(i < player.lives);
                i++;
            }
        } else {
            layout.groupAlpha = 0;
            // lifePrototype.ReturnAllToPool();
            foreach(var life in lifeUIs) life.GetComponent<Prototype>().ReturnToPool();
        }
    }

    void CreateLives () {
        // lifePrototype.ReturnAllToPool();
        foreach(var life in lifeUIs) life.GetComponent<Prototype>().ReturnToPool();
        
        var x = 0f;
        for(int i = 0; i < GameController.Instance.settings.initialLives; i++) {
            var lifeUI = lifePrototype.Instantiate<LifeUI>();
            lifeUI.layout.x = x;
            x += lifeUI.layout.width;
            lifeUIs.Add(lifeUI);
        }
    }
}