using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteScreen : MonoBehaviour
{
    [SerializeField] private Button m_nextLevelButton;
    [SerializeField] private Button m_mainMenuButton;

    private LevelManagementSystem m_levelManagementSystem;

    private void Start()
    {
        m_levelManagementSystem = SystemLocator.Get<LevelManagementSystem>();

        m_levelManagementSystem.LevelFinished.AddListener(ShowScreen);
        
        m_levelManagementSystem.LevelLoaded.AddListener(() => gameObject.SetActive(false));
        
        m_nextLevelButton.onClick.AddListener(NextLevel);
        m_mainMenuButton.onClick.AddListener(GoToMainMenu);
        
        gameObject.SetActive(false);
    }

    private void ShowScreen()
    {
        gameObject.SetActive(true);
    }

    private void GoToMainMenu()
    {
        gameObject.SetActive(false);
        m_levelManagementSystem.LoadMainMenu();
    }

    private void NextLevel()
    {
        gameObject.SetActive(false);
        m_levelManagementSystem.LoadNextLevel();
    }
}
