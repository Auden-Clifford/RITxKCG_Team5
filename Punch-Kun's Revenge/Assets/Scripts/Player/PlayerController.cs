using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

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
    [SerializeField] private LayerMask _groundLayer;

    [Space(10)]
    [Header("Stun Settings")]
    [SerializeField] private float _stunDuration = 2.0f;
    [SerializeField] private float _stunMoveSpeedMultiplier = 0.5f;

    [Space(10)]
    [Header("Attack Settings")]
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private int _damage = 3;
    [SerializeField] private LayerMask _damagableLayers;

    private Vector3 _attackDirection;
    private Vector3 _mouseScreenPosition;
    private bool _useGamepad;
    private Camera _mainCam;
    private Rigidbody _rb;
    private Animator _animator;
    private float _moveX;
    private bool _isStunned;

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

        if (!_useGamepad)
        {
            Vector3 mouseWorldPosition = _mainCam.ScreenToWorldPoint(_mouseScreenPosition); // world position
            Vector3 toMouse = mouseWorldPosition - transform.position; // vector from player to mouse
            _attackDirection = Vector3.Normalize(new Vector3(toMouse.x, 0, 0));
        }

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
        Debug.LogWarning("************ ATTACKING ***************");

        // ! TEMP
        //RaycastHit[] hits = Physics.SphereCastAll(transform.position, _attackRange, Vector3.right, _attackRange, _damagableLayers);
        //foreach (RaycastHit hit in hits)
        //    Destroy(hit.collider.gameObject);

        _animator.SetTrigger(_isAttackingHash);

        Vector3 attackPosition = transform.position + _attackDirection;
        List<Collider> HitObjects = new(Physics.OverlapBox(attackPosition, new Vector3(1, 0.75f, 1), Quaternion.identity, _damagableLayers));
        foreach (Collider collider in HitObjects)
        {
            Debug.Log("hit");
            if (collider.gameObject.TryGetComponent(out Health health)) health.TakeDamage(_damage);
        }
    }

    /// <summary>
    /// Rotates the attack hitbox towards the mouse
    /// </summary>
    /// <param name="val"></param>
    private void OnLookMouse(InputValue val)
    {
        _useGamepad = false;
        _mouseScreenPosition = val.Get<Vector2>(); // screen position
    }

    private void OnLookGamepad(InputValue val)
    {
        _useGamepad = true;
        Vector2 stickInput = val.Get<Vector2>();
        _attackDirection = Vector3.Normalize(new Vector3(stickInput.x, 0, 0));
    }

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
    }

    private bool IsGrounded() => Physics.Raycast(transform.position, Vector3.down, _groundCheckDistance, _groundLayer);

    private void GetStunned()
    {
        // apply stun for a short duration, then reset
        _isStunned = true;
        StartCoroutine(Timer.WaitFor(_stunDuration, () => _isStunned = false));

        // got hit animation
        _animator.SetTrigger(_isHitHash);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(transform.position, 2f);
        Vector3 attackPosition = transform.position + _attackDirection;
        Gizmos.DrawWireCube(attackPosition, new Vector3(2, 1.5f, 1));
    }
}