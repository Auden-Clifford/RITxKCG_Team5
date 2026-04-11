using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [Tooltip("Max health")]
    [SerializeField] protected int _maxHealth = 5;

    protected int _currentHealth;

    protected virtual void Start()
    {
        // init
        _currentHealth = _maxHealth;
    }

    public virtual void TakeDamage(int damage = 1)
    {
        _currentHealth = Mathf.Max(0, _currentHealth - damage);

        Debug.LogWarning($"{gameObject.name} took {damage} damage! Current health: {_currentHealth}");
    }
}