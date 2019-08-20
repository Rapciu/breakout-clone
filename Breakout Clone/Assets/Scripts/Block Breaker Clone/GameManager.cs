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
    bool updateTimer = true;
    bool showGUI = true;

    void ConfigureSingleton()
    {
        int gameMainObjCount = FindObjectsOfType<GameManager>().Length;

        if (gameMainObjCount > 1)
        {
            // Deactivate the object so the scene doesn't have 2 game managers for a moment
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

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
        scoreTextComp.text = Mathf.Round(CalculateScore()).ToString();
    }

    private void DisplayTimer()
    {
        int minutes = (int)timer / 60;
        int seconds = (int)Mathf.Round(timer % 60);
        //float miliseconds = (timer - (minutes / 60 + (int)seconds)) * 1000; breaks after 1 minute, fix later

        //int secondsRounded = (int)Mathf.Round(seconds);
        //int milisecondsRounded = (int)Mathf.Round(miliseconds);

        int secondsDigitNum = seconds.ToString().Length;
        //int milisecondsDigitNum = milisecondsRounded.ToString().Length;

        //string gap1 = new string('0', 4 - secondsDigitNum);
        //string gap2 = new string('0', 5 - milisecondsDigitNum);

        //timerTextComp.text = $"{minutes}m{secondsRounded.ToString().PadLeft(5-secondsDigitNum)}s{milisecondsRounded.ToString().PadLeft(8-milisecondsDigitNum)}ms";
        timerTextComp.text = $"{minutes}m{seconds.ToString().PadLeft(5 - secondsDigitNum)}s";
    }

    private void Awake()
    {
        ConfigureSingleton();
    }

    void Start()
    {
        sceneLoaderComp = sceneLoader.GetComponent<SceneLoader>();
        ballComp = ball.GetComponent<Ball>();

        //scoreTextComp = gameCanvas.GetComponentInChildren<TextMeshProUGUI>();
        //DisplayScore();
    }

    void Update()
    {
        PauseGame();
        ResetGame();
        ChangeSpeed();

        if (updateTimer)
        {
            UpdateTimer();
        }

        if (showGUI)
        {
            DisplayScore();
            DisplayTimer();
        }

        ballComp.SpawnBall();
    }
}