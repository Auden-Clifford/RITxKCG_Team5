using UnityEngine;

namespace Effects
{
    /// <summary>
    /// A sample break effect that spawns primitive fragments with random physics forces.
    /// This acts as a placeholder for actual particle effects.
    /// </summary>
    [System.Serializable]
    public struct FragmentColorOption
    {
        public Color color;

        [Tooltip("Higher weight makes this color appear more often. 0 means it never appears.")]
        public float weight;
    }

    public class SampleBreakEffect : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int _fragmentCount = 10;

        [SerializeField] private float _explosionForce = 5f;
        [SerializeField] private float _fragmentScale = 0.3f;

        [Header("Color Settings")]
        [Tooltip("List of colors and their relative probabilities. If empty, the material's original color is kept.")]
        [SerializeField]
        private FragmentColorOption[] _colorOptions = new FragmentColorOption[]
        {
            new FragmentColorOption { color = new Color(0.5f, 0.3f, 0.1f), weight = 1f },
            new FragmentColorOption { color = new Color(0.7f, 0.5f, 0.2f), weight = 1f }
        };

        /// <summary>
        /// Manually trigger the effect.
        /// <param name="source">The object being broken. Its visuals will be used for fragments.</param>
        /// </summary>
        public void Play(GameObject source = null)
        {
            if (source != null)
            {
                GenerateFragmentsFromSource(source);
            }
            else
            {
                GenerateFragments();
            }

            // Ensure the parent object itself is also cleaned up
            if (!TryGetComponent<AutoDestroy>(out _))
            {
                gameObject.AddComponent<AutoDestroy>();
            }
        }

        private bool TryGetRandomColor(out Color result)
        {
            if (_colorOptions == null || _colorOptions.Length == 0)
            {
                result = Color.white;
                return false;
            }

            float totalWeight = 0f;
            foreach (var option in _colorOptions)
            {
                totalWeight += option.weight;
            }

            if (totalWeight <= 0f)
            {
                result = _colorOptions[0].color;
                return true;
            }

            float randomValue = Random.Range(0f, totalWeight);
            float currentWeight = 0f;

            foreach (var option in _colorOptions)
            {
                currentWeight += option.weight;
                if (randomValue <= currentWeight)
                {
                    result = option.color;
                    return true;
                }
            }

            result = _colorOptions[_colorOptions.Length - 1].color;
            return true;
        }

        private void GenerateFragments()
        {
            // Fallback: Create simple cubes
            for (int i = 0; i < _fragmentCount; i++)
            {
                GameObject fragment = GameObject.CreatePrimitive(PrimitiveType.Cube);

                if (fragment.TryGetComponent(out Renderer rend))
                {
                    if (TryGetRandomColor(out Color chosenColor))
                    {
                        rend.material.color = chosenColor;
                    }
                }

                SetupFragment(fragment, null);
            }
        }

        private void GenerateFragmentsFromSource(GameObject source)
        {
            Mesh sourceMesh = null;
            Material sourceMaterial = null;

            sourceMesh = source.GetComponentInChildren<MeshFilter>().sharedMesh;
            sourceMaterial = source.GetComponentInChildren<Renderer>().sharedMaterial;

            // if (source.TryGetComponent(out MeshFilter mf)) sourceMesh = mf.sharedMesh;
            // if (source.TryGetComponent(out Renderer rend)) sourceMaterial = rend.sharedMaterial;

            for (int i = 0; i < _fragmentCount; i++)
            {
                GameObject fragment = new GameObject("Fragment");

                // Add graphics
                MeshFilter fragmentMf = fragment.AddComponent<MeshFilter>();
                MeshRenderer fragmentRend = fragment.AddComponent<MeshRenderer>();

                if (sourceMesh != null)
                {
                    fragmentMf.sharedMesh = sourceMesh;
                }
                else
                {
                    // Fallback to cube if no mesh found
                    GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    fragmentMf.sharedMesh = temp.GetComponent<MeshFilter>().sharedMesh;
                    Destroy(temp);
                }

                bool hasTintColor = TryGetRandomColor(out Color chosenColor);

                if (sourceMaterial != null)
                {
                    if (hasTintColor && sourceMaterial.HasProperty("_Color"))
                    {
                        // Create a new material instance to apply the tint
                        fragmentRend.material = new Material(sourceMaterial);
                        fragmentRend.material.color = sourceMaterial.color * chosenColor;
                    }
                    else
                    {
                        // Fallback to shared material if no valid colors are set
                        fragmentRend.sharedMaterial = sourceMaterial;
                    }
                }
                else
                {
                    if (hasTintColor)
                    {
                        fragmentRend.material.color = chosenColor;
                    }
                }

                SetupFragment(fragment, source.GetComponent<Collider>());
            }
        }

        private void SetupFragment(GameObject fragment, Collider sourceCollider)
        {
            // Position
            fragment.transform.position = transform.position + Random.insideUnitSphere * 0.2f;
            fragment.transform.localScale = Vector3.one * Random.Range(_fragmentScale * 0.5f, _fragmentScale);
            fragment.transform.rotation = Random.rotation;

            // Physics and Cleanup
            if (fragment.TryGetComponent(out Collider col))
            {
                col.isTrigger = true;
                if (sourceCollider != null) Physics.IgnoreCollision(col, sourceCollider);
            }

            if (!fragment.TryGetComponent(out Rigidbody rb))
            {
                rb = fragment.AddComponent<Rigidbody>();
            }
            Vector3 force = Random.onUnitSphere * _explosionForce;
            force.y = Mathf.Abs(force.y);
            rb.AddForce(force, ForceMode.Impulse);
            rb.AddTorque(Random.onUnitSphere * 10f, ForceMode.Impulse);

            fragment.AddComponent<AutoDestroy>();
            fragment.transform.SetParent(this.transform);
        }
    }
}