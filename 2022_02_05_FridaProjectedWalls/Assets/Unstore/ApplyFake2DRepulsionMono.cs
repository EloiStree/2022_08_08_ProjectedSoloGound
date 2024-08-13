using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyFake2DRepulsionMono : MonoBehaviour
{
    public void SetAsActive(bool value) {
        m_isActive = value;
    }
    public bool m_isActive; 
    public List<FakeRepulsionSourceMono> m_sourceOfRepulsion;

    public PushableFake2DRigidBodyObjectMono[] m_toRepulse;


    public AnimationCurve m_amplifyCurve;
    public float m_amplifyPourcent=3f;

    [ContextMenu("Refresh with childrens")]
    public void SetWithPushableInScene() {

         Eloi.E_SearchInSceneUtility.TryToFetchWithActiveInScene<PushableFake2DRigidBodyObjectMono>(ref m_toRepulse);
    }

    public Eloi.DeltaGameTime m_delta;
    public bool m_useUpdate;
    private void Update()
    {
        if (m_useUpdate)
            ApplyForceSinceLastTime();
    }
    public void ApplyForceSinceLastTime()
    {
        if (!m_isActive)
            return;
        m_delta.SetTimeWithNow(out float detla);
        for (int i = 0; i < m_toRepulse.Length; i++)
        {
            for (int j = 0; j < m_sourceOfRepulsion.Count; j++)
            {

                m_toRepulse[i].GetWorldPosition(out Vector3 toRepulsePos);
                m_sourceOfRepulsion[j].GetWorldPosition(out Vector3 sourcePosition);

                float distance = Vector3.Distance(toRepulsePos, sourcePosition);
                if(distance< m_sourceOfRepulsion[j].GetEffectRadius())
                {
                    float percentClose = ( 1f - (distance / m_sourceOfRepulsion[j].GetEffectRadius()));
                    float intensityPercent = m_amplifyCurve.Evaluate(percentClose)* m_amplifyPourcent;

                    Vector3 direction = (toRepulsePos - sourcePosition).normalized;
                    m_toRepulse[i].AddPushEffect(direction, intensityPercent * detla);
                    bool isClockWise = (direction.z > 0f && direction.x > 0f) || (direction.z < 0f && direction.x < 0f);
                    m_toRepulse[i].AddTorque(isClockWise , intensityPercent * detla);


                }
            }

        }
    }
}
