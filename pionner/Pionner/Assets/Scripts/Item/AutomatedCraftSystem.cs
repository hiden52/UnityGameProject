using UnityEngine;
using UnityEngine.UI;

public class AutomatedCraftSystem : BaseCraftSystem
{
    [SerializeField] private float productionInterval = 5f;
    [SerializeField] private int maxQueuedOperations = 5; // �ִ� ��⿭ ũ��
    [SerializeField] private Transform outputTransform; // ����� �������� ��ġ�� ��ġ

    [Header("UI References")]
    [SerializeField] private Slider progressSlider; // ���� ���� ǥ��
    [SerializeField] private Text statusText; // ���� �ؽ�Ʈ
    [SerializeField] private Text queueText; // ��⿭ ǥ��

    private int queuedOperations = 0; // ���� ��⿭ ũ��
    private float timeSinceLastProduction = 0f;
    private bool isAutoRunning = false;

    protected override void Awake()
    {
        base.Awake();

        // �ʿ��� ���� ����
        if (progressSlider == null)
            progressSlider = GetComponentInChildren<Slider>();
    }

    private void Update()
    {
        if (!isAutoRunning || recipeForwarder.SelectedRecipe == null)
            return;

        // ���� Ÿ�̸� ����
        if (isCrafting)
        {
            timeSinceLastProduction += Time.deltaTime;
            UpdateProgressUI();

            // ���� ���� ���� �� ������ ����
            if (timeSinceLastProduction >= productionInterval)
            {
                ProcessCraft();
                timeSinceLastProduction = 0f;
            }
        }
    }

    public void ToggleAutoProduction()
    {
        isAutoRunning = !isAutoRunning;

        if (isAutoRunning)
            StartCrafting();
        else
            CancelCrafting();

        UpdateStatusUI();
    }

    public override void StartCrafting()
    {
        base.StartCrafting();
        timeSinceLastProduction = 0f;
        UpdateStatusUI();
    }

    public override void CancelCrafting()
    {
        base.CancelCrafting();
        isAutoRunning = false;
        UpdateStatusUI();
    }

    public override void ProcessCraft()
    {
        if (!CanCraft())
        {
            CancelCrafting();
            return;
        }

        CompleteCrafting();

        // ��⿭�� �ִٸ� ó��
        if (queuedOperations > 0)
        {
            queuedOperations--;
            UpdateQueueUI();
        }
    }

    // �ڵ� ���� ť�� �۾� �߰�
    public void EnqueueOperation(int count = 1)
    {
        if (queuedOperations < maxQueuedOperations)
        {
            queuedOperations += count;
            queuedOperations = Mathf.Min(queuedOperations, maxQueuedOperations);
            UpdateQueueUI();

            // �ڵ�ȭ�� �����ִٸ� �ٽ� ����
            if (!isAutoRunning)
                ToggleAutoProduction();
        }
    }

    // ���� ���� ����
    public void SetProductionInterval(float interval)
    {
        productionInterval = interval;
    }

    private void UpdateProgressUI()
    {
        if (progressSlider != null)
        {
            progressSlider.value = timeSinceLastProduction / productionInterval;
        }
    }

    private void UpdateQueueUI()
    {
        if (queueText != null)
        {
            queueText.text = $"Queue: {queuedOperations} / {maxQueuedOperations}";
        }
    }

    private void UpdateStatusUI()
    {
        if (statusText != null)
        {
            statusText.text = isAutoRunning ? "Status: Running" : "Status: Stopped";
        }
    }

    public override void UpdateCraftState()
    {
        // ��ᰡ ������ ���� �ߴ�
        if (!CanCraft() && isAutoRunning)
        {
            CancelCrafting();
        }

        UpdateStatusUI();
    }
}