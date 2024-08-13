using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experiment_SetLockerWithConfiguration : MonoBehaviour
{

    public LicenseLockConfiguration m_setValue;

    public Locker_DateTimeFromToLockerEventMono m_fromToLocker;
    public Eloi.PrimitiveUnityEvent_Bool m_isLockerRangeDateUse;

    public Locker_DateTimeToLockerEventMono m_toLocker;
    public Eloi.PrimitiveUnityEvent_Bool m_isLockerMaxDateUse;

    public Locker_ExpriationDateEventMono m_expirationDate;
    public Eloi.PrimitiveUnityEvent_Bool m_isLockerExpirationUse;
    


    public void SetLicenseLock(LicenseLockConfiguration setValue)
    {
        m_setValue = setValue;
        ApplyGivenValue();

    }

    [ContextMenu("Apply value")]
    private void ApplyGivenValue()
    {
        m_isLockerRangeDateUse.Invoke(m_setValue.m_useLicenseRangeFromToDate);
        m_isLockerMaxDateUse.Invoke(m_setValue.m_useLicenseEndingAt);

        m_isLockerExpirationUse.Invoke(m_setValue.m_useLicensePerExpiration);


        float days = (float)m_setValue.m_licenseExpirationDayFromFirstLaunchInDay;

        if (m_expirationDate){ 
        m_expirationDate.m_authorizedDay = days;
        m_expirationDate.LoadExprimationDate();
        if(m_setValue.m_useLicensePerExpiration)
            m_expirationDate.PushEventWithDateNow();
        }


        m_toLocker.SetValideDateTo(m_setValue.m_licenseFinishAtDate.m_finishDate.GetAsDate());
        if (m_setValue.m_useLicenseEndingAt)
            m_toLocker.PushEventWithDateNow();

        m_fromToLocker.SetValideDate(
            m_setValue.m_licenseFromToDate.m_valideFrom.GetAsDate(),
            m_setValue.m_licenseFromToDate.m_valideTo.GetAsDate());
        if (m_setValue.m_useLicenseRangeFromToDate)
            m_fromToLocker.PushEventWithDateNow();


      }
}
