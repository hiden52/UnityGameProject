// GameMenu.cs
using UnityEngine;
using UnityEngine.UI;

public class GameMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button exitButton;

    private bool isMenuOpen = false;

    private void Start()
    {
        // 버튼 이벤트 연결
        if (resumeButton != null)
            resumeButton.onClick.AddListener(CloseMenu);

        if (saveButton != null)
            saveButton.onClick.AddListener(SaveGame);

        if (loadButton != null)
            loadButton.onClick.AddListener(LoadGame);

        if (exitButton != null)
            exitButton.onClick.AddListener(ExitGame);

        // 초기 상태 설정
        if (menuPanel != null)
            menuPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;

        if (menuPanel != null)
            menuPanel.SetActive(isMenuOpen);

        Time.timeScale = isMenuOpen ? 0 : 1;

        Cursor.visible = isMenuOpen;
        Cursor.lockState = isMenuOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void CloseMenu()
    {
        isMenuOpen = false;

        if (menuPanel != null)
            menuPanel.SetActive(false);

        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void SaveGame()
    {
        SaveLoadManager.Instance.SaveGame();
    }

    private void LoadGame()
    {
        SaveLoadManager.Instance.LoadGame();
        CloseMenu();
    }

    private void ExitGame()
    {
        //SaveLoadManager.Instance.SaveGame();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}