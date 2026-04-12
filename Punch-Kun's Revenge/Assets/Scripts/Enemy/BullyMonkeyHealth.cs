using UnityEngine;
using Unity.VisualScripting;

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
            if (GameManager.Instance != null)
                GameManager.Instance.AddScore(score);
            Destroy(gameObject);
        }
    }
}