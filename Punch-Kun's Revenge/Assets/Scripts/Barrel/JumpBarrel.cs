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

            // 水平移動の値を設定する（通常の樽の動作に合わせる）
            MoveDir = Vector3.left;
            _timer = 0f;

            if (_barrelData != null)
            {
                BarrelSpeed = _barrelData.moveSpeed;
                BarrelType = _barrelData.barrelType;
            }
        }

        /// <summary>
        /// Moveメソッドをオーバーライドし、基本的な水平移動を維持しつつバウンド（ジャンプ）効果を追加します。
        /// </summary>
        protected override void Move()
        {
            // 1. 基本的な水平移動を実行する（MoveDir * BarrelSpeed による移動）
            base.Move();
        }

        protected override void OnCollisionEnter(Collision collision)
        {
            base.OnCollisionEnter(collision);

            // 少し上方向の力を加える
            _rb.AddForce((Vector3.left + Vector3.up) * _barrelData.verticleJumpForce, ForceMode.Impulse);
            // 少し回転力（トルク）を加える
            _rb.AddTorque((Vector3.left + Vector3.up) * _barrelData.torqueAmount, ForceMode.Impulse);
        }
    }
}