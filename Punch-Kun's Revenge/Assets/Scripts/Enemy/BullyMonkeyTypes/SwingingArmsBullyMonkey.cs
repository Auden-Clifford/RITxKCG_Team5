using UnityEngine;

/// <summary>
/// Bully Monkey that swings its arms at the player when in range.
/// </summary>
public class SwingingArmsBullyMonkey : BullyMonkey
{
    [Space(10)]
    [Header("Swinging Arms Bully Monkey")]
    [SerializeField] private float _inRangeWalkSpeed = 5f;

    protected override void AttackPlayer()
    {
        // play swinging arms attack animation
        _animator.SetTrigger(_isSwingingArmsAttackHash);

        // apply a small force towards the player to just move
        Vector3 directionToPlayer = (_player.position - transform.position).normalized;
        _rb.AddForce(_inRangeWalkSpeed * directionToPlayer, ForceMode.Impulse);

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