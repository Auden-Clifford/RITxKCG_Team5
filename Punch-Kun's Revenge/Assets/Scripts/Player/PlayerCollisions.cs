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
        // TODO: need to add a check whether player is attacking or not, then only destroy other things
        // give damage to any damageable object player collide with
        if (collision.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage();

            // also giving damage to player
            if (TryGetComponent(out PlayerHealth health)) health.TakeDamage(1);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_player.IsAttacking) return;

        if (other.gameObject.TryGetComponent(out Health health))
        {
            health.TakeDamage(_player.DamageOnAttack);
            CameraController.Instance.Shake(1f);
        }
    }
}