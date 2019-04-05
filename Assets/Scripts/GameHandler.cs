using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField]
    PlayerController player1;

    [SerializeField]
    PlayerController player2;

    [SerializeField]
    RoundPoint player1RoundPoint1;

    [SerializeField]
    RoundPoint player1RoundPoint2;

    [SerializeField]
    RoundPoint player2RoundPoint1;

    [SerializeField]
    RoundPoint player2RoundPoint2;

    [SerializeField]
    VictoryPoints player1VictoryPoints;

    [SerializeField]
    VictoryPoints player2VictoryPoints;

    Timer gameTimer;

    Timer roundoverTimer;

    // Start is called before the first frame update
    void Start()
    {
        gameTimer = GetComponent<Timer>();
        roundoverTimer = gameObject.AddComponent<Timer>();
        roundoverTimer.startingTime = 1f;
        roundoverTimer.PauseTimer();
        ResetGame();
    }

    // Update is called once per frame
    void Update()
    {
        if(RoundIsFinished())
        {
            /*roundoverTimer.ResetTimer();

            SlowDownTime(.5f);

            if (!roundoverTimer.TimerFinished())
            {
                return;
            }
            else
            {
                roundoverTimer.PauseTimer();
                roundoverTimer.ResetTimer();
            }*/

            PlayerController winner = DetermineWinner();

            if(winner == null)
            {
                ResetRound();
                return;
            }

            if (RewardPlayer(winner))
            {
                ResetGame();
            }
            else
            {
                ResetRound();
            }

            //SlowDownTime(1f);
        }
    }

    bool RoundIsFinished()
    {
        if (player1.CurrentHealth <= 0 || player2.CurrentHealth <= 0 || gameTimer.TimerFinished())
        {
            return true;
        }
        return false;
    }

    bool RewardPlayer(PlayerController player)
    {
        if(player == player1)
        {
            if (!player1RoundPoint1.Active && !player1RoundPoint2.Active)
            {
                player1RoundPoint1.Active = true;
            }
            else if (player1RoundPoint1.Active && !player1RoundPoint2.Active)
            {
                player1RoundPoint2.Active = true;
            }
            else if (player1RoundPoint1.Active && player1RoundPoint2.Active)
            {
                player1VictoryPoints.victoryPoints++;
                return true;
            }
        }
        else if(player == player2)
        {
            if (!player2RoundPoint1.Active && !player2RoundPoint2.Active)
            {
                player2RoundPoint1.Active = true;
            }
            else if (player2RoundPoint1.Active && !player2RoundPoint2.Active)
            {
                player2RoundPoint2.Active = true;
            }
            else if (player2RoundPoint1.Active && player2RoundPoint2.Active)
            {
                player2VictoryPoints.victoryPoints++;
                return true;
            }
        }
        return false;
    }

    PlayerController DetermineWinner()
    {
        if (player1.CurrentHealth > player2.CurrentHealth)
        {
            return player1;
        }
        else if(player1.CurrentHealth < player2.CurrentHealth)
        {
            return player2;
        }
        return null;
    }

    void ResetRound()
    {
        // Reset locations of players
        player1.transform.position = new Vector3(-5f, 0.3f);
        player2.transform.position = new Vector3(5f, 0.3f);

        // Reset health bars
        player1.CurrentHealth = player1.MaximumHealth;
        player2.CurrentHealth = player2.MaximumHealth;

        // Reset timer
        gameTimer.ResetTimer();
    }

    void ResetGame()
    {
        ResetRound();

        // Reset round points
        player1RoundPoint1.Active = false;
        player1RoundPoint2.Active = false;
        player2RoundPoint1.Active = false;
        player2RoundPoint2.Active = false;
    }

    void SlowDownTime(float scale)
    {
        Time.timeScale = scale;
    }
}
