using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocatorInitializer : MonoBehaviour
{
    [SerializeField] private SystemLocator m_systemLocator;
    [SerializeField] private MonoSystem m_monoSystem;

    private void Awake()
    {
        m_systemLocator.Init(m_monoSystem);
    }
    
    private void Start()
    {
        m_systemLocator.StartServices();
    }
}
