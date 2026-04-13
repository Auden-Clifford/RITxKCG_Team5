using UnityEngine;

namespace Barrel
{
    /// <summary>
    /// ScriptableObject that holds data for a specific barrel type.
    /// This allows for easy adjustment of barrel parameters without modifying prefabs directly.
    /// </summary>
    [CreateAssetMenu(fileName = "NewBarrelData", menuName = "Barrel/BarrelData")]
    public class BarrelData : ScriptableObject
    {
        [Header("General Settings")]
        [Tooltip("The type of this barrel")]
        public BarrelType barrelType;

        [Tooltip("Standard movement speed for this barrel")]
        public float moveSpeed = 5f;

        [Header("Jump Settings")]
        [Tooltip("How high the barrel jumps")]
        public float jumpAmplitude = 2f;

        [Tooltip("How fast the barrel jumps")]
        public float jumpFrequency = 3f;

        public float verticleJumpForce = 2f;

        [Header("Torque Settings")]
        [Tooltip("How much torque is applied to the bouncy barrel to rotate it when it jumps")]
        public float torqueAmount = 2f;

        [Header("Visual Settings")]
        [Tooltip("Visual effect spawned when this barrel breaks")]
        public GameObject breakEffectPrefab;
    }
}
