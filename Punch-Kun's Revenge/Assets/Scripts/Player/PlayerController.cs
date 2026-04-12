using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
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
    [SerializeField] private LayerMask _damagableLayers;

    private Camera _mainCam;
    private Rigidbody _rb;
    private float _moveX;
    private bool _isStunned;

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
    }

    private void Start()
    {
        _mainCam = Camera.main;
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

        if (_moveX == 0) return;

        // TODO: rotate player in the direction of movement
        // FlipPlayerHorizontally(_moveX < 0);
    }

    private void OnJump(InputValue val) => HandleJump();

    private void OnAttack(InputValue val)
    {
        Debug.LogWarning("************ ATTACKING ***************");

        // ! TEMP
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, _attackRange, Vector3.right, _attackRange, _damagableLayers);
        foreach (RaycastHit hit in hits)
            Destroy(hit.collider.gameObject);
    }

    private void HandleMove()
    {
        float currentSpeed = _isStunned ? _speed * _stunMoveSpeedMultiplier : _speed;
        Vector3 targetVelocity = new(_moveX * currentSpeed, _rb.linearVelocity.y, 0);
        Vector3 velocityChange = targetVelocity - _rb.linearVelocity;
        _rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    private void HandleJump()
    {
        // prevent jumping if not grounded
        if (!IsGrounded()) return;

        _rb.AddForce(_jumpForce * (Vector3.up + new Vector3(_moveX, 0, 0)), ForceMode.Impulse);
    }

    private bool IsGrounded() => Physics.Raycast(transform.position, Vector3.down, _groundCheckDistance, _groundLayer);

    private void GetStunned()
    {
        // apply stun for a short duration, then reset
        _isStunned = true;
        StartCoroutine(Timer.WaitFor(_stunDuration, () => _isStunned = false));
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}