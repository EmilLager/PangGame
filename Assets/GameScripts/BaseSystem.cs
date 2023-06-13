using UnityEngine;

/// <summary>
/// Base class of ScriptableObject based system
/// </summary>
public abstract class BaseScriptableObjectSystem : ScriptableObject
{
    /// <summary>
    /// Used for own initialization
    /// </summary>
    public abstract void Init();
    
    /// <summary>
    /// Used for initializing connections to other systems
    /// </summary>
    public abstract void StartSystem();
}
