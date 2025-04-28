using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttackHandler : MonoBehaviour
{
    [SerializeField] public WeaponItem CurrentWeapon => currentWeapon;

    [SerializeField] private float attackRange = 2f;
    [SerializeField] private LayerMask attackableLayers;
    [SerializeField] private Transform attackOrigin; // 주로 카메라 또는 무기 위치

    private WeaponItem currentWeapon;

    private void Start()
    {
        if (attackOrigin == null)
        {
            attackOrigin = Camera.main.transform;
        }

        EquipmentManager.OnEquimentChagned += UpdateCurrentWeapon;
        PlayerInputManager.Instance.OnAttackPressed += HandleAttack;

        UpdateCurrentWeapon();
    }

    private void OnDestroy()
    {
        EquipmentManager.OnEquimentChagned -= UpdateCurrentWeapon;
        PlayerInputManager.Instance.OnAttackPressed -= HandleAttack;
    }

    private void UpdateCurrentWeapon()
    {
        currentWeapon = EquipmentManager.Instance.ItemOnHand as WeaponItem;
    }

    private void HandleAttack()
    {
        if (currentWeapon == null) return;

        currentWeapon.Attack();

        
        if (currentWeapon.GetWeaponType() == WeaponType.Tool)
        {
            //
        }
    }

    public void AnimationHitCheck()
    {
        if (Physics.Raycast(attackOrigin.position, attackOrigin.forward, out RaycastHit hit, attackRange, attackableLayers))
        { 
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                DamageInfo damageInfo = new DamageInfo { DamageAmount = currentWeapon.GetDamage(), Type = currentWeapon.GetWeaponType() };
                damageable.TakeDamage(damageInfo);

                PlayHitEffect(hit.point, hit.normal);
            }
        }
    }

    private void PlayHitEffect(Vector3 position, Vector3 normal)
    {
        // Hit effect
        // Instantiate(hitEffectPrefab, position, Quaternion.LookRotation(normal));
        // audioSource.PlayOneShot(hitSound);
    }

}
