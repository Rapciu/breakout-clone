using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Objects and Components the Game Manager manages
    [SerializeField] GameObject sceneLoader;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] GameObject ball;

    //[SerializeField] GameObject gameCanvas;
    [SerializeField] TextMeshProUGUI scoreTextComp;
    [SerializeField] TextMeshProUGUI timerTextComp;
    [SerializeField] TextMeshProUGUI levelTextComp;

    // Config Parameters
    [Range(0.1f, 10f)] [SerializeField] float gameSpeed = 1f;
    [SerializeField] int pointsPerBlock = 10;

    [SerializeField] int[] noGameSceneIndexes;

    //Game State Variables
    [SerializeField] int currentPoints = 0;
    [SerializeField] float timer = 0f;

    SceneLoader sceneLoaderComp;
    Ball ballComp;

    bool paused = false;
    bool updateTimer = true;
    public bool showGUI = true;

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

    void ManageCanvas(Scene scene)
    {
        bool isNoGUIScene = System.Array.Exists(noGameSceneIndexes, index => index == scene.buildIndex);

        if (isNoGUIScene)
        {
            gameCanvas.SetActive(false);
            showGUI = false;
            updateTimer = false;
        }
        else
        {
            gameCanvas.SetActive(true);
            showGUI = true;
            updateTimer = true;

            UpdateAndDisplayLvlNum(scene);
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

    private void UpdateAndDisplayLvlNum(Scene scene)
    {
        levelTextComp.text = $"Level {scene.buildIndex}";
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

    public void DisplayScore()
    {
        //scoreTextComp.text = Mathf.Round(CalculateScore()).ToString();
        scoreTextComp.text = currentPoints.ToString();
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

    //deprecated
    //private void OnLevelWasLoaded(int level)
    //{
    //    ManageCanvas(level);
    //}

    private void Awake()
    {
        ConfigureSingleton();
    }

    private void OnEnable()
    {
        // Delegates the OnSceneLoaded as a event listener for when the scenes change
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //Event listener for scene load
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log($"Loaded scene: {scene.name} {scene.buildIndex}, mode: {mode}");
        ManageCanvas(scene);
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

        if (updateTimer)
        {
            UpdateTimer();
        }

        if (showGUI)
        {
            //DisplayScore();
            DisplayTimer();
        }

        ballComp.SpawnBall();
    }

    private void OnDisable()
    {
        //Undelegates the OnSceneLoaded when the game exists
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
