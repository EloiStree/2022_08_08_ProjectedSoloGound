using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fake2DSquareZoneMono : MonoBehaviour
{
    public Transform m_squareZone;

    public void GetSquareZoneCenter(out Transform rootSize)
    {
        rootSize = m_squareZone;
    }

    public bool IsOutOfBound(in Vector3 toRepulsePos)
    {
        Vector3 worldPos = m_squareZone.position;
        if (toRepulsePos.x < worldPos.x - m_squareZone.localScale.x * 0.5f)
            return true;
        if (toRepulsePos.x > worldPos.x + m_squareZone.localScale.x * 0.5f)
            return true;
        if (toRepulsePos.z < worldPos.z - m_squareZone.localScale.z * 0.5f)
            return true;
        if (toRepulsePos.z > worldPos.z + m_squareZone.localScale.z * 0.5f)
            return true;
        return false;

    }
    public void GetDirectionToRecenter(in Vector3 toRepulsePos, out Vector3 direction)
    {
        direction = new Vector3();
        Vector3 worldPos = m_squareZone.position;
        if (toRepulsePos.x < worldPos.x - m_squareZone.localScale.x * 0.5f)
            direction.x += 1f;
        if (toRepulsePos.x > worldPos.x + m_squareZone.localScale.x * 0.5f)
            direction.x -= 1f;
        if (toRepulsePos.z < worldPos.z - m_squareZone.localScale.z * 0.5f)
            direction.z += 1f;
        if (toRepulsePos.z > worldPos.z + m_squareZone.localScale.z * 0.5f)
            direction.z -= 1f;
    }

    internal Vector3 GetWorldCenterPosition()
    {
        GetWorldCenterPosition(out Vector3 r);
        return r;
    }

    public float GetMaxCornerRadious()
    {
        return Mathf.Sqrt((m_squareZone.localScale.x * m_squareZone.localScale.x) + (m_squareZone.localScale.z * m_squareZone.localScale.z)) * 0.5f;
    }

    public void GetWorldCenterPosition(out Vector3 sourcePosition)
    {
        sourcePosition = m_squareZone.position;
    }
    //   public void GetCenterDirection(in Vector3 source, out Vector3 direction) { 
    //     roo
    // }


    public void GetCorners(out Vector3[] cornersClockWise)
    {

        cornersClockWise = new Vector3[] {
        new Vector3(m_squareZone.localScale.x * 0.5f,0, m_squareZone.localScale.y * 0.5f),
        new Vector3(m_squareZone.localScale.x * 0.5f, 0,- m_squareZone.localScale.y * 0.5f),
        new Vector3(-m_squareZone.localScale.x * 0.5f, 0,- m_squareZone.localScale.y * 0.5f),
        new Vector3(-m_squareZone.localScale.x * 0.5f, 0, m_squareZone.localScale.y * 0.5f)
    };
    }


    public void Reset()
    {
        m_squareZone = transform;
    }
}
