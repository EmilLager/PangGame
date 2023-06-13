using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// This Scriptable object works as a registry for different parts of the project
/// Given its API a component of the system will have to know about a different component to access it through this object
/// This removes the need for direct references between different systems or singleton type solutions
/// And also allows easy connections between components of all types
/// (I would generally use something more sophisticated which would be a bit overkill for this project) 
/// </summary>
[CreateAssetMenu(fileName = "SystemLocator", menuName = "ScriptableObjects/SystemLocator")]
public class SystemLocator : ScriptableObject
{
    [SerializeField] private List<BaseScriptableObjectSystem> m_systemReferences;

    private static Dictionary<Type, Object> m_serviceDict;
    
    public void Init(MonoSystem monoSystem)
    {
        m_serviceDict = new Dictionary<Type, Object>();
        
        m_serviceDict.Add(monoSystem.GetType(), monoSystem);
        
        foreach (BaseScriptableObjectSystem t in m_systemReferences)
        {
            m_serviceDict.Add(t.GetType(), t);
            t.Init();
        }
    }

    public void StartServices()
    {
        foreach (BaseScriptableObjectSystem t in m_systemReferences)
        {
            t.StartSystem();
        }
    }

    //Note: could also use indexer for these to make API nicer
    public static T Get<T>() where T : Object
    {
        return (T) m_serviceDict[typeof(T)];
    }
    
    public static void Add<T>(T system) where T : Object
    {
        m_serviceDict.Add(typeof(T), system);
    }
    
    public static void Remove<T>(T system) where T : Object
    {
        m_serviceDict.Remove(typeof(T));
    }
}
