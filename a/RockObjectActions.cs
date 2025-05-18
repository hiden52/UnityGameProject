using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockObjectActions : MineableObject
{
    [Header("For Destory Effect")]
    [SerializeField] private GameObject rock;
    [SerializeField] private GameObject debris;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (debris != null) debris.SetActive(false);
        if (rock != null) rock.SetActive(true);
    }

    protected override void HarvestComplete()
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

        base.HarvestComplete();

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
