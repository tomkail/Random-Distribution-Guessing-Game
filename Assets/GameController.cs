using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoSingleton<GameController> {
    public bool animating;
    public int numPlayers = 1;
    public int currentPlayerIndex;
    public List<PlayerModel> players = new List<PlayerModel>();
    public PlayerModel currentPlayer => players.GetRepeating(currentPlayerIndex);
    public Sprite[] symbols;

    void OnEnable () {
        currentPlayerIndex = 0;
        players.Clear();
        for(int i = 0; i < numPlayers; i++) {
            players.Add(new PlayerModel(i));
        }
    }
}