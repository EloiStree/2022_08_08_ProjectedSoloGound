//public class Locker_ExpriationDateEventMono : MonoBehaviour
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker_ExpriationDateEventMono
    : MonoBehaviour
{
    public ManagerLicenseCreateAtFirstRunLaunchMono m_dayStartTracker;
    public Locker_DateTimeFromToLockerEvent m_params;
    public float m_authorizedDay = 7;


    [Range(0,1)]
    public double m_percentTimeLeft;


    [ContextMenu("Load Expriation Dates")]
    public void LoadExprimationDate()
    {
        m_dayStartTracker.FetchInfoIfNotDoneYet();
        DateTime from = m_dayStartTracker.GetDate();
        DateTime to = from.AddDays(m_authorizedDay);
        m_params.SetValideDateFrom(from);
        m_params.SetValideDateTo(to);
        GetTimeLeftInPercent(out m_percentTimeLeft);
    }
   
    public void GetTimeLeftInSeconds(out double timeLeftInSeconds, out double totalTimeInSeconds)
    {

        m_params.GetTotalTimeInSeconds(out totalTimeInSeconds);
        m_dayStartTracker.GetTimeSinceStartToNow(out timeLeftInSeconds);
        // m_dayStartTracker.get

    }
    public void GetTimeLeftInPercent(out double percent)
    {
        GetTimeLeftInSeconds(out double time, out double totalTime);
        percent = time / totalTime;
    }

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

    [ContextMenu("Reset Start Licnse To now")]
    public void ResetStartLicenseToNow()
    {
        m_dayStartTracker.ResetToZeroTimeTrackersAndSetToNow();
        //m_dayStartTracker.SaveToNow();
    }
}

