using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This system allows us to withold on using Unity physics
/// It's possible to implement relatively simply given we only need to detect sphere to rect collisions (And rect to rect for more advanced features)
/// </summary>
[CreateAssetMenu(fileName = "CollisionSystem", menuName = "ScriptableObjects/Settings/CollisionSystem")]
public class CollisionSystem : BaseScriptableObjectSystem
{

    private List<BallObstacle> m_spheres;
    private List<RectObstacle> m_rectObstacles;
    
    public override void Init()
    {
        m_spheres = new List<BallObstacle>();
        m_rectObstacles = new List<RectObstacle>();
    }

    public override void StartSystem()
    {
        SystemLocator.Get<MonoSystem>().OnUpdate.AddListener(CheckCollisions);
    }

    public void AddBallObstacle(BallObstacle ballObstacle)
    {
        m_spheres.Add(ballObstacle);
    }
    
    public void RemoveBallObstacle(BallObstacle ballObstacle)
    {
        m_spheres.Remove(ballObstacle);
    }
    
    public void AddRectObstacle(RectObstacle rectObstacle)
    {
        m_rectObstacles.Add(rectObstacle);
    }
    
    public void RemoveRectObstacle(RectObstacle rectObstacle)
    {
        m_rectObstacles.Remove(rectObstacle);
    }

    /// <summary>
    /// Check all collisions between relevant obstacle objects
    /// </summary>
    /// <param name="delta"></param>
    private void CheckCollisions(float delta)
    {
        //This does require o(n*m) time but is still more performant than the naive solution, could be further optimized
        RectObstacle rectHit = null;
        foreach (RectObstacle rect in m_rectObstacles)
        {
            BallObstacle ballHit = null;
            foreach (BallObstacle sphere in m_spheres)
            {
                if (SphereToRectangleIntersection(sphere.Position, sphere.Radius, rect.RectData))
                {
                    rectHit = rect;
                    ballHit = sphere;
                    break;
                }
            }

            if (ballHit)
            {
                switch (rect.CollisionType)
                {
                    case CollisionObjectType.Player:
                        break;
                    case CollisionObjectType.Wall:
                        ballHit.HitWall();
                        break;
                    case CollisionObjectType.Floor:
                        ballHit.HitFloor();
                        break;
                    case CollisionObjectType.Attack:
                        SystemLocator.Get<LevelManagementSystem>().AddScore(ballHit.Score);
                        ballHit.Break();
                        break;
                }
            }
        }
        if (rectHit)
        {
            switch (rectHit.CollisionType)
            {
                case CollisionObjectType.Attack:
                    rectHit.TakeHit();
                    break;
                case CollisionObjectType.Player:
                    SystemLocator.Get<LevelManagementSystem>().GameOver?.Invoke();
                    break;
                case CollisionObjectType.Wall:
                case CollisionObjectType.Floor:
                    break;
            }
        }
    }

    /// <summary>
    /// Calculate if a sphere is intersecting with a rectangle
    /// General math solution that works for the cases needed here
    /// This solution isn't accurate with corner detection but is totally serviceable for what I need here
    /// Given more time I'd make it detect corners properly too 
    /// </summary>
    public static bool SphereToRectangleIntersection(Vector2 sphereCenter, float sphereRadius, RectData rect)
    {
        Vector2 rectCenter = rect.rectCenter;
        Vector2 rectWidthVector = rect.point2 - rect.point1;
        Vector2 rectHeightVector = rect.point3 - rect.point2;
        
        float rectWidth = rectWidthVector.magnitude;
        float rectHeight = rectHeightVector.magnitude;
        
        Vector2 rectToSphere = sphereCenter - rectCenter;
        
        float sphereDistanceOnWidth = Vector3.Project(rectToSphere, rectWidthVector).magnitude - (rectWidth / 2) - sphereRadius;
        float sphereDistanceOnHeight = Vector3.Project(rectToSphere, rectHeightVector).magnitude - (rectHeight / 2) - sphereRadius;

        //Debug.Log($"sphereDistanceOnHeight is {sphereDistanceOnHeight} sphereDistanceOnWidth is {sphereDistanceOnWidth} test {(sphereDistanceOnHeight * sphereDistanceOnHeight) + (sphereDistanceOnWidth * sphereDistanceOnWidth)}");

        bool test = (sphereDistanceOnHeight * sphereDistanceOnHeight + sphereDistanceOnWidth * sphereDistanceOnWidth) > (sphereRadius * sphereRadius);
        //bool test = (sphereDistanceOnHeight + sphereDistanceOnWidth) * (sphereDistanceOnHeight + sphereDistanceOnWidth) > (sphereRadius * sphereRadius);
        if (sphereDistanceOnHeight < 0f && sphereDistanceOnWidth < 0f && (test))
        {
            return true;
        }

        return false;
    }
}

[Serializable]
public class RectData
{
    public Vector2 point1;
    public Vector2 point2;
    public Vector2 point3;
    public Vector2 point4;

    public Vector2 rectCenter => (point1 + point3) / 2f;

    public RectData(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
    {
        point1 = A;
        point2 = B;
        point3 = C;
        point4 = D;
    }
    
    public static RectData operator +(RectData rect, Vector2 offset)
        => new RectData(rect.point1 + offset, rect.point2 + offset, rect.point3 + offset, rect.point4 + offset);
    
    public static RectData operator -(RectData rect, Vector2 offset)
        => new RectData(rect.point1 - offset, rect.point2 - offset, rect.point3 - offset, rect.point4 - offset);
}

public enum CollisionObjectType
{
    Floor,
    Player,
    Wall,
    Attack,
}
