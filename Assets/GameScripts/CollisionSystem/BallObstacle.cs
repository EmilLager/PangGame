using System;
using UnityEngine;

public class BallObstacle : MonoBehaviour
{
    [SerializeField] private float m_scaleRadiusRatio = 1f;
    [SerializeField] private float m_radius;
    [SerializeField] private SpriteRenderer m_spriteRenderer;

    public Vector2 Position => transform.position;
    public int Score => m_currentSizeTier.Score;
    
    public float Radius
    {
        get => m_radius;
        set
        {
            m_radius = value;
            transform.localScale = Vector3.one * m_radius * m_scaleRadiusRatio;
        }
    }

    private BallObstaclePool m_ballObstaclePool;

    private BallSettings m_setting;
    private int m_currentSizeTierIndex;
    private SizeTier m_currentSizeTier;
    private float m_horizontalSpeed;
    private float m_currentVerticalSpeed;
    private float m_maxVerticalSpeed;
    private float m_verticalAcceleration;
    

    private void Start()
    {
        m_ballObstaclePool = SystemLocator.Get<BallObstaclePool>();
    }

    private void MoveObstacle(float delta)
    {
        transform.position += (m_horizontalSpeed * Vector3.right + m_currentVerticalSpeed * Vector3.up) * delta;
        m_currentVerticalSpeed -= m_verticalAcceleration * delta;
    }
    
    public void HitWall()
    {
        m_horizontalSpeed *= -1f;
    }
    
    public void HitFloor()
    {
        m_currentVerticalSpeed = m_maxVerticalSpeed;
    }
    
    public void Break()
    {
        Deactivate();

        if (m_currentSizeTierIndex > 0)
        {
            BallObstacle otherBall = m_ballObstaclePool.GetBall();
            otherBall.transform.position = transform.position;
            otherBall.Initialize(m_setting, m_currentSizeTierIndex - 1, -1);
            Initialize(m_setting, m_currentSizeTierIndex - 1, 1);
        }
        else
        {
            m_ballObstaclePool.ReturnBall(this);
        }
    }

    public void Initialize(BallSettings setting, int tier, int direction)
    {
        m_setting = setting;
        m_currentSizeTierIndex = tier;
        m_currentSizeTier = m_setting.SizeTiers[tier];
        
        Radius = m_currentSizeTier.Radius;
        m_horizontalSpeed = m_currentSizeTier.HorizontalSpeed * direction;
        m_maxVerticalSpeed = m_currentSizeTier.MaxVerticalSpeed;
        m_currentVerticalSpeed = m_currentSizeTier.StartVerticalSpeed;
        m_verticalAcceleration = m_currentSizeTier.VerticalAcceleration;
        m_spriteRenderer.sprite = m_currentSizeTier.Sprite;
        m_spriteRenderer.color = m_currentSizeTier.SpriteColor;
        SystemLocator.Get<MonoSystem>().OnUpdate.AddListener(MoveObstacle);
        SystemLocator.Get<CollisionSystem>().AddBallObstacle(this);
        gameObject.SetActive(true);
    }
    
    public void Deactivate()
    {
        SystemLocator.Get<MonoSystem>().OnUpdate.RemoveListener(MoveObstacle);
        SystemLocator.Get<CollisionSystem>().RemoveBallObstacle(this);
        gameObject.SetActive(false);
    }
    
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        Radius = m_radius; //Used to set scale at editor time too
    }
#endif

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Gizmos.DrawSphere(transform.position, m_radius);
    }
}
