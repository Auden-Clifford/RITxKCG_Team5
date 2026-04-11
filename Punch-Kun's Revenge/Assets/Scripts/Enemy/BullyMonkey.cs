using UnityEngine;

public enum BullyMonkeyState
{
    Idle,                   // not doing anything, just standing there unless player is in range
    MovingTowardsPlayer,    // moving towards player but not in range yet
    AttackingPlayer         // in range and attacking player
}

/// <summary>
/// Bully Monkey
/// Responsibilities:
/// - Move towards player
/// - Attack player when in range
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public abstract class BullyMonkey : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _forceMultiplier = 2f;

    [Space(10)]
    [Header("Chase Settings")]
    [SerializeField] private float _startChaseRange = 15f;

    [Space(10)]
    [Header("Attack Settings")]
    [SerializeField] private float _attackRange = 10f;
    [SerializeField] protected int _playerDamage = 1;
    [SerializeField] protected float _attackCooldown = 2f;

    protected Rigidbody _rb;
    protected Transform _player;
    protected BullyMonkeyState _state = BullyMonkeyState.Idle;
    protected bool _canMoveTowardsPlayer = false;
    protected bool _isCoolingDown = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _player = PlayerController.Instance.transform;

        if (!_player)
        {
            Debug.LogError("Player not found in this scene!");
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        // state machine
        switch (_state)
        {
            case BullyMonkeyState.Idle:
                if (IsPlayerInRange(_startChaseRange))
                    _state = BullyMonkeyState.MovingTowardsPlayer;
                break;

            case BullyMonkeyState.MovingTowardsPlayer:
                _canMoveTowardsPlayer = true;

                if (IsPlayerInRange(_attackRange))
                    _state = BullyMonkeyState.AttackingPlayer;
                break;

            case BullyMonkeyState.AttackingPlayer:
                _canMoveTowardsPlayer = false;

                if (!_isCoolingDown)
                    AttackPlayer();
                break;
        }
    }

    void FixedUpdate()
    {
        if (_canMoveTowardsPlayer) MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (_player.position - transform.position).normalized;

        // arcade like movement but needs to make the rigidbody kinematic
        // _rb.MovePosition(transform.position + _speed * Time.deltaTime * direction);

        // kind of natural movement but need to limit the max speed
        _rb.AddForce(_speed * _forceMultiplier * direction, ForceMode.Acceleration);
        _rb.linearVelocity = Vector3.ClampMagnitude(_rb.linearVelocity, _speed);

        // TODO: rotate bully monkey in the direction of movement
    }

    private bool IsPlayerInRange(float range)
    {
        float distance = Vector3.Distance(transform.position, _player.position);
        return distance <= range;
    }

    protected virtual void AttackPlayer()
    {
        // TODO: Implementation for attacking the player
        Debug.LogError("Attacking player!");

        // ! needed to add a cooldown for attacking the player, otherwise it will just keep damaging the player every frame when in range
        // TODO: maybe add an attack animation and only apply DAMAGE to player at a certain frame of the animation, then start cooldown after the animation is done
        _isCoolingDown = true;
        // _rb.constraints = RigidbodyConstraints.FreezeAll;   // freeze the bully monkey in place after attacking for cooldown
        StartCoroutine(Timer.WaitFor(_attackCooldown, () =>
        {
            _isCoolingDown = false;
            _state = BullyMonkeyState.Idle;
            // _rb.constraints = RigidbodyConstraints.FreezeRotation;   // unfreeze the position of bully monkey after cooldown
        }));
    }

    // ! Just for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _startChaseRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}