using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum GameState
{
    Gameplay,
    GameOver,
    Paused,
    LevelComplete
}

public class GameManager : Singleton<GameManager>
{
    private float score;
    [SerializeField] private Vector3 scrollSpeed;
    [SerializeField] private Vector3 scrollAcceleration;
    private GameState gameState;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject levelCompleteUI;
    [SerializeField] private GameObject pausedUI;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private GameObject barrelPrefab;
    [SerializeField] private GameObject potatoPrefab;

    [SerializeField] private Transform itemSpawnPosition;

    [SerializeField] private float enemySpawnTimer;
    [SerializeField] private float enemySpawnDelay;

    [SerializeField] private float obstacleSpawnTimer;
    [SerializeField] private float obstacleSpawnDelay;

    [SerializeField] private float healthItemSpawnTimer;
    [SerializeField] private float healthItemSpawnDelay;

    private List<GameObject> enemies;
    private List<GameObject> obstacles;
    private List<GameObject> healingItems;

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
        gameState = GameState.Gameplay;
        gameplayUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.Gameplay:
                Camera.main.gameObject.transform.position += scrollSpeed * Time.deltaTime; // move the camera
                scrollSpeed += scrollAcceleration * Time.deltaTime; // accelerate

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
                    enemies.Add(Instantiate(barrelPrefab, itemSpawnPosition.transform.position, itemSpawnPosition.transform.rotation));
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

    private void ResetScene()
    {
        score = 0;
        Camera.main.gameObject.transform.position = new Vector3(0, 0, -10);
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
        gameState = GameState.Paused;
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
        gameState = GameState.Gameplay;
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
        gameState = GameState.GameOver;
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
        gameState = GameState.LevelComplete;
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
        SceneManager.LoadScene(Scenes.TITLE);
    }
}