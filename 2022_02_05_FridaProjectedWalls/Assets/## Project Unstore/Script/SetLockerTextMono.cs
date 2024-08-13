using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLockerTextMono : JsonDataPathStorageMono<LockerTextDisplay>
{
    public Eloi.PrimitiveUnityEvent_String m_displayWhenCheckingReadOnly;
    public Eloi.PrimitiveUnityEvent_String m_displayWhenCheckingLicense;

    public Eloi.PrimitiveUnityEvent_String m_expireAtSpecificDate;
    public Eloi.PrimitiveUnityEvent_String m_expireFromToSpecificDate;
    public Eloi.PrimitiveUnityEvent_String m_someReadOnlyWereModified;

    public Eloi.PrimitiveUnityEvent_String m_cheatOnLicenseDetected;
    public Eloi.PrimitiveUnityEvent_String m_cheatOnReadOnlyDetected;
    public Eloi.PrimitiveUnityEvent_Float m_displayTimeAllowed;


    [ContextMenu("Refresh")]
    public void Refresh() {
        Import();
        GetImported(out LockerTextDisplay toDisplay);

        m_displayTimeAllowed.Invoke(toDisplay.m_timeAllowedToCheckDisplay);

        m_displayWhenCheckingReadOnly.Invoke(toDisplay.m_displayWhenCheckingReadOnly);
        m_displayWhenCheckingLicense.Invoke(toDisplay.m_displayWhenCheckingLicense);
        m_expireAtSpecificDate.Invoke(toDisplay.m_expiredAtSpecificDate);
        m_expireFromToSpecificDate.Invoke(toDisplay.m_expiredFromToSpecificDate);
        m_someReadOnlyWereModified.Invoke(toDisplay.m_someReadOnlyWereModify);

        m_cheatOnLicenseDetected.Invoke(toDisplay.m_cheatOnLicenseDetected);
        m_cheatOnReadOnlyDetected.Invoke(toDisplay.m_cheatOnReadOnlyFilesDetected);

    }

}

    [System.Serializable]
    public class LockerTextDisplay {

    public float m_timeAllowedToCheckDisplay=0.1f;

    [TextArea(0, 10)]
    public string m_displayWhenCheckingReadOnly;

    [TextArea(0, 10)]
    public string m_displayWhenCheckingLicense;


    [TextArea(0, 10)]
    public string m_expiredAtSpecificDate;
    [TextArea(0, 10)]
    public string m_expiredFromToSpecificDate;
    [TextArea(0, 10)]
    public string m_someReadOnlyWereModify;
    [TextArea(0, 10)]
    public string m_cheatOnLicenseDetected;
    [TextArea(0,10)]
    public string m_cheatOnReadOnlyFilesDetected;


}
