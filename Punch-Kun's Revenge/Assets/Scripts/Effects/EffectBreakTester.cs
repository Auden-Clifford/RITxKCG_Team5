using UnityEngine;
using UnityEngine.InputSystem;

namespace Effects
{
    /// <summary>
    /// Test tool to spawn and verify break effects in a clean environment.
    /// Attach this to an empty GameObject in a new scene to test prefabs.
    /// </summary>
    public class EffectBreakTester : MonoBehaviour
    {
        [Header("Test Settings")]
        [Tooltip("The effect prefab you want to test")]
        [SerializeField] private GameObject _testPrefab;

        [Tooltip("Key to trigger the effect")]
        [SerializeField] private Key _triggerKey = Key.Space;

        [Tooltip("Instruction text to display in Game view")]
        [SerializeField] private string _instructions = "Press SPACE to spawn break effect";

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current[_triggerKey].wasPressedThisFrame)
            {
                SpawnEffect();
            }
        }

        private void SpawnEffect()
        {
            if (_testPrefab == null)
            {
                Debug.LogWarning("No test prefab assigned to EffectBreakTester!");
                return;
            }

            Debug.Log($"Spawning effect: {_testPrefab.name}");
            // Instantiate the effect at the center of the scene/tester
            GameObject effect = Instantiate(_testPrefab, transform.position, Quaternion.identity);
            if (effect.TryGetComponent(out SampleBreakEffect sbe))
            {
                sbe.Play();
            }
        }

        private void OnGUI()
        {
            // Simple overlay instructions
            GUIStyle style = new GUIStyle();
            style.fontSize = 24;
            style.normal.textColor = Color.white;
            GUI.Label(new Rect(20, 20, 500, 50), _instructions, style);
        }
    }
}
