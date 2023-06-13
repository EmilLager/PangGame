using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This system allows better control over gameflow and improves performance by reducing the amount of Unity Update method calls
/// </summary>
public class MonoSystem : MonoBehaviour
{
    [SerializeField] private UpdateEvent m_onUpdate;
    [SerializeField] private float m_timeScale = 1f;
    
    public UpdateEvent OnUpdate => m_onUpdate;
    public float TimeScale => m_timeScale;

    private void Update()
    {
        m_onUpdate?.Invoke(Time.deltaTime * m_timeScale);
    }

    public void SetTimeScale(float timeScale)
    {
        m_timeScale = timeScale;
    }

    private void OnDestroy()
    {
        SystemLocator.Remove(this);
    }
}

[Serializable]
public class UpdateEvent : UnityEvent<float> {}