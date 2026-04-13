using UnityEngine;

namespace Barrel
{
    /// <summary>
    /// 特定の樽のデータを持つ ScriptableObject。
    /// これにより、プレハブを直接変更することなく樽のパラメータを簡単に調整できます。
    /// </summary>
    [CreateAssetMenu(fileName = "NewBarrelData", menuName = "Barrel/BarrelData")]
    public class BarrelData : ScriptableObject
    {
        [Header("General Settings")]
        [Tooltip("この樽の種類")]
        public BarrelType barrelType;

        [Tooltip("この樽の標準的な移動速度")]
        public float moveSpeed = 5f;

        [Header("Jump Settings")]
        [Tooltip("樽がジャンプする高さの振幅")]
        public float jumpAmplitude = 2f;

        [Tooltip("樽がジャンプする頻度（速さ）")]
        public float jumpFrequency = 3f;

        public float verticleJumpForce = 2f;

        [Header("Torque Settings")]
        [Tooltip("ジャンプ時に樽を回転させるためのトルクの強さ")]
        public float torqueAmount = 2f;

        [Header("Visual Settings")]
        [Tooltip("この樽が破壊されたときに生成される視覚エフェクト")]
        public GameObject breakEffectPrefab;
    }
}
