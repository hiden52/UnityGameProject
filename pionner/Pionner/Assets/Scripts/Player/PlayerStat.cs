using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float currentHp;

    public float MaxHp => maxHp;
    public float CurrentHp => currentHp;
    [SerializeField] public bool IsDead => currentHp <= 0f;

    [Header("Debug")]
    [SerializeField] public bool takeDamage;

    public event Action OnHealthChanged;
    public event Action OnDead;

    private void Awake()
    {
        currentHp = maxHp;
        takeDamage = false;
    }
    private void Update()
    {
        if (takeDamage)
        {
            Debug();
        }
    }
    public void TakeDamage(DamageInfo damageInfo)
    {
        currentHp = Mathf.Clamp(currentHp - damageInfo.DamageAmount, 0f, maxHp);
        OnHealthChanged?.Invoke();

        if (IsDead)
        {
            OnDead?.Invoke();
        }
    }

    private void Debug()
    {
        takeDamage = false;
        TakeDamage(new DamageInfo { DamageAmount = 10, Type = WeaponType.None });
    }

    public void Heal(float amount)
    {
        currentHp = Mathf.Clamp(currentHp + amount, 0f, maxHp);
        OnHealthChanged?.Invoke();
    }
}

