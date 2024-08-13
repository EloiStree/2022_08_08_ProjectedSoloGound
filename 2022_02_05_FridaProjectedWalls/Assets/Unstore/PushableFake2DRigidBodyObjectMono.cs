using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableFake2DRigidBodyObjectMono : MonoBehaviour
{

    public Transform m_rootUpDirection;
    [SerializeField] Rigidbody m_rigidBodyToAffect;
    public ForceMode m_forceMode = ForceMode.Acceleration;
    public float m_forceAtMax=4;

    public ForceMode m_forceModeRotation=ForceMode.Acceleration;
    public float m_torqueRotationAtMax=3;
    //public float m_forceAtMin=1;
    public void GetWorldPosition(out Vector3 toRepulsePos)
    {
        toRepulsePos = m_rootUpDirection.position;
    }

    public Vector3 m_stackDirection;
    public Vector3 m_stackDirectionImp;
    public float  m_stackTorque;
    public bool m_hasChanged;
    public void ApplyStacks() {
        if (m_hasChanged) { 
            if (m_stackDirection.magnitude > 0f)
            {
                m_rigidBodyToAffect.AddForce(m_stackDirection, m_forceMode);
                m_stackDirection = Vector3.zero;
            }
            if (m_stackDirectionImp.magnitude > 0f)
            {
                m_rigidBodyToAffect.AddForce(m_stackDirectionImp, ForceMode.Impulse);
                m_stackDirectionImp = Vector3.zero;
            }
            if (m_stackTorque != 0f) { 
                m_rigidBodyToAffect.AddTorque(new Vector3(0, m_stackTorque, 0), m_forceMode);
                m_stackTorque = 0;
            }
            m_hasChanged = false;
        }


    }
    public void AddPushEffect(Vector3 worldDirection, float percentContinusIntensity)
    {
        worldDirection.y = 0;
        m_stackDirection += worldDirection * m_forceAtMax * percentContinusIntensity;
        m_hasChanged = true;
    }
    public void AddTorque(bool clockWise, float pourcent)
    {
        m_stackTorque += pourcent * m_torqueRotationAtMax * (clockWise ? 1f : -1f);
        m_hasChanged = true;

    }
    public void AddPushImpulse(Vector3 worldDirection, float forceIntesity=1)
    {
        worldDirection.y = 0;
        m_stackDirectionImp += worldDirection * forceIntesity;
        m_hasChanged = true;
    }

    public void NeutralizedVelocity()
    {
        m_rigidBodyToAffect.velocity = Vector3.zero;
        m_rigidBodyToAffect.angularVelocity = Vector3.zero;
        m_stackDirection = Vector3.zero;
        m_stackDirectionImp = Vector3.zero;
        m_stackTorque=0;
        m_hasChanged = true;
    }

    public void AddRandomImpluseForce(float impulseForce) {

        Eloi.E_UnityRandomUtility.GetRandomVector3Direction(out Vector3 random);
        AddPushImpulse(random, impulseForce);
        m_hasChanged = true;
    }
}
