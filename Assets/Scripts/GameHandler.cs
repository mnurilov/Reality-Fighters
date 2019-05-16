using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    [SerializeField]
    MichaelPlayerController player1;

    [SerializeField]
    MichaelPlayerController player2;

    [SerializeField]
    RoundPoint player1RoundPoint1;

    [SerializeField]
    RoundPoint player1RoundPoint2;

    [SerializeField]
    RoundPoint player2RoundPoint1;

    [SerializeField]
    RoundPoint player2RoundPoint2;

    [SerializeField]
    Image VictoryMark1;

    [SerializeField]
    Image VictoryMark2;

    [SerializeField]
    Image VictoryMark3;

    [SerializeField]
    Image VictoryMark4;

    [SerializeField]
    VictoryPoints player1VictoryPoints;

    [SerializeField]
    VictoryPoints player2VictoryPoints;

    Timer gameTimer;

    Timer roundoverTimer;

    float victoryTimer;

    [SerializeField]
    float baseVictoryTimer;

    bool hasNotDisabled;

    bool timerStarted;

    bool TimerFinished()
    {
        if (victoryTimer < baseVictoryTimer && timerStarted)
        {
            //DisablePlayers();
            return false;
        }
        else
        {
            //victoryTimer = 0f;
            //ActivatePlayers();
            return true;
        }
    }

    void RunTimer()
    {
        if (!timerStarted)
        {
            return;
        }
        else
        {
            victoryTimer += Time.deltaTime;
        }
    }

    void DisablePlayers()
    {
        player1.DisableControls();
        player2.DisableControls();
    }

    void ActivatePlayers()
    {
        player1.ActivateControls();
        player2.ActivateControls();
    }

    void ResetMarks()
    {
        VictoryMark1.enabled = false;
        VictoryMark2.enabled = false;
        VictoryMark3.enabled = false;
        VictoryMark4.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameTimer = GetComponent<Timer>();
        roundoverTimer = gameObject.AddComponent<Timer>();
        roundoverTimer.startingTime = 1f;
        roundoverTimer.PauseTimer();
        hasNotDisabled = true;
        timerStarted = false;

        
        ResetMarks();
        ResetGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (!RoundIsFinished())
        {
            return;
        }
        if (RoundIsFinished())
        {
            if (timerStarted)
            {
                if (!TimerFinished())
                {
                    return;
                }
                else
                {
                    timerStarted = false;
                    victoryTimer = 0f;

                    MichaelPlayerController winner = DetermineWinner();

                    winner.anim.SetBool("victory", false);
                    winner.enemyScript.anim.SetBool("dead", false);
                    ActivatePlayers();

                    if (winner == null)
                    {
                        ResetRound();
                        return;
                    }

                    if (RewardPlayer(winner))
                    {
                        ResetGame();
                        return;
                    }
                    else
                    {
                        ResetRound();
                        return;
                    }
                }
            }
            else
            {
                timerStarted = true;
                MichaelPlayerController winner = DetermineWinner();

                winner.anim.SetBool("victory", true);
                winner.PlaySound("Victory1");

                winner.enemyScript.anim.SetTrigger("death");
                winner.enemyScript.anim.SetBool("dead", true);

                DisablePlayers();
            }
        }



        /*if (CheckTimer())
        {
            return;
        }
        if(RoundIsFinished() && hasNotDisabled)
        {
            timerStarted = true;
            hasNotDisabled = false;
            player1.anim.SetBool("victory", true);
            player2.anim.SetBool("dead", true);
        }
        else if(RoundIsFinished() && !hasNotDisabled)
        {

        }
        if(RoundIsFinished())
        {
            Debug.Log("Round finished");
            DisablePlayers();

            MichaelPlayerController winner = DetermineWinner();

            if(winner == null)
            {
                DisablePlayers();
                if (CheckTimer())
                {

                    ResetRound();
                    ActivatePlayers();
                }
                return;
            }

            if (RewardPlayer(winner))
            {
                ResetGame();
            }
            else
            {
                DisablePlayers();

                if (CheckTimer())
                {
                    ResetRound();
                    ActivatePlayers();

                }
                return;
            }
        }*/
    }

    void FixedUpdate()
    {
        RunTimer();
    }

    bool RoundIsFinished()
    {
        if (player1.CurrentHealth <= 0 || player2.CurrentHealth <= 0 || gameTimer.TimerFinished())
        {
            return true;
        }
        return false;
    }

    bool RewardPlayer(MichaelPlayerController player)
    {
        if(player == player1)
        {
            if (!player1RoundPoint1.Active && !player1RoundPoint2.Active)
            {
                player1RoundPoint1.Active = true;
                VictoryMark2.enabled = true;
            }
            else if (player1RoundPoint1.Active && !player1RoundPoint2.Active)
            {
                player1RoundPoint2.Active = true;
                VictoryMark1.enabled = true;
            }
            else if (player1RoundPoint1.Active && player1RoundPoint2.Active)
            {
                player1VictoryPoints.victoryPoints++;
                ResetMarks();
                return true;
            }
        }
        else if(player == player2)
        {
            if (!player2RoundPoint1.Active && !player2RoundPoint2.Active)
            {
                player2RoundPoint1.Active = true;
                VictoryMark4.enabled = true;
            }
            else if (player2RoundPoint1.Active && !player2RoundPoint2.Active)
            {
                player2RoundPoint2.Active = true;
                VictoryMark3.enabled = true;
            }
            else if (player2RoundPoint1.Active && player2RoundPoint2.Active)
            {
                player2VictoryPoints.victoryPoints++;
                ResetMarks();
                return true;
            }
        }
        return false;
    }

    MichaelPlayerController DetermineWinner()
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
        player1.transform.position = new Vector3(-5f, 2f);
        player2.transform.position = new Vector3(5f, 2f);

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
