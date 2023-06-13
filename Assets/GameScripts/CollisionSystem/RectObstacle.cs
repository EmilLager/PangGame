using UnityEngine;
using UnityEngine.Events;

public class RectObstacle : MonoBehaviour
{
    [SerializeField] private bool m_registerOnAwake;
    [SerializeField] private CollisionObjectType m_collisionType;
    [SerializeField] protected RectData m_rectData;
    [SerializeField] private bool m_updateRectWithPosition;
    [SerializeField] private bool m_matchScaleToRect;

    private CollisionSystem m_collisionSystem;
    public UnityEvent OnHitObstacle { get; } = new();
    
    public CollisionObjectType CollisionType => m_collisionType;
    public RectData RectData => m_rectData + (m_updateRectWithPosition ? transform.position : Vector3.zero);

    private Vector2 m_scaleVector;
    private void Awake()
    {
        m_collisionSystem = SystemLocator.Get<CollisionSystem>();
        if (m_registerOnAwake)
        {
            m_collisionSystem.AddRectObstacle(this);
        }

        m_scaleVector.x = transform.localScale.x / Mathf.Abs(m_rectData.point1.x - m_rectData.point2.x);
        m_scaleVector.y = transform.localScale.y / Mathf.Abs(m_rectData.point3.y - m_rectData.point2.y);
    }

    public void TakeHit()
    {
        OnHitObstacle?.Invoke();
    }

    public void Activate()
    {
        m_collisionSystem.AddRectObstacle(this);
        gameObject.SetActive(true);
    }
    
    public void Deactivate()
    {
        m_collisionSystem.RemoveRectObstacle(this);
        gameObject.SetActive(false);
    }

    public void SetRectData(RectData rectData)
    {
        m_rectData = rectData;
        if (m_matchScaleToRect)
        {
            //Debug.Log($"rectData {rectData.point1} {rectData.point2} {rectData.point3} {rectData.point4} scale {m_scaleVector}");
            //Vector2 delta = rectData.rectCenter - (Vector2) transform.position;
            //transform.position = rectData.rectCenter;
            transform.localScale = new Vector3(
                Mathf.Abs(m_rectData.point1.x - m_rectData.point2.x) * m_scaleVector.x,
                Mathf.Abs(m_rectData.point3.y - m_rectData.point2.y) * m_scaleVector.y);
            //m_rectData -= delta;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        
        Gizmos.DrawLine(RectData.point1, RectData.point2);
        Gizmos.DrawLine(RectData.point2, RectData.point3);
        Gizmos.DrawLine(RectData.point3, RectData.point4);
        Gizmos.DrawLine(RectData.point4, RectData.point1);
    }
}
