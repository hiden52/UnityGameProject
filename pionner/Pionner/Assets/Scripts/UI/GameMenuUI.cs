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

        isMenuOpen = false;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        menuPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    private void SaveGame()
    {
        if (SaveLoadManager.Instance != null)
        {
            SaveLoadManager.Instance.SaveGame();
            CloseMenu();
        }
    }

    private void LoadGame()
    {
        if (SaveLoadManager.Instance != null)
        {
            SaveLoadManager.Instance.LoadGame();
            CloseMenu();
        }
    }

    private void CloseMenu()
    {
        UIManager.Instance.CloseGameMenu();
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