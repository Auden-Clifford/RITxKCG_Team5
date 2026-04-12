using UnityEngine;

namespace Barrel
{
    /// <summary>
    /// Standard barrel implementation.
    /// Moves to the left by default.
    /// </summary>
    public class NormalBarrel : BarrelBase
    {
        protected override void Start()
        {
            base.Start();

            // Set initial movement values
            MoveDir = Vector3.left;

            if (_barrelData != null)
            {
                BarrelSpeed = _barrelData.moveSpeed;
                BarrelType = _barrelData.barrelType;
            }
        }

        /// <summary>
        /// Overrides Move to add any normal barrel specific behavior if needed.
        /// Currently uses the base linear movement.
        /// </summary>
        protected override void Move()
        {
            base.Move();
        }
    }
}