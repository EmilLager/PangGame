using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSettings", menuName = "ScriptableObjects/Settings/BallSettings")]
public class BallSettings : ScriptableObject
{
    [SerializeField] private List<SizeTier> m_sizeTiers;
    public List<SizeTier> SizeTiers => m_sizeTiers;
}

[Serializable]
public class SizeTier
{
    [SerializeField] private int m_score;
    [SerializeField] private float m_radius;
    [SerializeField] private float m_horizontalSpeed;
    [SerializeField] private float m_maxVerticalSpeed;
    [SerializeField] private float m_startVerticalSpeed;
    [SerializeField] private float m_verticalAcceleration;
    [SerializeField] private Sprite m_sprite;
    [SerializeField] private Color m_spriteColor;

    public int Score => m_score;
    public float Radius => m_radius;
    public float HorizontalSpeed => m_horizontalSpeed;
    public float MaxVerticalSpeed => m_maxVerticalSpeed;
    public float StartVerticalSpeed => m_startVerticalSpeed;
    public float VerticalAcceleration => m_verticalAcceleration;
    public Sprite Sprite => m_sprite;
    public Color SpriteColor => m_spriteColor;
}
