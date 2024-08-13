using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyPendingStackForceMono : MonoBehaviour
{
    public PushableFake2DRigidBodyObjectMono[] m_toRepulse;
    [ContextMenu("Refresh with in screne")]
    public void SetWithPushableInScene()
    {
        Eloi.E_SearchInSceneUtility.TryToFetchWithActiveInScene<PushableFake2DRigidBodyObjectMono>(ref m_toRepulse);
    }

    public void FlushStackForce() {
        if(m_toRepulse!=null )
        for (int i = 0; i < m_toRepulse.Length; i++)
        {
            m_toRepulse[i].ApplyStacks();
        }
    }
}
