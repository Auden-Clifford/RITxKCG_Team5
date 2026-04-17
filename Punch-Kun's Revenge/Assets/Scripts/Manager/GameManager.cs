using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum GameState
{
    Gameplay,
    GameOver,
    Paused,
    LevelComplete
}

[RequireComponent(typeof(PlayerInput))]
public class GameManager : Singleton<GameManager>
{
    private float score;
    [SerializeField] private TextMeshProUGUI scoreLabel;

    [SerializeField] private Vector3 scrollSpeed;
    [SerializeField] private Vector3 scrollAcceleration;
    [SerializeField] private float maxScrollSpeed;

    public GameState GameState;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject levelCompleteUI;
    [SerializeField] private GameObject pausedUI;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private GameObject[] barrelPrefab;
    [SerializeField] private GameObject potatoPrefab;

    [SerializeField] private Transform itemSpawnPosition;

    [SerializeField] private float enemySpawnTimer;
    [SerializeField] private float enemySpawnDelay;

    [SerializeField] private float obstacleSpawnTimer;
    [SerializeField] private float obstacleSpawnDelay;

    [SerializeField] private float healthItemSpawnTimer;
    [SerializeField] private float healthItemSpawnDelay;

    [SerializeField] private GameObject pauseContinueButton;
    [SerializeField] private GameObject gameOverRestartButton;
    [SerializeField] private GameObject levelCompleteRestartButton;
    [SerializeField] private PlayerInput _playerInputSystem;

    private List<GameObject> enemies;
    private List<GameObject> obstacles;
    private List<GameObject> healingItems;
    private CameraController _cameraController;
    private PlayerInput _uiInputSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemies = new List<GameObject>();
        obstacles = new List<GameObject>();
        healingItems = new List<GameObject>();
        ResetScene();

        // set the initial UI state
        gameOverUI.SetActive(false);
        levelCompleteUI.SetActive(false);
        pausedUI.SetActive(false);
        gameplayUI.SetActive(false);

        // start the game
        Time.timeScale = 1;
        GameState = GameState.Gameplay;
        gameplayUI.SetActive(true);

        _cameraController = CameraController.Instance;
        _uiInputSystem = GetComponent<PlayerInput>();
        if (_cameraController == null)
        {
            Debug.LogError("CameraController not found!");
            enabled = false;
        }

        ShowCursor(false);
        _uiInputSystem.enabled = false;
        _playerInputSystem.enabled = true;

