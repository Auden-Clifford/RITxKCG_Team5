using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private PlayerController _player;

    void Start()
    {
        _player = PlayerController.Instance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // give damage to any damageable object player collide with
        if (collision.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage();

            // also giving damage to player
            if (TryGetComponent(out PlayerHealth health)) health.TakeDamage(1);

            // playing audio
            if (TryGetComponent(out Barrel.BarrelBase barrel))
                AudioManager.Instance.PlaySFX("BARREL_DESTROYED");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_player.IsAttacking) return;

        if (other.gameObject.TryGetComponent(out Health health))
        {
            health.TakeDamage(_player.DamageOnAttack);
            CameraController.Instance.Shake(1f);

            // playing audio
            if (TryGetComponent(out Barrel.BarrelBase barrel)) AudioManager.Instance.PlaySFX("HIT_THE_BARREL");
            else if (TryGetComponent(out BullyMonkey bm)) AudioManager.Instance.PlaySFX("ATTACKED_THE_MONKEY");
        }
    }
}