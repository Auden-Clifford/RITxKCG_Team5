using UnityEngine;

namespace Barrel
{
    /// <summary>
    /// すべての樽（バレル）の基底クラス。
    /// 移動、環境との衝突判定（衝突時に破壊）、および破壊処理を管理します。
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BarrelBase : MonoBehaviour, IDamageable
    {
        [Header("Barrel Settings")]
        [Tooltip("衝突と破壊を判定するための地面や壁のレイヤーマスク")]
        [SerializeField] protected LayerMask _groundLayer;

        [Tooltip("この樽のステータス情報を持つ ScriptableObject")]
        [SerializeField] protected BarrelData _barrelData;

        /// <summary>
        /// 樽の移動速度を保持するプロパティ
        /// </summary>
        public float BarrelSpeed { get; set; }

        /// <summary>
        /// 樽が飛んでいく方向をベクトルで保持するプロパティ
        /// </summary>
        public Vector3 MoveDir { get; set; }

        /// <summary>
        /// 樽の種類を識別するためのプロパティ
        /// </summary>
        public BarrelType BarrelType { get; set; }

        protected Rigidbody _rb;

        protected virtual void Start()
        {
            // 必要に応じた初期化処理
            _rb = GetComponent<Rigidbody>();
        }

        protected virtual void FixedUpdate()
        {
            Move();
        }

        /// <summary>
        /// BarrelSpeedとMoveDirを使用して、樽の移動ロジックを処理します。
        /// </summary>
        protected virtual void Move()
        {
            // 方向と速度に基づいて移動する
            Vector3 movement = MoveDir * BarrelSpeed;
            _rb.AddForce(movement, ForceMode.Force);

            // 速度の制限（クランプ）
            _rb.linearVelocity = Vector3.ClampMagnitude(_rb.linearVelocity, BarrelSpeed);
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            // 地面またはプレイヤーに衝突したか確認する
            if (((1 << collision.gameObject.layer) & _groundLayer.value) != 0 ||
                collision.gameObject.CompareTag("Player"))
            {
                Break();
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            // プレイヤーがトリガーを使用している場合の念のための処理
            if (other.CompareTag("Player"))
            {
                Break();
            }
        }

        /// <summary>
        /// IDamageableの実装。プレイヤーからの攻撃で樽を破壊できるようにします。
        /// </summary>
        public void TakeDamage()
        {
            Break();
        }

        /// <summary>
        /// 樽の破壊処理を行います。
        /// </summary>
        protected virtual void Break()
        {
            // 破壊エフェクトが設定されていれば生成する
            if (_barrelData != null && _barrelData.breakEffectPrefab != null)
            {
                GameObject effect = Instantiate(_barrelData.breakEffectPrefab, transform.position, Quaternion.identity);
                if (effect.TryGetComponent(out Effects.SampleBreakEffect sbe))
                {
                    sbe.Play(gameObject);
                }
            }

            // 樽オブジェクトを破棄する
            Destroy(gameObject);
        }
    }
}