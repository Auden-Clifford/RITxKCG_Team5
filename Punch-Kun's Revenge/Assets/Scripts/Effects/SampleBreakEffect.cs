using UnityEngine;

namespace Effects
{
    /// <summary>
    /// A sample break effect that spawns primitive fragments with random physics forces.
    /// This acts as a placeholder for actual particle effects.
    /// </summary>
    public class SampleBreakEffect : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int _fragmentCount = 10;
        [SerializeField] private float _explosionForce = 5f;
        [SerializeField] private float _fragmentScale = 0.3f;
        [SerializeField] private Color _fragmentColor = new Color(0.5f, 0.3f, 0.1f); // Wood-like color

        /// <summary>
        /// Manually trigger the effect. 
        /// Called when the barrel breaks or when testing.
        /// </summary>
        public void Play()
        {
            GenerateFragments();
            
            // Ensure the parent object itself is also cleaned up
            if (!TryGetComponent<AutoDestroy>(out _))
            {
                gameObject.AddComponent<AutoDestroy>();
            }
        }

        private void GenerateFragments()
        {
            for (int i = 0; i < _fragmentCount; i++)
            {
                // Create a simple cube as a fragment
                GameObject fragment = GameObject.CreatePrimitive(PrimitiveType.Cube);
                
                // Disable collision with physics objects but keep world interaction
                if (fragment.TryGetComponent(out Collider col))
                {
                    col.isTrigger = true;
                }
                
                fragment.transform.position = transform.position + Random.insideUnitSphere * 0.2f;
                fragment.transform.localScale = Vector3.one * Random.Range(_fragmentScale * 0.5f, _fragmentScale);
                fragment.transform.rotation = Random.rotation;

                // Set color
                if (fragment.TryGetComponent(out Renderer renderer))
                {
                    renderer.material.color = _fragmentColor;
                }

                // Add physics
                Rigidbody rb = fragment.AddComponent<Rigidbody>();
                Vector3 force = Random.onUnitSphere * _explosionForce;
                force.y = Mathf.Abs(force.y); // Ensure some upward momentum
                rb.AddForce(force, ForceMode.Impulse);
                rb.AddTorque(Random.onUnitSphere * 10f, ForceMode.Impulse);

                // Ensure fragment is cleaned up eventually
                fragment.AddComponent<AutoDestroy>();
                
                // Add to this effect's lifecycle (optional)
                fragment.transform.SetParent(this.transform);
            }
        }
    }
}
