using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyFake2DTryToKeepInScreenMono : MonoBehaviour
{

    public Fake2DSquareZoneMono[] m_screenZones;
    public PushableFake2DRigidBodyObjectMono[] m_toRepulse;

    public float m_magnitureAsIntencity = 1;
    public Eloi.DeltaGameTime m_delta;
    public bool m_useUpdate;
    private void Update()
    {
        if (m_useUpdate)
            ApplyForceSinceLastTime();
    }
    public void ApplyForceSinceLastTime()
    {
        m_delta.SetTimeWithNow(out float delta);
        for (int i = 0; i < m_toRepulse.Length; i++)
        {
            m_toRepulse[i].GetWorldPosition(out Vector3 toRepulsePos);
            bool isInOneScreen = false;
            for (int j = 0; j < m_screenZones.Length; j++)
            {
                if (!m_screenZones[j].IsOutOfBound(in toRepulsePos))
                {
                    isInOneScreen = true;
                    break;
                }
            }

            if (!isInOneScreen)
            {
                GetClosestScreenBorder(in toRepulsePos,out Fake2DSquareZoneMono found, out Vector3 closePointFound);
                GetRandomPointInScreen(in found, out Vector3 randomPoint);

                //Vector3 direction = (((found.GetWorldCenterPosition() - toRepulsePos) + (closePointFound - toRepulsePos)) * 0.5f).normalized;
                Vector3 direction = (randomPoint - toRepulsePos).normalized;

                if (m_magnitureAsIntencity == 0f)
                    m_magnitureAsIntencity = 0.0001f;
                float percentClose = 0.5f + direction.magnitude / m_magnitureAsIntencity;
                m_toRepulse[i].AddPushEffect(direction, (percentClose) * delta);
                bool isClockWise = (direction.z > 0f && direction.x > 0f) || (direction.z < 0f && direction.x < 0f);
                m_toRepulse[i].AddTorque(isClockWise, percentClose * delta);
            }

        }


    }

    private void GetRandomPointInScreen(in Fake2DSquareZoneMono found, out Vector3 randomPoint)
    {
        Eloi.E_UnityRandomUtility.GetRandomPositionInTransform(in found.m_squareZone, out randomPoint);
    }

    [ContextMenu("Refresh with childrens")]
    public void SetWithPushableInScene()
    {

        Eloi.E_SearchInSceneUtility.TryToFetchWithActiveInScene<PushableFake2DRigidBodyObjectMono>(ref m_toRepulse);
    }
    private void GetClosestScreenBorder(in Vector3 point, out Fake2DSquareZoneMono found, out Vector3 closePointFound)
    {

        found = m_screenZones[0];
        closePointFound = m_screenZones[0].GetWorldCenterPosition();

        float closeDistance = float.MaxValue;
        for (int i = 0; i < m_screenZones.Length; i++)
        {
            m_screenZones[i].GetCorners(out Vector3[] corners);
            for (int j = 0; j < corners.Length; j++)
            {
                float distance = Vector3.Distance(point, corners[j]);
                if (distance<closeDistance)
                {
                    closeDistance = distance;
                    found = m_screenZones[i];
                    closePointFound = corners[j];
                }
            }

        }



    }
}
