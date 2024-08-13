using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eloi;
using System.IO;
using UnityEngine.Events;

public class PirateTag {

    public static bool m_isUserPirateSoLetHimBe;
    public static bool IsUserPirate() { return m_isUserPirateSoLetHimBe; }

}
public class PirateLicenseSkipperMono : MonoBehaviour
{
    public AbstractMetaAbsolutePathFileMono m_fileToBePirate;
    public TextAsset m_textKeyToBePirate;
    public UnityEvent m_userNotifyThatHeDontCareOfLicenses;
    public  bool m_userNotifyHeIsPirate;

    [ContextMenu("Check if use is pirate")]
    public void CheckIfUserSaidHeIsPirateAndSetProjectInfo() {
        if (DoesUserNotifyThatHisIsPirate()) {
            m_userNotifyThatHeDontCareOfLicenses.Invoke();
            PirateTag.m_isUserPirateSoLetHimBe = true;
            m_userNotifyHeIsPirate = true;
        }
    }
    public bool DoesUserNotifyThatHisIsPirate() {
        string path = GetPiratePassPath();
        if (File.Exists(path)){
            string text = File.ReadAllText(path);
            return Eloi.E_StringUtility.AreEquals(m_textKeyToBePirate.text, text, true, true);
        }
        return false;
    }

    [ContextMenu("Create pirate key")]
    public void CreatePirateKey() {
        Eloi.E_FileAndFolderUtility.CreateFile(m_fileToBePirate, m_textKeyToBePirate.text);
    }

    public string  GetPiratePassPath() {
        return m_fileToBePirate.GetPath();
    }
}
