using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experiment_ExtendLicenseValidityFor30Days : MonoBehaviour
{
    public LicenseLockConfigurationImport m_licenseImported;
    public Locker_ExpriationDateEventMono m_expirationDayFromStart;

    
    [ContextMenu("ExtendOf30Days")]
    public void ExtendOf30Days() => ExtendLicenseForNDay(30);
    [ContextMenu("ExtendOf7Days")]
    public void ExtendOf7Days() => ExtendLicenseForNDay(7);
    [ContextMenu("ExtendOf1Days")]
    public void ExtendOf1Days() => ExtendLicenseForNDay(1);
    [ContextMenu("ExtendOf1Hours")]
    public void ExtendOf1Hours() => ExtendOfNHours(1);
    [ContextMenu("ExtendOf1Minutes")]
    public void ExtendOf1Minutes() => ExtendOfNMinutes(1);
    [ContextMenu("ExtendOf5Minutes")]
    public void ExtendOf5Minutes() => ExtendOfNMinutes(5);
    [ContextMenu("ExtendOf15Minutes")]
    public void ExtendOf15Minutes() => ExtendOfNMinutes(15);


    public void ExtendOfNDays(double day) => ExtendLicenseForNDay(day);
    public void ExtendOfNHours(double nHours) => ExtendLicenseForNDay((1.0 / 24.0) *nHours);
    public void ExtendOfNMinutes(double nMinute) => ExtendLicenseForNDay((1.0 / ((24.0 * 60.0)) * nMinute));
    public void ExtendOfNSeconds(double nSeconds) => ExtendLicenseForNDay((1.0 / ((24.0 * 3600.0)) * nSeconds));

    public void ExtendLicenseForNDay(double numberOfDays)
    {
        if(m_expirationDayFromStart)
        m_expirationDayFromStart.ResetStartLicenseToNow();
        DateTime dt = DateTime.Now;
        DateTime dtInN = dt.AddDays(numberOfDays);

       LicenseLockConfiguration imported=  m_licenseImported.m_imported;
        imported.m_licenseFromToDate.m_valideFrom.SetWithDate(dt);
        imported.m_licenseFromToDate.m_valideTo.SetWithDate(dtInN);
        imported.m_licenseFinishAtDate.m_finishDate.SetWithDate(dtInN);
        imported.m_licenseExpirationFromUseTimeInDay = numberOfDays;

        if (m_expirationDayFromStart)
            m_expirationDayFromStart.m_dayStartTracker.ResetToZeroTimeTrackersAndSetToNow();

        m_licenseImported.m_imported = imported;
        m_licenseImported.OverrideCurrentLicense(imported);
        m_licenseImported.Import();

    }
}
