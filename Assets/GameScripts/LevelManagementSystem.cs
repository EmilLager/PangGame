using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;

[CreateAssetMenu(fileName = "LevelManagementSystem", menuName = "ScriptableObjects/Settings/LevelManagementSystem")]
public class LevelManagementSystem : BaseScriptableObjectSystem
{
    [SerializeField] private List<LevelSettings> m_levelSettings;
    
    //TODO: this isn't the best place for keeping track of scores and lives (single responsibility) and would ideally be somewhere else
    [SerializeField] private int m_savedHighScoresAmount;
    [SerializeField] private int m_startingLives;
    
    public List<LevelSettings> LevelSettings => m_levelSettings;
    public UnityEvent LevelLoaded { get; } = new UnityEvent();
    public UnityEvent MainMenuLoaded { get; } = new UnityEvent();
    public UnityEvent GameOver { get; } = new UnityEvent();
    public UnityEvent LevelFinished { get; } = new UnityEvent();

    public List<int> HighScores => m_highScores;
    public int StartingLives => m_startingLives;
    public int CurrentLives => m_currentLives;

    private BallObstaclePool m_obstaclePool;
    private int m_currentLevel;
    private List<int> m_highScores;
    private int m_currentScore;
    private int m_currentLives;
    
    public override void Init() { }

    public override void StartSystem()
    {
        m_obstaclePool = SystemLocator.Get<BallObstaclePool>();
    }

    public void LoadLevel(int levelIndex)
    {
        m_currentLevel = levelIndex;
        m_currentScore = 0;
        m_currentLives = m_startingLives;
        m_obstaclePool.SpawnLevel(m_levelSettings[levelIndex]);
        LevelLoaded?.Invoke();
    }
    
    public void ReloadCurrentLevel()
    {
        m_currentLives--;
        m_obstaclePool.ObstaclesCleared.AddListener(OnLevelComplete);
        m_obstaclePool.SpawnLevel(m_levelSettings[m_currentLevel]);
        LevelLoaded?.Invoke();
    }

    private void OnLevelComplete()
    {
        m_obstaclePool.ObstaclesCleared.RemoveListener(OnLevelComplete);
        LevelFinished?.Invoke();
    }

    public void LoadNextLevel()
    {
        if (m_currentLevel < m_levelSettings.Count - 1)
        {
            m_currentLevel++;
            m_obstaclePool.ObstaclesCleared.AddListener(OnLevelComplete);
            m_obstaclePool.SpawnLevel(m_levelSettings[m_currentLevel]);
            LevelLoaded?.Invoke();
        }
        else
        {
            LoadMainMenu();
        }
    }
    
    public void LoadMainMenu()
    {
        UpdateHighScores(m_currentScore);
        m_currentScore = 0;
        m_currentLives = m_startingLives;
        m_obstaclePool.ClearActiveInstances();
        MainMenuLoaded?.Invoke();
    }

    public void AddScore(int score)
    {
        m_currentScore += score;
    }

    private void UpdateHighScores(int newScore)
    {
        m_highScores.Add(newScore);
        m_highScores = m_highScores.OrderByDescending(x => x).ToList();
        if (m_highScores.Count > m_savedHighScoresAmount)
        {
            m_highScores = m_highScores.GetRange(0, m_savedHighScoresAmount);
        }
    }
}
