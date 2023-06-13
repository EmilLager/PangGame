using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button m_levelButton;
    [SerializeField] private ScrollRect m_levelScrollRect;
    [SerializeField] private TMP_Text m_highscoreText;
    [SerializeField] private ScrollRect m_highScoresScrollRect;
    [SerializeField] private List<string> m_highScoreFormats;

    private LevelManagementSystem m_levelManagementSystem;
    private List<TMP_Text> m_highScoreTexts = new();
    
    private void Start()
    {
        m_levelManagementSystem = SystemLocator.Get<LevelManagementSystem>();
        int levelCount = m_levelManagementSystem.LevelSettings.Count;

        SetupButton(m_levelButton, 0);
        
        for (int i = 0; i < levelCount - 1; i++)
        {
            Button newButton = Instantiate(m_levelButton, m_levelButton.transform.parent);
            int levelIndex = i + 1;
            SetupButton(newButton, levelIndex);
        }

        Vector2 sizeDelta = m_levelScrollRect.content.sizeDelta;
        sizeDelta.y *= levelCount;
        m_levelScrollRect.content.sizeDelta = sizeDelta;
        
        m_highScoreTexts.Add(m_highscoreText);

        for (int i = 0; i < m_highScoreFormats.Count - 1; i++)
        {
            TMP_Text newText = Instantiate(m_highscoreText, m_highscoreText.transform.parent);
            int highScoreIndex = i + 1;
            SetupHighScoreText(newText, highScoreIndex);
            m_highScoreTexts.Add(newText);
        }

        sizeDelta = m_highScoresScrollRect.content.sizeDelta;
        sizeDelta.y *= m_highScoreFormats.Count;
        m_highScoresScrollRect.content.sizeDelta = sizeDelta;
        
        m_levelManagementSystem.GameOver.AddListener(() => gameObject.SetActive(false));
        m_levelManagementSystem.LevelFinished.AddListener(() => gameObject.SetActive(false));
        m_levelManagementSystem.LevelLoaded.AddListener(() => gameObject.SetActive(false));
        m_levelManagementSystem.MainMenuLoaded.AddListener(() =>
        {
            UpdateScores(m_levelManagementSystem.HighScores);
            gameObject.SetActive(true);
        });
    }

    private void SetupButton(Button button, int levelIndex)
    {
        button.onClick.AddListener(() => m_levelManagementSystem.LoadLevel(levelIndex));
        RectTransform rect = button.GetComponent<RectTransform>();
        var pos = rect.anchoredPosition;
        pos.y -= m_levelScrollRect.content.sizeDelta.y * levelIndex;
        rect.anchoredPosition = pos;
        button.GetComponentInChildren<TMP_Text>().text = "Level " + (levelIndex + 1);
    }
    
    private void SetupHighScoreText(TMP_Text text, int highscoreIndex)
    {
        RectTransform rect = text.GetComponent<RectTransform>();
        var pos = rect.anchoredPosition;
        pos.y -= m_highScoresScrollRect.content.sizeDelta.y * highscoreIndex;
        rect.anchoredPosition = pos;
        text.gameObject.SetActive(false);
    }
    
    private void UpdateScores(List<int> scores)
    {
        for (int i = 0; i < m_highScoreFormats.Count && i < scores.Count; i++)
        {
            m_highScoreTexts[i].text = String.Format(m_highScoreFormats[i], scores[i]);
            m_highScoreTexts[i].gameObject.SetActive(true);
        }
    }
}
