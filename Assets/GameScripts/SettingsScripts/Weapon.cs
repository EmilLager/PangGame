using UnityEngine;

/// <summary>
/// Base class for different kinds of weapons
/// </summary>
public abstract class Weapon : ScriptableObject
{
    [SerializeField] private float m_fireTime;

    public float FireTime => m_fireTime;

    public abstract void Init();
    public abstract bool Fire(Vector3 firePosition);
}
