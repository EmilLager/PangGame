using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private Button m_resumeButton;
    [SerializeField] private Button m_mainMenuButton;

    private LevelManagementSystem m_levelManagementSystem;
    private InputSystem m_inputSystem;
    private MonoSystem m_monoSystem;

    
    private void Start()
    {
        m_levelManagementSystem = SystemLocator.Get<LevelManagementSystem>();
        m_inputSystem = SystemLocator.Get<InputSystem>();
        m_monoSystem = SystemLocator.Get<MonoSystem>();
        
        m_inputSystem.OnPausePressed.AddListener(Pause);
        
        m_resumeButton.onClick.AddListener(UnPause);
        
        m_mainMenuButton.onClick.AddListener(GoToMainMenu);
        
        gameObject.SetActive(false);
    }

    private void GoToMainMenu()
    {
        m_monoSystem.SetTimeScale(1f);
        gameObject.SetActive(false);
        m_levelManagementSystem.LoadMainMenu();
    }

    private void UnPause()
    {
        m_monoSystem.SetTimeScale(1f);
        gameObject.SetActive(false);
    }

    private void Pause()
    {
        m_monoSystem.SetTimeScale(0f);
        gameObject.SetActive(true);
    }
}

