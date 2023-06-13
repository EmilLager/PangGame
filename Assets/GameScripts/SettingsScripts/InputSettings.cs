using System;
using UnityEngine;

[CreateAssetMenu(fileName = "InputSettings", menuName = "ScriptableObjects/Settings/InputSettings")]
public class InputSettings : ScriptableObject
{
    [SerializeField] private PlayerControls m_player1Controls;
    [SerializeField] private PlayerControls m_player2Controls;
    
    [SerializeField] private KeyCode m_pauseKey;
    [SerializeField] private KeyCode m_addPlayerKey;
    
    public PlayerControls Player1Controls => m_player1Controls;
    public PlayerControls Player2Controls => m_player2Controls;
    public KeyCode PauseKey => m_pauseKey;
    public KeyCode AddPlayerKey => m_addPlayerKey;
    
}

[Serializable]
public class PlayerControls //Having a class for this allows easier management when changing settings and in code
{
    [SerializeField] private KeyCode m_moveLeftKey;
    [SerializeField] private KeyCode m_moveRightKey;
    [SerializeField] private KeyCode m_fireKey;
    
    public KeyCode MoveLeftKey => m_moveLeftKey;
    public KeyCode MoveRightKey => m_moveRightKey;
    public KeyCode FireKey => m_fireKey;
}
