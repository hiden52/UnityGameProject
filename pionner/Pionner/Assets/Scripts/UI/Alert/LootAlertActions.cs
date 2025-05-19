using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootAlertActions : ItemAlertActions
{
    [SerializeField] private string lootFormat = "{0} È¹µæ!";
    [SerializeField] private ParticleSystem lootParticle;

    protected override void Awake()
    {
        base.Awake();
        alertFormat = lootFormat;
    }

    public override void ShowItemAlert(ItemData itemData, int quantity = 1)
    {
        base.ShowItemAlert(itemData, quantity);

        if (lootParticle != null)
        {
            lootParticle.Play();
        }
    }
}
