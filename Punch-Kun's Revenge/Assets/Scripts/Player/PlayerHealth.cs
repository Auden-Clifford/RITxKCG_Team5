using System;
using UnityEngine;
using UnityProgressBar;

public class PlayerHealth : Health
{
    public static event Action OnPlayerTakeDamage = delegate { };
    public static event Action OnPlayerDeath = delegate { };

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private ProgressBar healthBar;

    private float invincibilityTimer;
    [SerializeField] private float invincibilityTime;

    void Update()
    {
        if(GameManager.Instance.GameState == GameState.Gameplay)
        {
            invincibilityTimer -= Time.deltaTime;

            // flicker the player model while invincible
            if (invincibilityTimer > 0)
            {
                if ((int)(invincibilityTimer * 20) % 2 == 1)
                {
                    meshRenderer.enabled = false;
                }
                else
                {
                    meshRenderer.enabled = true;
                }
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        // only take damage if the i-frames are done
        if(invincibilityTimer < 0)
        {
            invincibilityTimer = invincibilityTime; // reset the timer
            meshRenderer.enabled = true;

            base.TakeDamage(damage);

            healthBar.Value = (float)_currentHealth / (float)_maxHealth;

            OnPlayerTakeDamage?.Invoke();

            // ** lose condition **
            if (_currentHealth == 0)
            {
                OnPlayerDeath?.Invoke();
                if (GameManager.Instance != null)
                    GameManager.Instance.GameOver();
            }
        }
    }

    public void Heal(int healAmount = 1)
    {
        _currentHealth = Math.Min(_currentHealth + healAmount, _maxHealth);
    }
}