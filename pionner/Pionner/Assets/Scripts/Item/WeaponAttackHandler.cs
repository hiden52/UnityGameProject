using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponAttackHandler : MonoBehaviour
{
    [SerializeField] public WeaponItem CurrentWeapon => currentWeapon;

    [SerializeField] private LayerMask attackableLayers;
    [SerializeField] private Transform rightHandPivot; 
    [SerializeField] private Collider attackCollider;

    private WeaponItem currentWeapon;
    private bool isAttacking = false;
    private float lastAttackTime;
    private HashSet<Collider> hitColliders = new HashSet<Collider>();

    private void Start()
    {
        if (rightHandPivot == null)
        {
            Debug.LogError("[WeaponAttackHandler] Right Hand Pivot is null");
        }

        EquipmentManager.OnEquimentChagned += UpdateCurrentWeapon;
        PlayerInputManager.Instance.OnAttackPressed += HandleAttack;

        UpdateCurrentWeapon();
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
            attackCollider.isTrigger = true;
        }
    }

    private void OnDestroy()
    {
        EquipmentManager.OnEquimentChagned -= UpdateCurrentWeapon;
        PlayerInputManager.Instance.OnAttackPressed -= HandleAttack;
    }

    private void UpdateCurrentWeapon()
    {
        currentWeapon = EquipmentManager.Instance.ItemOnHand as WeaponItem;
        attackCollider = rightHandPivot.GetComponentInChildren<Collider>();
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
    public void EnableWeaponCollider()
    {
        if (attackCollider == null) return;

        isAttacking = true;
        attackCollider.enabled = true;
        hitColliders.Clear();
    }
    public void DisableWeaponCollider()
    {
        if (attackCollider == null) return;

        isAttacking = false;
        attackCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAttacking || currentWeapon == null) return;
        if (hitColliders.Contains(other)) return;

        if (((1 << other.gameObject.layer) & attackableLayers) == 0) return;

        hitColliders.Add(other);

        IDamageable damageable = other.GetComponent<IDamageable>();
        if(damageable != null)
        {
            DamageInfo damageInfo = new DamageInfo
            {
                DamageAmount = currentWeapon.GetDamage(),
                Type = currentWeapon.GetWeaponType(),
            };
           

            Vector3 hitPoint = other.ClosestPoint(attackCollider.bounds.center);
            Vector3 hitNormal = (hitPoint - attackCollider.bounds.center).normalized;
            damageable.TakeDamage(damageInfo, hitPoint);
            PlayHitEffect(hitPoint, hitNormal);
        }

    }

    //public void AnimationHitCheck()
    //{
    //    if(attackCollider == null) return;
    //    attackCollider.enabled = true;
    //    if (Physics.Raycast(attackOrigin.position, attackOrigin.forward, out RaycastHit hit, attackRange, attackableLayers))
    //    { 
    //        IDamageable damageable = hit.collider.GetComponent<IDamageable>();
    //        if (damageable != null)
    //        {
    //            DamageInfo damageInfo = new DamageInfo { DamageAmount = currentWeapon.GetDamage(), Type = currentWeapon.GetWeaponType() };
    //            damageable.TakeDamage(damageInfo);

    //            PlayHitEffect(hit.point, hit.normal);
    //        }
    //    }
    //}

    private void PlayHitEffect(Vector3 position, Vector3 normal)
    {
        // Hit effect
        // Instantiate(hitEffectPrefab, position, Quaternion.LookRotation(normal));
        // audioSource.PlayOneShot(hitSound);
    }

}
