using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker_DateTimeFromToLockerEventMono
    : MonoBehaviour
{

    public Locker_DateTimeFromToLockerEvent m_params;


    public void SetValideDate(DateTime from, DateTime to)
    {
        SetValideDateFrom(from);
        SetValideDateTo(to);
    }
    public void SetValideDateFrom(DateTime from)
    {
        m_params.SetValideDateFrom(from);
    }
    public void SetValideDateTo(DateTime to)
    {
        m_params.SetValideDateTo(to);
    }
    [ContextMenu("Push with now date")]
    public void PushEventWithDateNow()
    {
        m_params.PushEventWithDateNow();
    }
    public void PushEventWithDate(DateTime time)
    {
        m_params.PushEventWithDate(time);
    }
}


[System.Serializable]
public class Locker_DateTimeFromToLockerEvent
{
    public SeriaDateRange m_valideDate;
    public Eloi.PrimitiveUnityEventExtra_Bool m_isValideDateEvent;


    public void SetValideDate(DateTime from, DateTime to)
    {
        SetValideDateFrom(from);
        SetValideDateTo(to);
    }
    public void SetValideDateFrom(DateTime from)
    {
        m_valideDate.m_valideFrom.SetWithDate(from);
    }
    public void SetValideDateTo(DateTime to)
    {
        m_valideDate.m_valideTo.SetWithDate(to);
    }
    public void PushEventWithDateNow()
    {
        PushEventWithDate(DateTime.Now);
    }
    public void PushEventWithDate(DateTime time)
    {
        Eloi.E_DateTime.IsBetween(in time,
            m_valideDate.m_valideFrom.GetAsDate(), 
            m_valideDate.m_valideTo.GetAsDate(), out bool isBetween);
        m_isValideDateEvent.Invoke(isBetween);
    }

    internal void GetTotalTimeInSeconds(out double totalTimeInSeconds)
    {
        DateTime from=m_valideDate.m_valideFrom.GetAsDate(), to= m_valideDate.m_valideTo.GetAsDate();
        totalTimeInSeconds = (to - from).TotalSeconds;
    }
}


[System.Serializable]
public class SeriaDateRange
{
    public Eloi.SerializableDateTime m_valideFrom = new Eloi.SerializableDateTime(DateTime.Now);
    public Eloi.SerializableDateTime m_valideTo = new Eloi.SerializableDateTime(DateTime.Now);
}

[System.Serializable]
public class SeriaDateEnd
{
    public Eloi.SerializableDateTime m_finishDate = new Eloi.SerializableDateTime(DateTime.Now);
}
