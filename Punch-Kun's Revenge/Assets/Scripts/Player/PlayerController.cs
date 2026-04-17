using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput), typeof(Animator))]
public class PlayerController : Singleton<PlayerController>
{
    [Header("Movement Settings")]
    [SerializeField] private float _speed = 10.0f;
    [SerializeField] private float _jumpForce = 7.0f;

    [Space(10)]
    [Header("Gravity Settings")]
    [SerializeField] private float _gravityInAirGravity = 5.0f;

    [Space(10)]
    [Header("Jump Settings")]
    [SerializeField] private float _groundCheckDistance = 1.3f;
    [SerializeField] private float _groundCheckOffset = 0.1f;   // need to lift off the ground a bit
    [SerializeField] private LayerMask _groundLayer;

    [Space(10)]
    [Header("Stun Settings")]
    [SerializeField] private float _stunDuration = 2.0f;
    [SerializeField] private float _stunMoveSpeedMultiplier = 0.5f;

    [Space(10)]
    [Header("Attack Settings")]
    [SerializeField] private int _damageToEnemy = 3;

    private Vector3 _attackDirection;
    private Camera _mainCam;
    private Rigidbody _rb;
    private Animator _animator;
    private float _moveX;
    private bool _isStunned;
    private bool _isAttacking;

    // animator hashes
    private int _isRunningHash;
    private int _isJumpingHash;
    private int _isAttackingHash;
    private int _isHitHash;

    // animator params
    private const string RUN = "Speed";
    private const string ATTACK = "Attack";
    private const string JUMP = "Jump";
    private const string Hit = "Got_Hit";

    public bool IsAttacking => _isAttacking;
    public int DamageOnAttack => _damageToEnemy;

    private void OnEnable()
    {
        PlayerHealth.OnPlayerTakeDamage += GetStunned;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerTakeDamage -= GetStunned;
    }

    protected override void Awake()
    {
        base.Awake();

        _isStunned = false;
        _isAttacking = false;
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _mainCam = Camera.main;

        // setting animator hashes
        _isRunningHash = Animator.StringToHash(RUN);
        _isJumpingHash = Animator.StringToHash(JUMP);
        _isAttackingHash = Animator.StringToHash(ATTACK);
        _isHitHash = Animator.StringToHash(Hit);
    }

    private void Update()
    {
        _animator.SetFloat(_isRunningHash, Mathf.Abs(_rb.linearVelocity.x));
    }

    private void FixedUpdate()
    {
        HandleMove();

        // apply extra gravity when in the air
        if (!IsGrounded()) _rb.AddForce(Vector3.down * _gravityInAirGravity, ForceMode.Acceleration);
    }

    private void LateUpdate()
    {
        // keep player inside the camera bounds
        Vector3 camPos = _mainCam.transform.position;
        Vector3 playerPos = transform.position;
        playerPos.x = Mathf.Clamp(playerPos.x, camPos.x - _mainCam.orthographicSize * _mainCam.aspect, camPos.x + _mainCam.orthographicSize * _mainCam.aspect);
        // playerPos.y = Mathf.Clamp(playerPos.y, camPos.y - _mainCam.orthographicSize, camPos.y + _mainCam.orthographicSize);
        transform.position = playerPos;
    }

    private void OnMove(InputValue val)
    {
        _moveX = val.Get<Vector2>().x;
    }

    private void OnJump(InputValue val) => HandleJump();

    private void OnAttack(InputValue val)
    {
        _animator.SetTrigger(_isAttackingHash);

        AudioManager.Instance.PlaySFX("PUNCH");

        // shake camera when attacking
        StartCoroutine(Timer.WaitFor(
            _animator.GetCurrentAnimatorStateInfo(0).length * 0.3f,
            () => CameraController.Instance.Shake(.1f)
        ));
    }

    // animation events
    public void StartAttacking() => _isAttacking = true;

    public void StopAttacking() => _isAttacking = false;

    private void HandleMove()
    {
        float currentSpeed = _isStunned ? _speed * _stunMoveSpeedMultiplier : _speed;
        Vector3 targetVelocity = new(_moveX * currentSpeed, _rb.linearVelocity.y, 0);
        Vector3 velocityChange = targetVelocity - _rb.linearVelocity;
        _rb.AddForce(velocityChange, ForceMode.VelocityChange);

        // rotate player in the direction of movement
        transform.rotation = Quaternion.Euler(0, _moveX < 0 ? 270 : 90, 0);
    }

    private void HandleJump()
    {
        // prevent jumping if not grounded
        if (!IsGrounded()) return;

        _animator.SetTrigger(_isJumpingHash);

        _rb.AddForce(_jumpForce * (Vector3.up + new Vector3(_moveX, 0, 0)), ForceMode.Impulse);

        AudioManager.Instance.PlaySFX("JUMP");
    }

    private bool IsGrounded() => Physics.Raycast(transform.position + Vector3.up * _groundCheckOffset, Vector3.down, _groundCheckDistance, _groundLayer);

    private void GetStunned()
    {
        // apply stun for a short duration, then reset
        _isStunned = true;
        StartCoroutine(Timer.WaitFor(_stunDuration, () => _isStunned = false));

        // got hit animation
        _animator.SetTrigger(_isHitHash);
    }
}