using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    float currentTime;

    public float startingTime;

    [SerializeField]
    Text countdownText;

    bool timerPaused;

    // Start is called before the first frame update
    void Start()
    {
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerPaused)
        {
            return;
        }

        currentTime -= Time.deltaTime;

        if(countdownText != null)
        {
            countdownText.text = currentTime.ToString("0");
        }

        if (currentTime <= 0)
        {
            currentTime = 0;
        }
    }

    public bool TimerFinished()
    {
        if(currentTime <= 0)
        {
            return true;
        }
        return false;
    }

    public void ResetTimer()
    {
        currentTime = startingTime;
    }

    public void PauseTimer()
    {
        timerPaused = true;
    }

    public void ResumeTimer()
    {
        timerPaused = false;
    }
}
