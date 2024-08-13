using Eloi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LicenseLockConfigurationImport : JsonFileDefaultOrImportAccess<LicenseLockConfiguration>
{
    [ContextMenu("Import")]
    public void ImportLicense()
    {
        base.Import();
    }


    [ContextMenu("Reset to Zero")]
    public void ResetToDefaultLicense()
    {
        base.ResetToDefault();
    }

    public void OverrideCurrentLicense(LicenseLockConfiguration config)
    {
        base.OverrideFile( config);
    }
}



[System.Serializable]
public class LicenseLockConfiguration {

    public bool m_useLicenseEndingAt;
    public SeriaDateEnd m_licenseFinishAtDate;

    public bool m_useLicenseRangeFromToDate;
    public SeriaDateRange m_licenseFromToDate;

    public bool m_useLicensePerExpiration;
    public double m_licenseExpirationDayFromFirstLaunchInDay = 7;

    public bool m_useLicensePerUseTime;
    public double m_licenseExpirationFromUseTimeInDay = 0.4;

    //public SeriaDateRange [] m_licenseSpreadFromToDate;

}