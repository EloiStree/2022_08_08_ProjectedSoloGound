using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExportJson_KinectHeadsMono : MonoBehaviour
{

    public AbstractGetKinectHeadsAsTransformMono m_accesskinectInfo;
    public Eloi.PrimitiveUnityEvent_String m_jsonToExport;

    [TextArea(0,5)]
    public string m_jsonExported;
    [ContextMenu("Export")]
    public void Export() {

        m_accesskinectInfo.GetCurrentState(out KinectHeads source);
        JsonKinectHeadsUtility.GetAsJson(in source, out  m_jsonExported);
        m_jsonToExport.Invoke(m_jsonExported);
    }
}
