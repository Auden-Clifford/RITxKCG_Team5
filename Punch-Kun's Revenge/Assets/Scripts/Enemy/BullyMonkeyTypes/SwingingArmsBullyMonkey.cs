using UnityEngine;

/// <summary>
/// Bully Monkey that swings its arms at the player when in range.
/// </summary>
public class SwingingArmsBullyMonkey : BullyMonkey
{
    protected override void AttackPlayer()
    {
        // TODO: play swinging arms attack animation

        // cooldown at the end
        base.AttackPlayer();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerHealth health))
        {
            health.TakeDamage(_playerDamage);
        }
    }
}