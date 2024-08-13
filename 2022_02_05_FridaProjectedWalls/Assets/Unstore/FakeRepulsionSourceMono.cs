using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeRepulsionSourceMono : MonoBehaviour
{
    public CapsuleCollider m_radiusToUse;
    public Transform m_centerRoot;

    public void GetCenterRoot(out Transform root) => root =m_centerRoot;
    public void GetEffectRadius(out float radius) => radius = m_radiusToUse.radius;

    public void GetWorldPosition(out Vector3 toRepulsePos)
    {
        toRepulsePos = m_centerRoot.position;
    }

    public float GetEffectRadius()
    {
        return m_radiusToUse.radius;
    }
    public void SetRadius(float radius) {
        m_radiusToUse.radius = radius;
    }
}
