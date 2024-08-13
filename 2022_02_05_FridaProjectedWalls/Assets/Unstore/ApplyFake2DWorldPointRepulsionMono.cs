using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyFake2DWorldPointRepulsionMono : MonoBehaviour
{

     Vector3[] m_worldPointRepulse;
    //public Experiment_DrawLinePlayZonePointsMono m_tempSource;

    public PushableFake2DRigidBodyObjectMono[] m_toRepulse;
    public int m_worldPointCount;
    public float m_affectDistance = 0.1f;
    public float m_percentAffect=0.2f;
    public int m_maxWorldPointToFetch = 20;


    public void SetWorldPointToUse(Vector3 [] worldPoint) {
        m_worldPointRepulse = worldPoint; 
    }

    public void SetPixelAffectDistanceMeter(float distanceInMeter)
    {
        m_affectDistance = distanceInMeter;
    }
    public void SetMaxPixelToAffectPerFrame(int maxPixelToAffect)
    {
        m_maxWorldPointToFetch = maxPixelToAffect;
    }
    public void SetPowerPerPixel(float percentAffect) {
        m_percentAffect = percentAffect;
    }
    public void SetActiveRepulsion( bool isActive) {
        m_useUpdate = isActive;
    }
    [ContextMenu("Refresh with in screne")]
    public void SetWithPushableInScene()
    {
        Eloi.E_SearchInSceneUtility.TryToFetchWithActiveInScene<PushableFake2DRigidBodyObjectMono>(ref m_toRepulse);
    }
    public Eloi.DeltaGameTime m_delta;
    public bool m_useUpdate;
    private void Update()
    {
        if (m_useUpdate)
            ApplyForceSinceLastTime();
    }
    public bool m_useDebugDraw;
    public int m_drawPointsCount = 30;
    public Vector3[] randomPointToDraw;
    public void ApplyForceSinceLastTime()
    {

        if(m_useDebugDraw)
        Eloi.E_UnityRandomUtility.GetRandomOfOrLess(out randomPointToDraw, in m_drawPointsCount, m_worldPointRepulse);
        if (randomPointToDraw.Length > 1)
        {
            Eloi.E_DrawingUtility.DrawLines(0.3f, Color.red, randomPointToDraw);
        }



        int maxCount = m_maxWorldPointToFetch;
        m_delta.SetTimeWithNow(out float delta);
       // m_tempSource.GetClampNumberOfRandomWorldPoints(m_maxWorldPointToFetch, out Vector3 [] worldPoints);
        m_worldPointCount = m_worldPointRepulse.Length;
        Vector3[] randomPoint;
        Eloi.E_UnityRandomUtility.GetRandomOfOrLess(out randomPoint, in maxCount, m_worldPointRepulse);
      
        float distance = 0;
        Vector3[] repPosition = new Vector3[m_toRepulse.Length];
        for (int i = 0; i < m_toRepulse.Length; i++)
        {
            m_toRepulse[i].GetWorldPosition(out repPosition[i]);
        }
        //float pct = 0;
        float f = m_percentAffect * delta;
        for (int j = 0; j < randomPoint.Length; j++)
        {
            for (int i = 0; i < m_toRepulse.Length; i++)
            {
                randomPoint[j].y = repPosition[i].y;
                distance = Distance(in randomPoint[j], in repPosition[i]);
                if (distance < m_affectDistance)
                {
                   // pct = Mathf.Clamp01(1f - distance / m_affectDistance);
                    m_toRepulse[i].AddPushEffect((repPosition[i] - randomPoint[j]).normalized, f);
                    
                }
            }
        }
    }

    public Vector3 m_diff = new Vector3();
    public float Distance(in Vector3 a, in Vector3 b)
    {
        m_diff.x = a.x - b.x;
        m_diff.y = a.y - b.y;
        m_diff.z = a.z - b.z;
        return Distance(in m_diff);
    }
    public float Distance(in Vector3 diff)
    {
        return (float)( Math.Sqrt(
      Math.Pow(diff.x, 2f) +
      Math.Pow(diff.y, 2f) +
      Math.Pow(diff.z, 2f)) );
    }
}

