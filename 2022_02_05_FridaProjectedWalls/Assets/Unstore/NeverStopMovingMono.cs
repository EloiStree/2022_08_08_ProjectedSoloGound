using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeverStopMovingMono : MonoBehaviour
{

    public Rigidbody m_target;
    public PushableFake2DRigidBodyObjectMono m_toAffect;
    public float m_minVelocity=0.04f;
    public Vector3 m_previous;
    public Vector3 m_current;
    public Vector3 m_direction;
    public void SetMinVelocity(float minVelocity) {
        m_minVelocity = minVelocity;
    }
    public float m_pourcentPush=0.3f;

    public void Refresh()
    {
        m_current = transform.position;
        m_direction = m_current - m_previous;

        if (m_target.velocity.magnitude <m_minVelocity)
        {
            m_toAffect.AddPushEffect(m_direction.normalized, m_pourcentPush);
        }

        if (m_previous != m_current)
            m_previous = m_current;
    }
    private void Reset()
    {
        m_target = GetComponent<Rigidbody>();
        m_toAffect = GetComponent < PushableFake2DRigidBodyObjectMono>();
    }
}
