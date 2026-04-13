using Unity.VisualScripting;
using UnityEngine;

namespace Barrel
{
    public class JumpBarrel : BarrelBase
    {
        private float _timer;

        protected override void Start()
        {
            base.Start();

            // Set horizontal movement values (match normal barrel behavior)
            MoveDir = Vector3.left;
            _timer = 0f;

            if (_barrelData != null)
            {
                BarrelSpeed = _barrelData.moveSpeed;
                BarrelType = _barrelData.barrelType;
            }
        }

        /// <summary>
        /// Overrides Move to add a bouncing (jumping) effect while maintaining base horizontal movement.
        /// </summary>
        protected override void Move()
        {
            // 1. Perform base horizontal movement (Translate by MoveDir * BarrelSpeed)
            base.Move();
        }

        protected override void OnCollisionEnter(Collision collision)
        {
            base.OnCollisionEnter(collision);

            // add a little upward force
            _rb.AddForce((Vector3.left + Vector3.up) * _barrelData.verticleJumpForce, ForceMode.Impulse);
            // add a little torque
            _rb.AddTorque((Vector3.left + Vector3.up) * _barrelData.torqueAmount, ForceMode.Impulse);
        }
    }
}