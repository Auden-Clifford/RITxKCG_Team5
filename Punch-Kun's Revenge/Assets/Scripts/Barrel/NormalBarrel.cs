using UnityEngine;

namespace Barrel
{
    /// <summary>
    /// Standard barrel implementation.
    /// Moves to the left by default.
    /// </summary>
    public class NormalBarrel : BarrelBase
    {
        [Header("Normal Barrel Settings")]
        [SerializeField] private float _defaultSpeed = 5f;

        protected override void Start()
        {
            base.Start();

            // Set initial movement values
            MoveDir = Vector3.left;
            BarrelSpeed = _defaultSpeed;
            BarrelType = BarrelType.Normal;
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