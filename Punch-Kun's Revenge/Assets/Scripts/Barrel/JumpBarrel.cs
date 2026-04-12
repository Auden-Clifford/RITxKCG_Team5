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

            // 2. Add vertical bounce offset
            // if (_barrelData != null && _barrelData.jumpAmplitude > 0)
            // {
            //     float prevTimer = _timer;
            //     _timer += Time.deltaTime;

            //     // Calculate vertical delta based on absolute sine wave to create a "bouncing" effect
            //     float currentY = Mathf.Abs(Mathf.Sin(_timer * _barrelData.jumpFrequency)) * _barrelData.jumpAmplitude;
            //     float prevY = Mathf.Abs(Mathf.Sin(prevTimer * _barrelData.jumpFrequency)) * _barrelData.jumpAmplitude;
            //     float deltaY = currentY - prevY;

            //     transform.Translate(new Vector3(0, deltaY, 0), Space.World);
            // }
        }
    }
}