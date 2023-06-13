using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSettings", menuName = "ScriptableObjects/Settings/CharacterSettings")]
public class CharacterSettings : ScriptableObject
{
    [SerializeField] private float m_movementSpeed;
    
    public float MovementSpeed => m_movementSpeed;
}
