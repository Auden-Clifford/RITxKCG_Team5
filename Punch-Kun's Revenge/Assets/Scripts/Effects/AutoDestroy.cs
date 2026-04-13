using UnityEngine;

namespace Effects
{
    /// <summary>
    /// Utility script to automatically destroy a GameObject after a specified time.
    /// Useful for visual effects and temporary objects.
    /// </summary>
    public class AutoDestroy : MonoBehaviour
    {
        [SerializeField] private float _delay = 2.0f;

        /// <summary>
        /// Set the destruction delay at runtime.
        /// </summary>
        public void SetDelay(float delay)
        {
            _delay = delay;
        }

        private void Start()
        {
            Destroy(gameObject, _delay);
        }
    }
}
