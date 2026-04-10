using UnityEngine;

public enum GameState
{
    MainMenu,
    Gameplay,
    GameOver,
    Paused,
    LevelComplete
}

public class GameManager : MonoBehaviour
{
    private float score;
    private float scrollSpeed;
    private GameState gameState;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject levelCompleteUI;
    [SerializeField] private GameObject pausedUI;
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject barrelPrefab;
    [SerializeField] private GameObject potatoPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
        scrollSpeed = 0;
        gameState = GameState.MainMenu;

        // set the initial UI state
        mainMenuUI.SetActive(true);
        gameOverUI.SetActive(false);
        levelCompleteUI.SetActive(false);
        pausedUI.SetActive(false);
        gameplayUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.MainMenu: break;
            case GameState.Gameplay: break;
            case GameState.GameOver: break;
            case GameState.LevelComplete: break;
            case GameState.Paused: break;

        }
    }

    /// <summary>
    /// Restarts the game from its initial state
    /// </summary>
    public void StartGame()
    {
        score = 0;
        scrollSpeed = 0;
        gameState = GameState.Gameplay;
        mainMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
        levelCompleteUI.SetActive(false);
        pausedUI.SetActive(false);
        gameplayUI.SetActive(true);

    }

    /// <summary>
    /// Pauses Gameplay
    /// </summary>
    public void PauseGame()
    {
        gameState = GameState.Paused;
        mainMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
        levelCompleteUI.SetActive(false);
        pausedUI.SetActive(true);
        gameplayUI.SetActive(false);
    }

    /// <summary>
    /// Resumes paused gameplay
    /// </summary>
    public void ResumeGame()
    {
        gameState = GameState.Gameplay;
        mainMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
        levelCompleteUI.SetActive(false);
        pausedUI.SetActive(false);
        gameplayUI.SetActive(true);
    }

    /// <summary>
    /// Sets the game to the Game Over screen
    /// </summary>
    public void GameOver()
    {
        gameState = GameState.GameOver;
        mainMenuUI.SetActive(false);
        gameOverUI.SetActive(true);
        levelCompleteUI.SetActive(false);
        pausedUI.SetActive(false);
        gameplayUI.SetActive(false);
    }

    /// <summary>
    /// Sets the game to the Win screen
    /// </summary>
    public void LevelComplete()
    {
        gameState = GameState.LevelComplete;
        mainMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
        levelCompleteUI.SetActive(true);
        pausedUI.SetActive(false);
        gameplayUI.SetActive(false);
    }

    /// <summary>
    /// Quits back to the title screen
    /// </summary>
    public void QuitToMain()
    {
        gameState = GameState.MainMenu;
        mainMenuUI.SetActive(true);
        gameOverUI.SetActive(false);
        levelCompleteUI.SetActive(false);
        pausedUI.SetActive(false);
        gameplayUI.SetActive(false);
    }
}
