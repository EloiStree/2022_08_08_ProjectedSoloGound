using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileChange_ImportTextureMono : MonoBehaviour
{
    public Eloi.ClassicUnityEvent_Texture2D m_textureFetched;
    public Texture2D m_lastImported;
    public void FileChangeReceived(ExistingFileChangedEvent fileChanged)
    {
        fileChanged.GetFileReference(out Eloi.IMetaAbsolutePathFileGet file);
        Eloi.E_FileAndFolderUtility.ImportTexture(file, out Texture2D texture);
        m_lastImported = texture;
        m_textureFetched.Invoke(texture);
    }
}
