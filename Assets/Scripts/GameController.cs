using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoSingleton<GameController> {
    public GameSettings settings;
    public bool animating;
    
    public List<PlayerModel> players = new List<PlayerModel>();
    public PlayerModel currentPlayer;
    public int round;
    public Sprite[] symbols;

    void OnEnable () {
        StartNewGame();
    }

    void Update () {
        if(Input.GetKeyDown(KeyCode.Backspace)) {
            StartNewGame();
        }
    }

    void StartNewGame () {
        players.Clear();
        for(int i = 0; i < settings.numPlayers; i++) {
            players.Add(new PlayerModel(i, settings.initialLives));
        }
        currentPlayer = players.First();
    }

    public void AdvanceTurn () {
        PlayerModel nextPlayer = null;
        var livingPlayers = 0;
        int currentPlayerIndex = players.IndexOf(currentPlayer);
        for(int i = 1; i < players.Count+1; i++) {
            var newPlayerIndex = players.GetRepeatingIndex(currentPlayerIndex+i);
            var player = players.GetRepeating(currentPlayerIndex+i);
            if(player.alive) {
                livingPlayers++;
                if(nextPlayer == null)
                    nextPlayer = player;
            }
        }
        if(nextPlayer == null || (currentPlayer == nextPlayer && livingPlayers > 1)) {
            GameOver();
        } else {
            currentPlayer = nextPlayer;
        }
    }

    void GameOver () {
        StartNewGame();
    }
}