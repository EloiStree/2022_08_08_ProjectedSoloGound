
using System;
using UnityEngine;

public class Locker_DateTimeToLockerEventMono
    : MonoBehaviour
{
    public Locker_DateTimeToLockerEvent m_params;

    public void SetValideDateTo(DateTime to)
    {
        m_params.SetValideDateTo(to);
    }
    [ContextMenu("Push with now date")]
    public void PushEventWithDateNow()
    {
        m_params. PushEventWithDate(DateTime.Now);
    }
    public void PushEventWithDate(DateTime time)
    {
        m_params.PushEventWithDate(time);
    }
}

[System.Serializable]
public class Locker_DateTimeToLockerEvent {
    public SeriaDateEnd m_maxValidateTime;
    public Eloi.PrimitiveUnityEventExtra_Bool m_isValideDateEvent;

    public void SetValideDateTo(DateTime to)
    {
        m_maxValidateTime.m_finishDate.SetWithDate(to);
    }
    [ContextMenu("Push with now date")]
    public void PushEventWithDateNow()
    {
        PushEventWithDate(DateTime.Now);
    }
    public void PushEventWithDate(DateTime time)
    {
        Eloi.E_DateTime.IsBetween(in time, Eloi.E_DateTime.Get1970Date(), m_maxValidateTime.m_finishDate.GetAsDate(), out bool isBetween);
        m_isValideDateEvent.Invoke(isBetween);
    }
}