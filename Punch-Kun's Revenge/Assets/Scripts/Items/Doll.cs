using UnityEngine;

public class Doll : Item
{
    protected override void OnPickup(GameObject player)
    {
        // ** Win condition **
        if (GameManager.Instance != null)
            GameManager.Instance.LevelComplete();
    }
}