using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "ScriptableObjects/Settings/LevelSettings")]
public class LevelSettings : ScriptableObject
{
    [SerializeField] private List<StartingObstacle> m_startingObstacles;
    public List<StartingObstacle> StartingObstacles => m_startingObstacles;
}

[Serializable]
public class StartingObstacle
{
    [SerializeField] private BallSettings m_ballData;
    [SerializeField] private Vector3 m_startPosition;
    [SerializeField] private int m_startDirection;
    
    public BallSettings BallData => m_ballData;
    public Vector3 StartPosition => m_startPosition;
    public int StartDirection => m_startDirection;
}