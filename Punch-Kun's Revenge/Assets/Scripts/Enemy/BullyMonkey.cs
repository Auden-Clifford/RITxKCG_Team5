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
[RequireComponent(typeof(Rigidbody), typeof(Animator))]
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
    protected Animator _animator;
    protected BullyMonkeyState _state = BullyMonkeyState.Idle;
    protected bool _canMoveTowardsPlayer = false;
    protected float _cooldownTimer;

    // animator hashes
    protected int _isRunningHash;
    protected int _isJumpingHash;
    protected int _isThrowingAttackHash;
    protected int _isSwingingArmsAttackHash;
    protected int _runningAttackSpeedMultiplierHash;
    protected int _isDeadHash;

    // animator params
    private const string RUN = "Speed";
    private const string RUN_ATTACK_SPEED_MULTIPLIER = "Run_Attack_Speed_Multiplier";
    private const string THROW_ATTACK = "Throw_Attack";
    private const string SWINGING_ARMS_ATTACK = "SwingArms";
    private const string JUMP = "Jump";
    private const string DEAD = "Dead";

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _player = PlayerController.Instance.transform;

        if (!_player)
        {
            Debug.LogError("Player not found in this scene!");
            enabled = false;
            return;
        }

        // setting animator hashes
        _isRunningHash = Animator.StringToHash(RUN);
        _isJumpingHash = Animator.StringToHash(JUMP);
        _isThrowingAttackHash = Animator.StringToHash(THROW_ATTACK);
        _runningAttackSpeedMultiplierHash = Animator.StringToHash(RUN_ATTACK_SPEED_MULTIPLIER);
        _isSwingingArmsAttackHash = Animator.StringToHash(SWINGING_ARMS_ATTACK);
        _isDeadHash = Animator.StringToHash(DEAD);
    }

    private void Update()
    {
        if (GameManager.Instance.GameState != GameState.Gameplay) return;

        // reduce cooldown
        if (_cooldownTimer >= 0)
            _cooldownTimer -= Time.deltaTime;

        // state machine
        switch (_state)
        {
            case BullyMonkeyState.Idle:
                if (IsPlayerInRange(_startChaseRange) && _cooldownTimer < 0)
                    _state = BullyMonkeyState.MovingTowardsPlayer;
                break;

            case BullyMonkeyState.MovingTowardsPlayer:
                _canMoveTowardsPlayer = true;

                if (IsPlayerInRange(_attackRange))
                    _state = BullyMonkeyState.AttackingPlayer;
                break;

            case BullyMonkeyState.AttackingPlayer:
                _canMoveTowardsPlayer = false;

                // if (_cooldownTimer < 0)
                AttackPlayer();
                break;
        }

        // run animation
        _animator.SetFloat(_isRunningHash, Mathf.Abs(_rb.linearVelocity.x));
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
        bool isMovingRight = Vector3.Dot(_rb.linearVelocity, Vector3.right) > 0f;
        transform.rotation = Quaternion.Euler(0, isMovingRight ? 90 : -90, 0);
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

        _cooldownTimer = _attackCooldown;
        _state = BullyMonkeyState.Idle;
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