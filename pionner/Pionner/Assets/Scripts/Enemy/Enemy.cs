using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHp = 50;
    private float currentHp;

    public bool IsDead => currentHp <= 0;

    private void Awake()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(DamageInfo damageInfo, Vector3 hitPoint)
    {
        currentHp -= damageInfo.DamageAmount;
        if (IsDead)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}