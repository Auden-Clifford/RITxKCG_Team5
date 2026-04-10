using UnityEngine;

public enum GameState
{
    MainMenu,
    Gameplay,
    GameOver,
    LevelComplete
}

public class GameManager : MonoBehaviour
{
    private float score;
    private float scrollSpeed;
    private GameState gameState;
    private GameObject enemyPrefab;
    private GameObject barrelPrefab;
    private GameObject potatoPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
        scrollSpeed = 0;
        gameState = GameState.MainMenu;
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.MainMenu: break;
            case GameState.Gameplay: break;
            case GameState.GameOver: break;

        }
    }
}
