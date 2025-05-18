using UnityEngine;
using UnityEngine.UI;

public class AutomatedCraftSystem : BaseCraftSystem
{
    [SerializeField] private float productionInterval = 5f;
    [SerializeField] private int maxQueuedOperations = 5; // 최대 대기열 크기
    [SerializeField] private Transform outputTransform; // 생산된 아이템이 배치될 위치

    [Header("UI References")]
    [SerializeField] private Slider progressSlider; // 진행 상태 표시
    [SerializeField] private Text statusText; // 상태 텍스트
    [SerializeField] private Text queueText; // 대기열 표시

    private int queuedOperations = 0; // 현재 대기열 크기
    private float timeSinceLastProduction = 0f;
    private bool isAutoRunning = false;

    protected override void Awake()
    {
        base.Awake();

        // 필요한 참조 설정
        if (progressSlider == null)
            progressSlider = GetComponentInChildren<Slider>();
    }

    private void Update()
    {
        if (!isAutoRunning || recipeForwarder.SelectedRecipe == null)
            return;

        // 생산 타이머 갱신
        if (isCrafting)
        {
            timeSinceLastProduction += Time.deltaTime;
            UpdateProgressUI();

            // 생산 간격 충족 시 아이템 생산
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

        // 대기열이 있다면 처리
        if (queuedOperations > 0)
        {
            queuedOperations--;
            UpdateQueueUI();
        }
    }

    // 자동 생산 큐에 작업 추가
    public void EnqueueOperation(int count = 1)
    {
        if (queuedOperations < maxQueuedOperations)
        {
            queuedOperations += count;
            queuedOperations = Mathf.Min(queuedOperations, maxQueuedOperations);
            UpdateQueueUI();

            // 자동화가 멈춰있다면 다시 시작
            if (!isAutoRunning)
                ToggleAutoProduction();
        }
    }

    // 생산 간격 설정
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
        // 재료가 없으면 생산 중단
        if (!CanCraft() && isAutoRunning)
        {
            CancelCrafting();
        }

        UpdateStatusUI();
    }
}