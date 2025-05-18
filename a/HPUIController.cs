using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPUIController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerStat playerStat;
    [Header("Component")]
    [SerializeField] private Image hpBar;
    [SerializeField] private Text hpText;

    private void OnEnable()
    {
        if(playerStat == null)
        {
            Debug.LogError("[HPUIController] PlayerStat is Null");
            return;
        }

        playerStat.OnHealthChanged += UpdateHpDisplay;
        UpdateHpDisplay();
    }
    private void OnDisable()
    {
        if (playerStat != null)
        {
            playerStat.OnHealthChanged -= UpdateHpDisplay;
        }
    }
    private void UpdateHpDisplay()
    {
        if (playerStat == null) return;

        float currentHp = playerStat.CurrentHp;
        float maxHp = playerStat.MaxHp;

        if (hpBar != null)
        {
            hpBar.fillAmount = (maxHp > 0) ? (currentHp / maxHp) : 0;
        }

        if (hpText != null)
        {
            hpText.text = $"{Mathf.FloorToInt(currentHp)} / {Mathf.FloorToInt(maxHp)}";
        }
    }
}


