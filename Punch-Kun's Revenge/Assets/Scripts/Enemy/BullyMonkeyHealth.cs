using UnityEngine;

public class BullyMonkeyHealth : Health
{
    [SerializeField] private float score;

    public event System.Action OnBullyMonkeyDeath = delegate { };

    protected override void Start()
    {
        base.Start();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        // ** death **
        if (_currentHealth == 0)
        {
            Debug.Log("I died");
            if (GameManager.Instance != null)
                GameManager.Instance.AddScore(score);

            // Destroy(gameObject);
            // playing animation instead of destroying
            OnBullyMonkeyDeath?.Invoke();
        }
    }
}