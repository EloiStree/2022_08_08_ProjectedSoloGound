using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker_SHA256AreEqualsMono : MonoBehaviour
{
    [TextArea(0, 5)]
    public string m_lastTextGiven;

    [TextArea(0, 5)]
    public string m_lastTextAsSHA256;


    [TextArea(0,5)]
    public string m_lastSHA256Given;

    public Locker_SHA256AreEqualsEvent m_hashToBeValide;


    public void PushEventWithText(string textToCheck)
    {
        m_lastTextGiven = textToCheck;
        m_lastTextAsSHA256= TextToSHA256.sha256_UTF8(textToCheck);
        m_hashToBeValide.PushEventWithText(textToCheck);
    }

    public void SetLockerWithHash256(string hash256)
    {
        m_hashToBeValide.SetLockerWithSHA256(hash256);
        m_lastSHA256Given = m_hashToBeValide.m_hashToBeValide.m_lockerSHA256;
    }
    public void SetLockerWithText(string text)
    {
        m_hashToBeValide.SetLockerWithSHA256FromText(text);
        m_lastSHA256Given = m_hashToBeValide.m_hashToBeValide.m_lockerSHA256;
    }


    [ContextMenu("Push last text")]
    public void PushLastText()
    {
        SetLockerWithHash256(m_lastSHA256Given);
        PushEventWithText(m_lastTextGiven);
    }
}

[System.Serializable]
public class Locker_SHA256AreEqualsEvent
{
    public Locker_SHA256AreEquals m_hashToBeValide;
    public Eloi.PrimitiveUnityEventExtra_Bool m_isValideDateEvent;

    public void SetLockerWithSHA256(string hash256)
    {
        m_hashToBeValide.SetLockerWithSHA256(hash256);
    }
    public void SetLockerWithSHA256FromText(string text)
    {
        m_hashToBeValide.SetLockerWithText(text);
    }
    public void PushEventWithText(string textToCheck) {
        bool isvalide = m_hashToBeValide.IsTextValideUnicode(textToCheck);
        m_isValideDateEvent.Invoke(isvalide);
    }
}
[System.Serializable]
public class Locker_SHA256AreEquals {
    public string m_lockerSHA256;
    public bool IsTextValideUnicode(string text)
    {
       string given= TextToSHA256.sha256_UTF8(text);
       return Eloi.E_StringUtility.AreEquals(given, m_lockerSHA256, false, true);
    }
    public void SetLockerWithText(string text)
    {
        m_lockerSHA256= TextToSHA256.sha256_UTF8(text);
    }
    public void SetLockerWithSHA256(string sha256)
    {
        m_lockerSHA256= sha256;
    }
}
