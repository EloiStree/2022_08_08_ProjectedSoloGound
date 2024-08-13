using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Eloi;
public class UITogglePlayerPrefs : AbstrectUIPlayerPrefs
{
    public Toggle  m_toggle;
    protected override void Reset()
    {
        base.Reset();
        m_toggle = GetComponent<Toggle>();
    }

    public override void GetInfoToStoreAsString(out string infoToStore)
    {
        infoToStore =""+ m_toggle.isOn;
    }

    public override void SetWithStoredInfoFromString(string recoveredInfo)
    {
        if (bool.TryParse(recoveredInfo, out bool value)) {
            m_toggle.isOn = !value;
            m_toggle.isOn = value;
        }
    }
}
