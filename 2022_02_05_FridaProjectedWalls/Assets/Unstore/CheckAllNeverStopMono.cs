using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAllNeverStopMono : MonoBehaviour
{
    public NeverStopMovingMono[] m_toRepulse;
    [ContextMenu("Refresh with in screne")]
    public void SetWithPushableInScene()
    {
        Eloi.E_SearchInSceneUtility.TryToFetchWithActiveInScene<NeverStopMovingMono>(ref m_toRepulse);
    }

    public void PushObjectWithNeverStop()
    {
        if (m_toRepulse != null)
            for (int i = 0; i < m_toRepulse.Length; i++)
            {
                m_toRepulse[i].Refresh();
            }
    }
}
