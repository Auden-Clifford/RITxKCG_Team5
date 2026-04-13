using UnityEngine;

namespace Barrel
{
    /// <summary>
    /// 標準的な樽の実装。
    /// デフォルトでは左方向に移動します。
    /// </summary>
    public class NormalBarrel : BarrelBase
    {
        protected override void Start()
        {
            base.Start();

            // 初期の移動値を設定する
            MoveDir = Vector3.left;

            if (_barrelData != null)
            {
                BarrelSpeed = _barrelData.moveSpeed;
                BarrelType = _barrelData.barrelType;
            }
        }

        /// <summary>
        /// 必要に応じて通常の樽特有の動作を追加するためにMoveメソッドをオーバーライドします。
        /// 現在は基底クラスの直線移動を使用しています。
        /// </summary>
        protected override void Move()
        {
            base.Move();
        }
    }
}