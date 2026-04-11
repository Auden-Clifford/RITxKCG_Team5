using UnityEngine;

public class HealingItem : Item
{
    [SerializeField] private int _healAmount = 1;

    protected override void OnPickup(GameObject player)
    {
        if (player.TryGetComponent(out PlayerHealth health))
            health.Heal(_healAmount);
    }
}