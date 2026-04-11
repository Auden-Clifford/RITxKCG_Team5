using UnityEngine;

/// <summary>
/// Bully Monkey that throws barrels at the player when in range.
/// </summary>
public class BarrelThrowingBullyMonkey : BullyMonkey
{
    [Header("Barrel Settings")]
    [Tooltip("The barrel prefab that the bully monkey throws at the player")]
    [SerializeField] private GameObject _barrelPrefab;
    [Tooltip("The force applied to the barrel when thrown towards the player")]
    [SerializeField] private float _attackForce = 20f;

    protected override void AttackPlayer()
    {
        // TODO: play barrel throwing attack animation

        // throw barrel towards player
        GameObject barrel = Instantiate(_barrelPrefab);
        if (barrel != null && barrel.TryGetComponent(out Rigidbody barrelRb))
        {
            barrel.transform.position = transform.position + Vector3.up; // spawn barrel slightly above the bully monkey
            Vector3 directionToPlayer = (_player.position - transform.position).normalized;
            barrelRb.AddForce(directionToPlayer * _attackForce, ForceMode.Impulse);
        }

        // cooldown at the end
        base.AttackPlayer();
    }

    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.TryGetComponent(out PlayerHealth health))
    //     {
    //         health.TakeDamage(_playerDamage);
    //     }
    // }
}