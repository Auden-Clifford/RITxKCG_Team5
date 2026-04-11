using System;

public class PlayerHealth : Health
{
    public static event Action OnPlayerTakeDamage = delegate { };
    public static event Action OnPlayerDeath = delegate { };

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        OnPlayerTakeDamage?.Invoke();

        // ** lose condition **
        if (_currentHealth == 0) OnPlayerDeath?.Invoke();
    }
}