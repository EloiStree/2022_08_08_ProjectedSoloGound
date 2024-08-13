using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LifeTimeMono : MonoBehaviour
{
    public float m_lifeTimeStart=30;
    public float m_currentLifeTime=30;
    public UnityEvent m_onBorn;
    public UnityEvent m_onDeath;
    public AbstractDestroyManagerMono m_autoDestroy;
    // Start is called before the first frame update
    void Start()
    {
        SetAsBorn();
    }

    public void SetAsBorn()
    {
        m_currentLifeTime = m_lifeTimeStart;
        m_onBorn.Invoke();
    }   
    public void Kill()
    {
        m_currentLifeTime = 0;
        m_onDeath.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_currentLifeTime > 0) { 
            m_currentLifeTime -= Time.deltaTime;
            if (m_currentLifeTime<0) {
                m_currentLifeTime = 0;
                m_onDeath.Invoke();
            }
        }
    }

    [ContextMenu("Add Destroyer")]
    public void AddDestoryer() {
        AbstractDestroyManagerMono autoDestroy =
            this.gameObject.GetComponent<AbstractDestroyManagerMono>();
        if (autoDestroy == null)
            autoDestroy = this.gameObject.AddComponent<DefaultDestroyManagerMono>();
    }

   
}
