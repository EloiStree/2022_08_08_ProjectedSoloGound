using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eloi;
using System;

public class TakeKinectTextureScreenshotMono : MonoBehaviour
{
    public FetchKinectPointFromQuadToTexture2DMono m_source;
    public Eloi.AbstractMetaAbsolutePathDirectoryMono m_whereToStore;
    [ContextMenu("TakeScreenhot")]
    public void TakeScreenshot()
    {
        m_source.GetCurrentTexture(out RenderTexture texture);
        E_Texture2DUtility.RenderTextureToTexture2D(in texture, out Texture2D screen);
        Eloi.E_DateTime.GetFileFormatOfNow(out string date);
        MetaFileNameWithExtension file = new MetaFileNameWithExtension(date, "png");
        IMetaAbsolutePathFileGet aFile =E_FileAndFolderUtility.Combine(m_whereToStore, file);
        E_FileAndFolderUtility.OverrideFilePNG(aFile, screen, out bool saved);
    }

  
}
