public interface IDamageable : IHitable
{
    void TakeDamage(int damageAmount, float knockBackThrust);
}