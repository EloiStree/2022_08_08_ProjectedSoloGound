using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyFake2DKeepInsideSquareZoneMono : MonoBehaviour
{
    public Fake2DSquareZoneMono m_outOfBoundPush;
    public PushableFake2DRigidBodyObjectMono[] m_toRepulse;

    public float m_intensityPadding = 5;

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
        m_outOfBoundPush.GetWorldCenterPosition(out Vector3 sourcePosition);
        float squareRadius = m_outOfBoundPush.GetMaxCornerRadious();
        for (int i = 0; i < m_toRepulse.Length; i++)
        {
            m_toRepulse[i].GetWorldPosition(out Vector3 toRepulsePos);
            if (m_outOfBoundPush.IsOutOfBound(in toRepulsePos)) {

                float distance = Vector3.Distance(toRepulsePos, sourcePosition);

                if (m_intensityPadding == 0f)
                    m_intensityPadding = 0.0001f;
                    float percentClose =0.5f+ Mathf.Clamp01((distance-squareRadius) / m_intensityPadding);

                    if (distance > squareRadius+m_intensityPadding)
                        m_toRepulse[i].NeutralizedVelocity();
                    //Vector3 direction = (sourcePosition- toRepulsePos ).normalized;
                    m_outOfBoundPush.GetDirectionToRecenter(in toRepulsePos, out Vector3 direction);
                    m_toRepulse[i].AddPushEffect(direction, (percentClose) * delta);
                    bool isClockWise = (direction.z > 0f && direction.x > 0f) || (direction.z < 0f && direction.x < 0f);
                    m_toRepulse[i].AddTorque(isClockWise, percentClose * delta);

                 
            }

        }


    }
    [ContextMenu("Refresh with childrens")]
    public void SetWithPushableInScene()
    {

        Eloi.E_SearchInSceneUtility.TryToFetchWithActiveInScene<PushableFake2DRigidBodyObjectMono>(ref m_toRepulse);
    }
}