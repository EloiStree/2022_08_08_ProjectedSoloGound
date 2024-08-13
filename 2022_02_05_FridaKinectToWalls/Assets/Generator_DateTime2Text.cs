using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator_DateTime2Text : MonoBehaviour
{
    public Eloi.PrimitiveUnityEvent_String m_dateAsStringEvent;
    public string m_dateFormat = "yyyy-mm-dd hh:mm";

    [ContextMenu("Push date now")]
    public void PushDateTimeNow()
    {
        PushDateTime(DateTime.Now);

    }
    public void PushDateTime(DateTime date)
    {
        m_dateAsStringEvent.Invoke(date.ToString(m_dateFormat));
    }
}
