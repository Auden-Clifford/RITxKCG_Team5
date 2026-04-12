using UnityEngine;

public class Doll : Item
{
    [SerializeField] private float score;
    protected override void OnPickup(GameObject player)
    {
        // ** Win condition **
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(score);
            GameManager.Instance.LevelComplete();
        }
            
    }
}