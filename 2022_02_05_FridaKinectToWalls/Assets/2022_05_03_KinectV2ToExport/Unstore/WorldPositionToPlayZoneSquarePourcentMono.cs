using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPositionToPlayZoneSquarePourcentMono : MonoBehaviour
{

    public PlayZoneWorldSpaceDebugMono m_localPlayZone;
    public bool m_useDebugDraw;
    public void GetPercent(in Transform point, in  bool inverseHorizontal, out float horizontalPercent, out float verticalPercent)
    {
        GetPercent( point.position,in inverseHorizontal, out horizontalPercent, out verticalPercent);
    }

    private Vector3 vBin;
    private Vector3 vZero = Vector3.zero;
    public void GetPercent(in Vector3 point, in bool inverseHorizontal,  out float horizontalPercent, out float verticalPercent)
    {
        //Eloi.E_CodeTag.DirtyCode.Info("This code is unperfect but I don't have time to make it good enought");
        m_localPlayZone.WorldToLocal(in point, out vBin, out Vector3 local);
        //Eloi.E_DrawingUtility.DrawLines(Time.deltaTime, Color.red,
        //    m_localPlayZone.m_dlLocalWorld, localWorld);
        //Eloi.E_DrawingUtility.DrawLines(Time.deltaTime, Color.red,
        //   Vector3.zero, local);
        // MEASURE LEFT BORDER
        verticalPercent = local.z / m_localPlayZone.m_tlLocal.magnitude;

        if (inverseHorizontal) { 
            //MEASURE DOWN BORDER
            horizontalPercent = ComputePourcentOfSegment( in m_localPlayZone.m_drLocal, in vZero, in local);
        }
        else horizontalPercent = ComputePourcentOfSegment(in vZero, in m_localPlayZone.m_drLocal, in local);
    }

    private float ComputePourcentOfSegment(in Vector3 from,in  Vector3 to,
       in Vector3 localPointToTrack)
    {
        Vector3 relocateSegDirection = to - from;
        if (relocateSegDirection.magnitude == 0)
            return 0;
        Quaternion q = Quaternion.LookRotation(relocateSegDirection);
        Quaternion relocatingRotation = Quaternion.Inverse(q);
        Vector3 relocatedPoint = relocatingRotation * (localPointToTrack - from);
        if (m_useDebugDraw) { 
         Eloi.E_DrawingUtility.DrawLines(Time.deltaTime, Color.yellow,
           Vector3.zero, relocatedPoint);
        }
        return relocatedPoint.z / relocateSegDirection.magnitude;
    }
}
