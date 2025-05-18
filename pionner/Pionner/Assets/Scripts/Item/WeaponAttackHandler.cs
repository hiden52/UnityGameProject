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
    [SerializeField] private Animator animator;

    private WeaponItem currentWeapon;
    [SerializeField] private bool isAttacking = false;
    private float lastAttackTime;
    private HashSet<Collider> hitColliders = new HashSet<Collider>();

    private PlayerMovementController movementController;

    public bool IsAttacking => isAttacking;

    private void Start()
    {
        if (rightHandPivot == null)
        {
            Debug.LogError("[WeaponAttackHandler] Right Hand Pivot is null");
        }
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
        }
        movementController = GetComponent<PlayerMovementController>();

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
        if (isAttacking || currentWeapon == null)
            return;

        isAttacking = true;

        if (movementController != null)
        {
            movementController.isAiming = true;
        }

        currentWeapon.Attack();

        if (animator != null)
        {
            if (currentWeapon.GetWeaponType() == WeaponType.Tool)
            {
                animator.SetTrigger("ToolAttack");
            }
            else if (currentWeapon.GetWeaponType() == WeaponType.Sword)
            {
                animator.SetTrigger("SwordAttack");
            }
            else
            {
                animator.SetTrigger("Attack");
            }
        }
    }

    public void OnAttackStart()
    {
        EnableWeaponCollider();
    }

    public void OnAttackEnd()
    {
        DisableWeaponCollider();
    }

    public void OnAttackComplete()
    {
        isAttacking = false;

        if (movementController != null && movementController.moveDirection.magnitude < 0.1f)
        {
            movementController.isAiming = false;
        }
    }

    public void EnableWeaponCollider()
    {
        if (attackCollider == null) return;

        attackCollider.enabled = true;
        hitColliders.Clear();
    }

    public void DisableWeaponCollider()
    {
        if (attackCollider == null) return;

        attackCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAttacking || currentWeapon == null) return;
        if (hitColliders.Contains(other)) return;

        if (((1 << other.gameObject.layer) & attackableLayers) == 0) return;

        hitColliders.Add(other);

        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
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

    private void PlayHitEffect(Vector3 position, Vector3 normal)
    {
        // Hit effect
    }


    //private void HandleAttack()
    //{
    //    if (isAttacking || Time.time - lastAttackTime < attackCooldown || currentWeapon == null) return;

    //    currentWeapon.Attack();


    //    if (currentWeapon.GetWeaponType() == WeaponType.Tool)
    //    {
    //        //
    //    }
    //}
    //public void EnableWeaponCollider()
    //{
    //    if (attackCollider == null) return;

    //    isAttacking = true;
    //    attackCollider.enabled = true;
    //    hitColliders.Clear();
    //}
    //public void DisableWeaponCollider()
    //{
    //    if (attackCollider == null) return;

    //    isAttacking = false;
    //    attackCollider.enabled = false;
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log(other.name);
    //    if (!isAttacking || currentWeapon == null) return;
    //    if (hitColliders.Contains(other)) return;

    //    if (((1 << other.gameObject.layer) & attackableLayers) == 0) return;

    //    hitColliders.Add(other);

    //    IDamageable damageable = other.GetComponent<IDamageable>();
    //    if(damageable != null)
    //    {
    //        DamageInfo damageInfo = new DamageInfo
    //        {
    //            DamageAmount = currentWeapon.GetDamage(),
    //            Type = currentWeapon.GetWeaponType(),
    //        };


    //        Vector3 hitPoint = other.ClosestPoint(attackCollider.bounds.center);
    //        Vector3 hitNormal = (hitPoint - attackCollider.bounds.center).normalized;
    //        damageable.TakeDamage(damageInfo, hitPoint);
    //        PlayHitEffect(hitPoint, hitNormal);
    //    }

    //}

    //private void PlayHitEffect(Vector3 position, Vector3 normal)
    //{
    //    // Hit effect
    //    // Instantiate(hitEffectPrefab, position, Quaternion.LookRotation(normal));
    //    // audioSource.PlayOneShot(hitSound);
    //}

}
