using System;
using UnityEngine;
using UnityProgressBar;

public class PlayerHealth : Health
{
    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private ProgressBar healthBar;
    [SerializeField] private float invincibilityTime;

    private float invincibilityTimer;

    public static event Action OnPlayerTakeDamage = delegate { };
    public static event Action OnPlayerDeath = delegate { };

    void Update()
    {
        if (GameManager.Instance.GameState != GameState.Gameplay) return;

        if (invincibilityTimer >= 0)
            invincibilityTimer -= Time.deltaTime;

        // flicker the player model while invincible
        if (invincibilityTimer >= 0)
        {
            if ((int)(invincibilityTimer * 20) % 2 == 1)
                meshRenderer.enabled = false;
            else
                meshRenderer.enabled = true;
        }
    }

    public override void TakeDamage(int damage)
    {
        // only take damage if the i-frames are done
        if (invincibilityTimer >= 0) return;

        invincibilityTimer = invincibilityTime; // reset the timer
        meshRenderer.enabled = true;

        base.TakeDamage(damage);

        healthBar.Value = (float)_currentHealth / _maxHealth;

        OnPlayerTakeDamage?.Invoke();

        // ** lose condition **
        if (_currentHealth == 0)
        {
            OnPlayerDeath?.Invoke();
            if (GameManager.Instance != null)
                GameManager.Instance.GameOver();
        }
    }

    public void Heal(int healAmount = 1)
    {
        _currentHealth = Math.Min(_currentHealth + healAmount, _maxHealth);
    }
}