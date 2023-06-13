using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private Button m_tryAgainButton;
    [SerializeField] private Button m_mainMenuButton;

    private LevelManagementSystem m_levelManagementSystem;
    private MonoSystem m_monoSystem;

    private void Start()
    {
        m_levelManagementSystem = SystemLocator.Get<LevelManagementSystem>();
        m_monoSystem = SystemLocator.Get<MonoSystem>();

        m_levelManagementSystem.GameOver.AddListener(ShowScreen);
        
        m_levelManagementSystem.LevelLoaded.AddListener(() => gameObject.SetActive(false));
        
        m_tryAgainButton.onClick.AddListener(Retry);
        m_mainMenuButton.onClick.AddListener(GoToMainMenu);
        
        gameObject.SetActive(false);
    }

    private void ShowScreen()
    {
        m_monoSystem.SetTimeScale(0f);
        m_tryAgainButton.interactable = m_levelManagementSystem.CurrentLives > 0;
        gameObject.SetActive(true);
    }

    private void GoToMainMenu()
    {
        m_monoSystem.SetTimeScale(1f);
        gameObject.SetActive(false);
        m_levelManagementSystem.LoadMainMenu();
    }

    private void Retry()
    {
        m_monoSystem.SetTimeScale(1f);
        gameObject.SetActive(false);
        m_levelManagementSystem.ReloadCurrentLevel();
    }
}
