using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineableObject : DefaultObject, IDamageable, IInteractable
{
    [SerializeField] private CountableItemData itemData;
    [SerializeField] private float maxDurability = 5;
    [SerializeField] private float currentDurability;
    [SerializeField] protected Collider col;

    [SerializeField] private WeaponType requiredWeaponType = WeaponType.Tool;
    [SerializeField] private float damageMultiplierForCorrectTool = 1.25f;

    // ��ƼŬ �� �Ҹ�
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip breakSound;

    [SerializeField] private Vector3 knockAngle;
    [SerializeField] private AnimationCurve knockCurve;
    [SerializeField] private float knockDuration = 1;

    private AudioSource audioSource;

    public bool IsDead => currentDurability <= 0;

    protected virtual void Awake()
    {
        col = GetComponent<Collider>();
        currentDurability = maxDurability;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    protected virtual void OnEnable()
    {
        col.enabled = true;
        currentDurability = maxDurability;
    }

    public void TakeDamage( DamageInfo damageInfo, Vector3 hitPoint)
    {
        float actualDamage = damageInfo.DamageAmount;
        if (damageInfo.Type == requiredWeaponType)
        {
            actualDamage *= damageMultiplierForCorrectTool;
        }
        else if (damageInfo.Type == WeaponType.None)
        {
            // �Ǽ��� ��� ������ ����
            actualDamage *= 0.5f;
        }

        // ���� ������ ����
        currentDurability = Mathf.Max(0, currentDurability - actualDamage);

        // ��Ʈ ����Ʈ ���
        PlayHitEffect(hitPoint);

        // �������� 0�� �Ǹ� �ı� ó��
        if (IsDead)
        {
            HarvestComplete();
        }
    }

    public void Interact()
    {
        
        float damage = 1f; // �⺻ ������

        WeaponItem equippedWeapon = EquipmentManager.Instance.ItemOnHand as WeaponItem;
        if (equippedWeapon != null)
        {
            damage = equippedWeapon.GetDamage();
        }
        DamageInfo damageInfo = new DamageInfo { DamageAmount = 1f, Type = WeaponType.None };
        TakeDamage(damageInfo, transform.position);
    }

    protected virtual void PlayHitEffect(Vector3 point)
    {
        // Hit effect
        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitEffectPrefab, point, Quaternion.identity, transform);
            Destroy(effect, 2f);
        }

        // HIt sound
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
        StartCoroutine(Animate());
    }

    protected virtual void HarvestComplete()
    {
        if (audioSource != null && breakSound != null)
        {
            audioSource.PlayOneShot(breakSound);
        }

        // ������ ȹ��
        if (itemData != null)
        {
            int amount = Random.Range(1, 6);
            InventoryManager.Instance.AddItem(itemData, amount);
        }
        
    }

    private IEnumerator Animate() //Knock animation coroutine.
    {
        float t = 0;
        while (t < knockDuration)
        {
            float v = knockCurve.Evaluate(t / knockDuration);
            transform.localRotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(knockAngle), v);
            t += Time.deltaTime;
            yield return null;
        }
    }
}
