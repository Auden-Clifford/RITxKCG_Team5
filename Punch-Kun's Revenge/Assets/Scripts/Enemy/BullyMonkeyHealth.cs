public class BullyMonkeyHealth : Health
{
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        // ** death **
        if (_currentHealth == 0) Destroy(gameObject);
    }
}