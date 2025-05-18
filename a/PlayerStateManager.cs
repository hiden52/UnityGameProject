using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PlayerState
{
    Unarmed,
    Armed,
    Builder,
}

public class PlayerStateManager : Singleton<PlayerStateManager>
{
    [SerializeField] private PlayerState currentState = PlayerState.Unarmed;
    [SerializeField] private PlayerState previousState = PlayerState.Unarmed;

    public PlayerState CurrentState => currentState;
    public PlayerState PreviousState => previousState;
    public bool IsArmed => currentState == PlayerState.Armed;

    public event Action<PlayerState> OnStateChanged;

    protected override void Awake()
    {
        base.Awake();
        // init
        SetState(PlayerState.Unarmed);
    }

    private void Start()
    {
        EquipmentManager.OnEquimentChagned += UpdatePlayerState;
    }

    private void OnDestroy()
    {
        EquipmentManager.OnEquimentChagned -= UpdatePlayerState;
    }

    private void UpdatePlayerState()
    {
        // 오른손에 무기가 장착되어 있는지 확인
        var itemOnHand = EquipmentManager.Instance.ItemOnHand;

        if (itemOnHand != null && itemOnHand is WeaponItem)
        {
            SetState(PlayerState.Armed);
        }
        else
        {
            SetState(PlayerState.Unarmed);
        }
    }
    public void ReturnToPreviousState()
    {
        if(currentState != previousState)
        {
            currentState = previousState;
        }
    }
    public void SetState(PlayerState newState)
    {
        if (currentState != newState)
        {
            previousState = currentState;
            currentState = newState;
            OnStateChanged?.Invoke(currentState);
        }
    }
}