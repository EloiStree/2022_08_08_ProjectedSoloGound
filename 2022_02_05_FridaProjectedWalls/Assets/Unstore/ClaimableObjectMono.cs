using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IClaimableItem {

    public bool IsClaimable();
    public bool IsAutoHandlingUnclaim();
    public void Claim();
    public void Unclaim();
    public void ForceUnclaim();

}

public  class ClaimableObjectMono : MonoBehaviour, IClaimableItem
{
    public bool m_isClaimed;
    public UnityEvent m_claimed;
    public UnityEvent m_naturalUnclaimed;
    public UnityEvent m_beforeForcedUnclaim;
    public bool m_isUnclaimedManagedByDeveloper;

    [ContextMenu("Claim")]
    public void Claim()
    {
        m_claimed.Invoke();
    }

    [ContextMenu("ForceUnclaim")]
    public void ForceUnclaim()
    {
        m_beforeForcedUnclaim.Invoke();
    }

    public bool IsAutoHandlingUnclaim()
    {
        return m_isUnclaimedManagedByDeveloper;
    }

    public bool IsClaimable()
    {
        return m_isClaimed;
    }

    [ContextMenu("Unclaim")]
    public void Unclaim()
    {
        m_naturalUnclaimed.Invoke();
    }
}



