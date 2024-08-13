using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AllowToChooseExportPlayZoneType : MonoBehaviour
{
    public bool m_useDiskFileExport=false;
    public bool m_useMemoryFileExport = true;
    public bool m_useNetworkExport = false;
    public ExportTextureAsFileMono m_fileExport;
    public MemoryFileConnectionMono m_memoryExport;
    public Eloi.ClassicUnityEvent_Texture2D m_networkExportManager;
    public UnityEvent m_exportEvent;

    public void AllowFileExport(bool value) => m_useDiskFileExport = value;
    public void AllowMemoryExport(bool value) => m_useMemoryFileExport = value;
    public void AllowNetworkExport(bool value) => m_useNetworkExport = value;

    public void PushTexture(Texture2D texture)
    {
        if (m_useDiskFileExport)
            m_fileExport.PushTexture(texture);
        if (m_useMemoryFileExport)
            m_memoryExport.SetAsTexture(texture);
        if (m_useNetworkExport)
            m_networkExportManager.Invoke(texture);
        m_exportEvent.Invoke();
    }
}
