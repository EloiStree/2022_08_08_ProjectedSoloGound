using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Debug_FourPointsPlayZoneLenght : MonoBehaviour
{
    public Transform m_topLeft;
    public Transform m_topRight;
    public Transform m_downLeft;
    public Transform m_downRight;
    [TextArea(0,2)]
    public string m_dipslayFormat= "{0:0.00}t|{1:0.00}l\n{2:0.00}d | {3:0.00}r\n";
    public Eloi.PrimitiveUnityEvent_String m_dimensionDebug;

    public void RefreshUI() {
        float top, down, right, left;
        top = Vector3.Distance(m_topLeft.position, m_topRight.position);
        down = Vector3.Distance(m_downLeft.position, m_downRight.position);
        right = Vector3.Distance(m_topRight.position, m_downRight.position);
        left = Vector3.Distance(m_topLeft.position, m_downLeft.position);
        m_dimensionDebug.Invoke(string.Format(m_dipslayFormat
            , top, left, down, right));
    }
}
