using UnityEngine;

public interface IDamageable
{
    void TakeDamage(DamageInfo damageInfo, Vector3 hitPoint);
    bool IsDead { get; }
}