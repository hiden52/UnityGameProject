using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineableObject : DefaultObject, IDamageable, IInteractable
{
    [SerializeField] private CountableItemData itemData;
    [SerializeField] private float maxDurability = 5;
    [SerializeField] private float currentDurability;
    [SerializeField] private Collider col;
    [SerializeField] private GameObject rock;
    [SerializeField] private GameObject debris;

    [SerializeField] private WeaponType requiredWeaponType = WeaponType.Tool;
    [SerializeField] private float damageMultiplierForCorrectTool = 1.25f;

    // ��ƼŬ �� �Ҹ�
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip breakSound;

    private AudioSource audioSource;

    public bool IsDead => currentDurability <= 0;

    private void Awake()
    {
        col = GetComponent<Collider>();
        currentDurability = maxDurability;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnEnable()
    {
        if (debris != null) debris.SetActive(false);
        if (rock != null) rock.SetActive(true);
        col.enabled = true;
        currentDurability = maxDurability;
    }

    public void TakeDamage( DamageInfo damageInfo)
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
        PlayHitEffect();

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
        TakeDamage(damageInfo);
    }

    private void PlayHitEffect()
    {
        // Hit effect
        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity, transform);
            Destroy(effect, 2f);
        }

        // HIt sound
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }

    private void HarvestComplete()
    {
        col.enabled = false;
        if (rock != null)
        {
            rock.SetActive(false);
        }

        if (debris != null)
        {
            debris.SetActive(true);
        }
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

        StartCoroutine(CleanupAfterDelay(3f));
    }
    private IEnumerator CleanupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (debris != null)
        {
            debris.SetActive(false);
        }

        ObjectPool.Instance.ReturnObject(gameObject);
    }
}
