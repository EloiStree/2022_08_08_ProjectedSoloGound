using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class ManagerLicenseCreateAtFirstRunLaunchMono : MonoBehaviour
{
    public Eloi.SerializableDateTime m_startOfTheLicenseDate;
    public Eloi.AbstractMetaAbsolutePathFileMono m_licenseStartAsFileSave;
    public Eloi.AbstractMetaAbsolutePathFileMono m_licenseCheatTagAsFileSave;
    public string m_playerPrefsKey = "LicenseStartDate";

    [Header("Debug")]
    public double m_totalDaysSince;
    public double m_totalHoursSince;
    public double m_totalSecondsSince;

    public DateTime GetDate()
    {
        return m_startOfTheLicenseDate.GetAsDate();
    }

    public string m_licensePlayerPrefs;
    public string m_licensePermaData;

    public UnityEvent m_someoneCouldTryToCheat;
    bool m_infoFetched;
    void Awake()
    {
        FetchInfoIfNotDoneYet();
    }
    public void FetchInfoIfNotDoneYet()
    {
        if (!m_infoFetched)
            Load();
    }
    [ContextMenu("Reload info")]
    public void Load()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString(m_playerPrefsKey)))
        {
            SaveToNow();
        }
        m_licensePlayerPrefs = PlayerPrefs.GetString(m_playerPrefsKey);

        if (File.Exists(GetPathOfLicenseDateFile()))
            m_licensePermaData = File.ReadAllText(GetPathOfLicenseDateFile());
        else m_licensePermaData = "";

        if (Eloi.E_StringUtility.AreNotEquals(m_licensePlayerPrefs, m_licensePermaData, true, true))
            m_someoneCouldTryToCheat.Invoke();

        m_startOfTheLicenseDate = JsonUtility.FromJson<Eloi.SerializableDateTime>(m_licensePlayerPrefs);
        m_totalSecondsSince = (DateTime.Now - m_startOfTheLicenseDate.GetAsDate()).TotalSeconds;
        m_totalHoursSince = m_totalSecondsSince / 3600.0;
        m_totalDaysSince = m_totalHoursSince / 24.0;
        m_infoFetched = true;
    }

    public void GetTimeSinceStartToNow( out double totalTimeInSeconds)
    {
        GetTimeSinceStart(DateTime.Now, out totalTimeInSeconds);
    }

    public void GetTimeSinceStart(DateTime now, out double totalTimeInSeconds) {
        totalTimeInSeconds = (now - m_startOfTheLicenseDate.GetAsDate()).TotalSeconds;
    }

    [ContextMenu("Override License date to now")]
    public void SaveToNow()
    {
        m_startOfTheLicenseDate.SetWithDate(DateTime.Now);
        string json = JsonUtility.ToJson(m_startOfTheLicenseDate,true);
        PlayerPrefs.SetString(m_playerPrefsKey, json);
        Eloi.E_FileAndFolderUtility.ExportByOverriding(m_licenseStartAsFileSave, json);
        m_infoFetched = false;

        FetchInfoIfNotDoneYet();
    }
    public string GetPathOfLicenseDateFile()
    {
        return m_licenseStartAsFileSave.GetPath();
    }
    public string GetTagDataFile()
    {
        return m_licenseCheatTagAsFileSave.GetPath();
    }
 

    [ContextMenu("Tag as cheating")]
    public void TagComputerAsMaybeCheating()
    {
        Eloi.E_FileAndFolderUtility.AppendTextAtStart(m_licenseCheatTagAsFileSave,DateTime.Now.ToString() + "\n");
    }


    [ContextMenu("Open License file")]
    public void OpenLicenseFolder()
    {
        Application.OpenURL(m_licenseStartAsFileSave.GetPath());
    }
    [ContextMenu("Open Cheat Tag file")]
    public void OpenCheatTagFolder()
    {
        Application.OpenURL(m_licenseCheatTagAsFileSave.GetPath());
    }

    [ContextMenu("Reset to zero")]
    public void ResetToZeroTimeTrackersAndSetToNow()
    {

        if (File.Exists(GetPathOfLicenseDateFile()))
            File.Delete(GetPathOfLicenseDateFile());
        PlayerPrefs.DeleteKey(m_playerPrefsKey);
        PlayerPrefs.SetString(m_playerPrefsKey, "");
        Load();

    }
}
