using System.Collections.Generic;
using UnityEngine;

//TODO: this and the ball obstacle pool should be merged into a single generic object pooling solutions with specific inheritors
public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private RectObstacle m_projectilePrefab;
    [SerializeField] private int m_initialPoolSize;
    
    private List<RectObstacle> m_pool = new();
    private List<RectObstacle> m_availableProjectiles = new();

    private void Awake()
    {
        for (int i = 0; i < m_initialPoolSize; i++)
        {
            RectObstacle projectile = Instantiate(m_projectilePrefab);
            m_pool.Add(projectile);
            projectile.Deactivate();
        }
        m_availableProjectiles.AddRange(m_pool);
        SystemLocator.Add(this);
    }
    
    public RectObstacle GetProjectile()
    {
        RectObstacle rectObstacle;
        if (m_availableProjectiles.Count > 0)
        {
            rectObstacle = m_availableProjectiles[0];
            m_availableProjectiles.Remove(rectObstacle);
        }
        else
        {
            rectObstacle = Instantiate(m_projectilePrefab);
            m_pool.Add(rectObstacle);
        }
        
        return rectObstacle;
    }
    
    public void ReturnRect(RectObstacle rectObstacle)
    {
        rectObstacle.Deactivate();
        m_availableProjectiles.Add(rectObstacle);
    }

    public void ClearActiveInstances()
    {
        foreach (RectObstacle obstacle in m_pool)
        {
            obstacle.Deactivate();
        }
        
        m_availableProjectiles.Clear();
        m_availableProjectiles.AddRange(m_pool);
    }
}
