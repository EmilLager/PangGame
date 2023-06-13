using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private InputSettings m_inputSettings;
    [SerializeField] private TMP_Text m_moveRightText;
    [SerializeField] private TMP_Text m_moveLeftText;
    [SerializeField] private TMP_Text m_fireText;
    [SerializeField] private TMP_Text m_pauseText;
    [SerializeField] private TMP_Text m_addPlayerText;
    [SerializeField] private TMP_Text m_player2MoveRight;
    [SerializeField] private TMP_Text m_player2MoveLeft;
    [SerializeField] private TMP_Text m_player2Fire;
    [SerializeField] private Image m_lifeIcon;
    [SerializeField] private RectTransform m_toutchControls;
    private LevelManagementSystem m_levelManagementSystem;

    private List<Image> m_lifeIcons = new List<Image>();

    private void Start()
    {
        m_levelManagementSystem = SystemLocator.Get<LevelManagementSystem>();

        m_levelManagementSystem.GameOver.AddListener(() => gameObject.SetActive(true));
        m_levelManagementSystem.LevelFinished.AddListener(() => gameObject.SetActive(false));
        m_levelManagementSystem.LevelLoaded.AddListener(() =>
        {
            UpdateLivesCount();
            gameObject.SetActive(true);
        });
        m_levelManagementSystem.MainMenuLoaded.AddListener(() => gameObject.SetActive(false));

        m_moveRightText.text = "Move right: " + m_inputSettings.Player1Controls.MoveRightKey;
        m_moveLeftText.text = "Move left: " + m_inputSettings.Player1Controls.MoveLeftKey;
        m_fireText.text = "Fire: " + m_inputSettings.Player1Controls.FireKey;
        m_pauseText.text = "Pause: " + m_inputSettings.PauseKey;
        m_addPlayerText.text = "Add player: " + m_inputSettings.AddPlayerKey;
        m_player2MoveRight.text = "Player2 move right: " + m_inputSettings.Player2Controls.MoveRightKey;
        m_player2MoveLeft.text = "Player2 move left: " + m_inputSettings.Player2Controls.MoveRightKey;
        m_player2Fire.text = "Player2 fire:" + m_inputSettings.Player2Controls.FireKey;
        
        gameObject.SetActive(false);
        
        m_lifeIcons.Add(m_lifeIcon);
        
        RectTransform baseRect = m_lifeIcon.GetComponent<RectTransform>();
        for (int i = 0; i < m_levelManagementSystem.StartingLives - 1; i++)
        {
            Image newImage = Instantiate(m_lifeIcon, m_lifeIcon.transform.parent);
            RectTransform rect = newImage.GetComponent<RectTransform>();
            var pos = rect.anchoredPosition;
            pos.x += baseRect.sizeDelta.x * (i + 1);
            rect.anchoredPosition = pos;
            m_lifeIcons.Add(newImage);
        }

#if UNITY_STANDALONE
        m_toutchControls.gameObject.SetActive(false);
#elif UNITY_ANDROID
        m_toutchControls.gameObject.SetActive(true);
#endif
    }

    private void UpdateLivesCount()
    {
        foreach (Image icon in m_lifeIcons)
        {
            icon.gameObject.SetActive(false);
        }
        
        for (int i = 0; i < m_levelManagementSystem.CurrentLives; i++)
        {
            m_lifeIcons[i].gameObject.SetActive(true);
        }
    }
}
