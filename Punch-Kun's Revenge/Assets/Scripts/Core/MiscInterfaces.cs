/// <summary>
/// Interface for objects that can take damage but does not have specific health (e.g. destroyable objects, barrels, etc.)
/// </summary>
public interface IDamageable
{
    void TakeDamage();
}