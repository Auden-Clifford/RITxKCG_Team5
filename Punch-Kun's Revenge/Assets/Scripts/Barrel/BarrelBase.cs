using UnityEngine;

namespace Barrel
{
    /// <summary>
    /// Base class for all barrel types.
    /// Handles movement, collision detection with environment (break on impact), and destruction.
    /// </summary>
    public abstract class BarrelBase : MonoBehaviour, IDamageable
    {
        [Header("Barrel Settings")]
        [Tooltip("Layer mask for the ground and walls to detect collision and break")]
        [SerializeField] protected LayerMask _groundLayer;

        [Tooltip("ScriptableObject containing stats for this barrel")]
        [SerializeField] protected Barrel.BarrelData _barrelData;

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

        protected virtual void Start()
        {
            // Base initialization if needed
        }

        protected virtual void Update()
        {
            Move();
        }

        /// <summary>
        /// Handles the movement logic of the barrel using BarrelSpeed and MoveDir.
        /// </summary>
        protected virtual void Move()
        {
            // Move based on direction and speed
            Vector3 movement = MoveDir * BarrelSpeed * Time.deltaTime;
            transform.Translate(movement, Space.World);
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            // Check if hit the ground or environment
            if (((1 << collision.gameObject.layer) & _groundLayer.value) != 0)
            {
                Break();
            }
        }

        /// <summary>
        /// Implementation of IDamageable. Allows the player to break the barrel.
        /// </summary>
        public virtual void TakeDamage()
        {
            Break();
        }

        /// <summary>
        /// Handles the destruction of the barrel.
        /// </summary>
        protected virtual void Break()
        {
            // Spawn break effect if it exists
            if (_barrelData != null && _barrelData.breakEffectPrefab != null)
            {
                Instantiate(_barrelData.breakEffectPrefab, transform.position, Quaternion.identity);
            }

            // Destroy the barrel object
            Destroy(gameObject);
        }
    }
}