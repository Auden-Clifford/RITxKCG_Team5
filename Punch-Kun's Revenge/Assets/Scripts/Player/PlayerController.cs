using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
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
    [Header("Misc Settings")]
    [SerializeField] private float _stunDuration = 2.0f;

    private Rigidbody _rb;
    private float _moveX;
    private bool _isStunned;

    private void Start()
    {
        _isStunned = false;
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // prevent player from moving or jumping if stunned
        if (_isStunned) GetStunned();

        HandleMove();

        // apply extra gravity when in the air
        if (!IsGrounded()) _rb.AddForce(Vector3.down * _gravityInAirGravity, ForceMode.Acceleration);
    }

    private void OnMove(InputValue val)
    {
        _moveX = val.Get<Vector2>().x;

        if (_moveX == 0) return;
        // FlipPlayerHorizontally(_moveX < 0);
    }

    private void OnJump(InputValue val) => HandleJump();

    private void HandleMove()
    {
        Vector3 targetVelocity = new(_moveX * _speed, _rb.linearVelocity.y, 0);
        Vector3 velocityChange = targetVelocity - _rb.linearVelocity;
        _rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    private void HandleJump()
    {
        // prevent jumping if not grounded or if stunned
        if (!IsGrounded() || _isStunned) return;

        _rb.AddForce(_jumpForce * (Vector3.up + new Vector3(_moveX, 0, 0)), ForceMode.Impulse);
    }

    private bool IsGrounded() => Physics.Raycast(transform.position, Vector3.down, _groundCheckDistance, _groundLayer);

    private void GetStunned()
    {
        // disable player input for a short duration, then reset
        _isStunned = true;
        StartCoroutine(Timer.WaitFor(_stunDuration, () => _isStunned = false));
    }
}