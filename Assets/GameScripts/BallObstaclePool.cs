using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallObstaclePool : MonoBehaviour
{
    [SerializeField] private BallObstacle m_ballPrefab;
    [SerializeField] private int m_initialPoolSize;
    
    private List<BallObstacle> m_pool = new List<BallObstacle>();
    private List<BallObstacle> m_availableBallObstacles = new List<BallObstacle>();
    
    public UnityEvent ObstaclesCleared { get; } = new UnityEvent();

    private void Awake()
    {
        for (int i = 0; i < m_initialPoolSize; i++)
        {
            BallObstacle ball = Instantiate(m_ballPrefab);
            m_pool.Add(ball);
            ball.Deactivate();
        }
        m_availableBallObstacles.AddRange(m_pool);
        SystemLocator.Add(this);
    }

    public BallObstacle GetBall()
    {
        BallObstacle ballObstacle;
        if (m_availableBallObstacles.Count > 0)
        {
            ballObstacle = m_availableBallObstacles[0];
            m_availableBallObstacles.Remove(ballObstacle);
        }
        else
        {
            ballObstacle = Instantiate(m_ballPrefab);
            m_pool.Add(ballObstacle);
        }
        
        return ballObstacle;
    }
    
    public void ReturnBall(BallObstacle ballObstacle)
    {
        ballObstacle.Deactivate();
        m_availableBallObstacles.Add(ballObstacle);
        
        if (m_pool.Count == m_availableBallObstacles.Count)
        {
            ObstaclesCleared?.Invoke();
        }
    }

    public void ClearActiveInstances()
    {
        foreach (BallObstacle obstacle in m_pool)
        {
            obstacle.Deactivate();
        }
        
        m_availableBallObstacles.Clear();
        m_availableBallObstacles.AddRange(m_pool);
    }

    public void SpawnLevel(LevelSettings levelSettings)
    {
        ClearActiveInstances();
        foreach (StartingObstacle setting in levelSettings.StartingObstacles)
        {
            BallObstacle ball = GetBall();
            ball.transform.position = setting.StartPosition;
            ball.Initialize(setting.BallData, setting.BallData.SizeTiers.Count - 1, setting.StartDirection);
        }
    }
}
