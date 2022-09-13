using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public Player[] players;

    private void Awake()
    {
        Instance = this;
    }

    public Player GetRandomEnemyPlayer(Player me)
    {
        Player ranPlayer = players[Random.Range(0, players.Length)];

        while (ranPlayer == me)
        {
            ranPlayer = players[Random.Range(0, players.Length)];
        }

        return ranPlayer;
    }

    public void UnitDeathCheck()
    {
        int remainingPlayers = 0;
        Player winner = null;

        for (int x = 0; x < players.Length; x++)
        {
            if (players[x].units.Count > 0)
            {
                remainingPlayers++;
                winner = players[x];
            }
        }

        if (remainingPlayers != 1)
        {
            return;
        }
        
        EndScreenUI.Instance.SetEndScreen(winner.isMe);
    }
}
