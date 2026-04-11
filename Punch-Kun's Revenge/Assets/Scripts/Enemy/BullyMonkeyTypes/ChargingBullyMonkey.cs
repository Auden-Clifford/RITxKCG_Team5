using UnityEngine;

/// <summary>
/// Bully Monkey that charges at the player at high speed when in range.
/// </summary>
public class ChargingBullyMonkey : BullyMonkey
{
    [Header("Charge Settings")]
    [Tooltip("The force applied to the bully monkey when it charges at the player")]
    [SerializeField] private float _attackForce = 40f;

    protected override void AttackPlayer()
    {
        // apply a strong force towards the player to simulate a charge attack
        Vector3 directionToPlayer = (_player.position - transform.position).normalized;
        _rb.AddForce(_attackForce * directionToPlayer, ForceMode.Impulse);

        // TODO: play attack animation


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