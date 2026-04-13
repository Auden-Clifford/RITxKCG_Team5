using UnityEngine;

public class BullyMonkeyHealth : Health
{
    [SerializeField] private float score;

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
            Destroy(gameObject);
        }
    }
}