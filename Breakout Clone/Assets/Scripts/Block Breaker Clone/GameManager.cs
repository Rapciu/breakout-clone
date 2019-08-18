using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject sceneLoader;
    [SerializeField] GameObject ball;

    //[SerializeField] GameObject gameCanvas;
    [SerializeField] TextMeshProUGUI scoreTextComp;
    [SerializeField] TextMeshProUGUI timerTextComp;

    // Config Parameters
    [Range(0.1f, 10f)] [SerializeField] float gameSpeed = 1f;
    [SerializeField] int pointsPerBlock = 100;

    //Game State Variables
    [SerializeField] int currentPoints = 0;
    [SerializeField] float timer = 0f;

    SceneLoader sceneLoaderComp;
    Ball ballComp;

    bool paused = false;

    private void PauseGame()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (Time.timeScale > 0)
            {
                Time.timeScale = 0;
                paused = true;
            }
            else
            {
                Time.timeScale = gameSpeed;
                paused = false;
            }
        }
    }

    private void ResetGame()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            sceneLoaderComp.LoadScene(0);
        }
    }

    private void ChangeSpeed()
    {
        if (!paused)
        {
            Time.timeScale = gameSpeed;
        }
    }

    public void AddPoints()
    {
        currentPoints += pointsPerBlock;
    }

    private void UpdateTimer()
    {
        timer += Time.deltaTime;
    }

    private float CalculateScore()
    {
        //return currentScore / (int)Mathf.Round(timer);

        return currentPoints / timer;
    }

    private void DisplayScore()
    {
        scoreTextComp.text = CalculateScore().ToString();
    }

    private void DisplayTimer()
    {
        float seconds = timer % 60;
        int minutes = (int)timer / 60;
        float miliseconds = (timer - (minutes / 60 + (int)seconds)) * 1000;

        timerTextComp.text = $"{minutes}m {(int)Mathf.Round(seconds)}s {(int)Mathf.Round(miliseconds)}ms";
    }

    void Start()
    {
        sceneLoaderComp = sceneLoader.GetComponent<SceneLoader>();
        ballComp = ball.GetComponent<Ball>();

        //scoreTextComp = gameCanvas.GetComponentInChildren<TextMeshProUGUI>();
        DisplayScore();
    }

    void Update()
    {
        PauseGame();
        ResetGame();
        ChangeSpeed();

        UpdateTimer();

        DisplayScore();
        DisplayTimer();

        ballComp.SpawnBall();
    }
}