        _playerInputSystem.actions["ShowPauseMenu"].performed += ctx => PauseGame();
        _uiInputSystem.actions["HidePauseMenu"].performed += ctx => ResumeGame();
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameState)
        {
            case GameState.Gameplay:
                _cameraController.MainCineCam.transform.Translate(scrollSpeed * Time.deltaTime);    // move the camera

                float newScrollSpeed = Mathf.Clamp(scrollSpeed.x + scrollAcceleration.x * Time.deltaTime, 0, maxScrollSpeed); // accelerate
                scrollSpeed.x = newScrollSpeed;

                enemySpawnTimer -= Time.deltaTime;
                obstacleSpawnTimer -= Time.deltaTime;
                healthItemSpawnTimer -= Time.deltaTime;

                if (enemySpawnTimer < 0)
                {
                    Debug.Log("spawning enemy");
                    enemySpawnTimer = enemySpawnDelay;
                    //GameObject enemy = Instantiate(enemyPrefab, itemSpawnPosition);
                    enemies.Add(Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], itemSpawnPosition.transform.position, itemSpawnPosition.transform.rotation));
                }
                if (obstacleSpawnTimer < 0)
                {
                    Debug.Log("spawning obstacle");
                    obstacleSpawnTimer = obstacleSpawnDelay;
                    //GameObject obstacle = Instantiate(barrelPrefab, itemSpawnPosition);
                    enemies.Add(Instantiate(barrelPrefab[Random.Range(0, barrelPrefab.Length)], itemSpawnPosition.transform.position, itemSpawnPosition.transform.rotation));
                }
                if (healthItemSpawnTimer < 0)
                {
                    Debug.Log("spawning health item");
                    healthItemSpawnTimer = healthItemSpawnDelay;
                    //GameObject healingitem =  Instantiate(potatoPrefab, itemSpawnPosition);
                    healingItems.Add(Instantiate(potatoPrefab, itemSpawnPosition.transform.position, itemSpawnPosition.transform.rotation));
                }
                break;
            case GameState.GameOver: break;
            case GameState.LevelComplete: break;
            case GameState.Paused: break;
        }
    }

    private void ShowCursor(bool showCursor)
    {
        Cursor.visible = showCursor;
        Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void ResetScene()
    {
        score = 0;
        enemySpawnTimer = enemySpawnDelay;
        obstacleSpawnTimer = obstacleSpawnDelay;
        healthItemSpawnTimer = healthItemSpawnDelay;

        for (int i = 0; i < enemies.Count; i++)
        {
            Destroy(enemies[i]);
        }
        enemies.Clear();
        for (int i = 0; i < obstacles.Count; i++)
        {
            Destroy(obstacles[i]);
        }
        obstacles.Clear();
        for (int i = 0; i < healingItems.Count; i++)
        {
            Destroy(healingItems[i]);
        }
        healingItems.Clear();
    }

    /// <summary>
    /// Restarts the game from its initial state
    /// </summary>
    public void StartGame()
    {
        // just reload current scene
        SceneManager.LoadScene(Scenes.MAIN_LEVEL);
    }

    /// <summary>
    /// Pauses Gameplay
    /// </summary>
    public void PauseGame()
    {
        // ShowCursor(true);

        _playerInputSystem.enabled = false;
        _uiInputSystem.enabled = true;
        pauseContinueButton.GetComponent<Button>().Select();

        GameState = GameState.Paused;
        gameOverUI.SetActive(false);
        levelCompleteUI.SetActive(false);
        pausedUI.SetActive(true);
        gameplayUI.SetActive(false);

        // stop time
        Time.timeScale = 0;
    }

    /// <summary>
    /// Resumes paused gameplay
    /// </summary>
    public void ResumeGame()
    {
        ShowCursor(false);

        _uiInputSystem.enabled = false;
        _playerInputSystem.enabled = true;

        GameState = GameState.Gameplay;
        gameOverUI.SetActive(false);
        levelCompleteUI.SetActive(false);
        pausedUI.SetActive(false);
        gameplayUI.SetActive(true);

        // resume time
        Time.timeScale = 1;
    }

    /// <summary>
    /// Sets the game to the Game Over screen
    /// </summary>
    public void GameOver()
    {
        // ShowCursor(true);

        _playerInputSystem.enabled = false;
        _uiInputSystem.enabled = true;
        gameOverRestartButton.GetComponent<Button>().Select(); ;

        GameState = GameState.GameOver;
        gameOverUI.SetActive(true);
        levelCompleteUI.SetActive(false);
        pausedUI.SetActive(false);
        gameplayUI.SetActive(false);

        // stop time
        Time.timeScale = 0;
    }

    /// <summary>
    /// Sets the game to the Win screen
    /// </summary>
    public void LevelComplete()
    {
        // ShowCursor(true);

        _playerInputSystem.enabled = false;
        _uiInputSystem.enabled = true;
        levelCompleteRestartButton.GetComponent<Button>().Select(); ;

        GameState = GameState.LevelComplete;
        gameOverUI.SetActive(false);
        levelCompleteUI.SetActive(true);
        pausedUI.SetActive(false);
        gameplayUI.SetActive(false);

        // stop time
        Time.timeScale = 0;
    }

    /// <summary>
    /// Quits back to the title screen
    /// </summary>
    public void QuitToMain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(Scenes.TITLE);
    }

    /// <summary>
    /// Slows down the camera scrolling by a specified amount
    /// </summary>
    /// <param name="amount">Amount to slow down the camera</param>
    public void SlowCamera(float amount)
    {
        float newSpeed = Mathf.Clamp(scrollSpeed.x - amount, 0, maxScrollSpeed);
        scrollSpeed.x = newSpeed;
    }

    public void AddScore(float amount)
    {
        score += amount;
        scoreLabel.text = "Score: " + score;
    }

    // ** Input System Callbacks **
    // private void OnPauseMenu(InputValue val)
    // {
    //     if (GameState != GameState.Paused) PauseGame();
    //     else ResumeGame();
    // }
}