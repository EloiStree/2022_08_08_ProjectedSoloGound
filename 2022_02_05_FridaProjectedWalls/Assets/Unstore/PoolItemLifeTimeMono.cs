using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PoolItemLifeTimeMono : MonoBehaviour
{
    public float m_lifeTime = 9;
    public float m_executionWarningAt = 3;
    public float m_timeLeft;
    public UnityEvent m_onAwake;
    public UnityEvent m_setAlive;
    public ExecutionWarningTimeLeft m_onWillBeKilled;
    public UnityEvent m_onKilled;


    [System.Serializable]
    public class ExecutionWarningTimeLeft : UnityEvent<float> { }


    public void Awake()
    {
        m_onAwake.Invoke();
    }

    [ContextMenu("Restart")]
    public void Restart()
    {
        m_timeLeft = m_lifeTime;
        m_setAlive.Invoke();
    }
    [ContextMenu("Stop")]
    public void Stop()
    {
        m_timeLeft = 0;
        Update();
    }

    float m_previousTime;
    public void Update()
    {
        if (m_lifeTime > 0) {
            m_previousTime = m_timeLeft;
            m_timeLeft -= Time.deltaTime;

            if (m_previousTime > m_executionWarningAt && m_timeLeft <= m_executionWarningAt)
            {
                m_onWillBeKilled.Invoke(m_timeLeft);
            }
            if (m_timeLeft <= 0) {
                m_timeLeft = 0;
                m_onKilled.Invoke();
            }
        }
    }

}
