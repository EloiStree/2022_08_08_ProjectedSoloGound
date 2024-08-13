using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayFilePngToRawImageMono : MonoBehaviour
{

    
    public Eloi.ClassicUnityEvent_Texture2D m_textureFetched;

    public void FileChangeReceived(ExistingFileChangedEvent fileChanged){
        fileChanged.GetFileReference(out Eloi.IMetaAbsolutePathFileGet file);
        Eloi.E_FileAndFolderUtility.ImportTexture(file, out Texture2D texture);
        m_textureFetched.Invoke(texture);
    }
}
