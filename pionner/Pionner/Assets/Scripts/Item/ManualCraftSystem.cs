using System.Collections;
using UnityEngine;

public class ManualCraftSystem : BaseCraftSystem
{
    [SerializeField] private ProgressionBarController progressionBar;
    [SerializeField] private CraftButtonAction craftButton;

    protected override void Awake()
    {
        base.Awake();

        if (progressionBar == null)
            progressionBar = GetComponentInChildren<ProgressionBarController>();

        if (craftButton == null)
            craftButton = GetComponentInChildren<CraftButtonAction>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (craftButton != null)
            craftButton.OnButtonStateChanged += HandleButtonStateChanged;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (craftButton != null)
            craftButton.OnButtonStateChanged -= HandleButtonStateChanged;
    }

    private void HandleButtonStateChanged(bool isPressed)
    {
        if (isPressed)
            StartCrafting();
        else
            CancelCrafting();
    }

    public override void StartCrafting()
    {
        base.StartCrafting();

        if (isCrafting)
        {
            craftingCoroutine = StartCoroutine(CraftingProcess());
        }
    }

    public override void CancelCrafting()
    {
        base.CancelCrafting();

        if (progressionBar != null)
        {
            progressionBar.SetCanFill(false);
            progressionBar.ResetProgression();
        }
    }

    public override void ProcessCraft()
    {
        if (progressionBar != null)
        {
            Debug.Log($"Progression Bar active state: {progressionBar.gameObject.activeSelf}, hierarchy active: {progressionBar.gameObject.activeInHierarchy}");

           //

            // �ڷ�ƾ ����
            progressionBar.SetCanFill(true);
            progressionBar.StartProgress();
        }
    }

    private IEnumerator CraftingProcess()
    {
        ProcessCraft();

        // �Ϸ�� ������ ���
        while (isCrafting && progressionBar.Progress < 1f)
        {
            yield return null;
        }

        if (isCrafting && progressionBar.Progress >= 1f)
        {
            CompleteCrafting();

            // ���� �Ϸ� �� ���� �ʱ�ȭ
            isCrafting = false;
            if (progressionBar != null)
                progressionBar.ResetProgression();
        }
    }

    public override void UpdateCraftState()
    {
        bool canCraft = CanCraft();

        // ��ư ���� ������Ʈ
        if (craftButton != null)
            craftButton.SetButtonInteractable(canCraft);

        // ���α׷��� �� ���� ������Ʈ
        if (!canCraft && isCrafting)
            CancelCrafting();
    }
}