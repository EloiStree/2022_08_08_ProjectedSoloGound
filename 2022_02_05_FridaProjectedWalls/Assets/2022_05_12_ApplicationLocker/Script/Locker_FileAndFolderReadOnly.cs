using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Eloi;
using System;
using System.IO;
using System.Linq;
using UnityEngine.Events;

public class Locker_FileAndFolderReadOnly : MonoBehaviour
{
    [Header("Set")]
    public Eloi.PrimitiveUnityEventExtra_Bool m_isValidePush;
    public FilesAndDirectoryLockInfo m_unityTrackInfo;
    public SearchOption m_searchFolderOption;

    public string m_playerPerfsKey = "LockerDefault";

    [Header("Debug")]
    public FilesAndDirectoryLock m_current;
    public FilesAndDirectoryLock m_lockedInfo;
    public bool m_isLockerSaved;

    public UnityEvent m_possibleCheatingDetected;

  
    private void Awake()
    {
        if (HasLocker())
            LoadLockerFromMemory();
        else 
            SetLockerWithCurrentFilesAndFolder();
    }

    public bool IsLockerIsCorrupted()
    {
        E_FileAndFolderUtility.IsContentAreNotEquals(m_readOnlyFileStorageLocker, m_readOnlyFileStorageLockerDouble, out bool areNotEquals);
        return areNotEquals;
    }
    public void CheckAndPushIfLockerAreCorrupted()
    {
        if (IsLockerIsCorrupted())
            m_possibleCheatingDetected.Invoke();
    }


    [ContextMenu("Save current locker locker in memory")]
    public void SaveGivenLockerInMemory()
    {
        SaveGivenLockerInMemory(m_lockedInfo);
    }
    public Eloi.AbstractMetaAbsolutePathFileMono m_readOnlyFileStorageLocker;
    public Eloi.AbstractMetaAbsolutePathFileMono m_readOnlyFileStorageLockerDouble;
    public Eloi.AbstractMetaAbsolutePathFileMono m_readOnlyFileStorageLastImportedImported;

    public void SaveGivenLockerInMemory(FilesAndDirectoryLock locker) {
        string json = JsonUtility.ToJson(locker, true);
        Eloi.E_FileAndFolderUtility.ExportByOverriding(m_readOnlyFileStorageLocker, json);
        Eloi.E_FileAndFolderUtility.ExportByOverriding(m_readOnlyFileStorageLockerDouble, json);
    }

    public bool HasLocker()
    {
        //m_isLockerSaved = PlayerPrefs.HasKey(m_playerPerfsKey)
        //    && !string.IsNullOrEmpty(PlayerPrefs.GetString(m_playerPerfsKey).Trim());
        //return m_isLockerSaved;
        bool fileExist = E_FileAndFolderUtility.Exists(m_readOnlyFileStorageLocker);
        if (!fileExist)
            return false;
        E_FileAndFolderUtility.ImportFileAsText(m_readOnlyFileStorageLocker, out string text);
        m_isLockerSaved =  !string.IsNullOrEmpty(text.Trim());
        return m_isLockerSaved;
    }

    [ContextMenu("Test Load from memory")]
    public void LoadLockerFromMemory() {
        LoadLockerInMemory(out bool found, out m_lockedInfo);
    }
    public string m_debugPlayerPrefSaved;
    public void LoadLockerInMemory(out bool found, out FilesAndDirectoryLock lockerToAffect)
    {
        try {
           
            E_FileAndFolderUtility.ImportFileAsText(m_readOnlyFileStorageLocker, out string text,"");
            //m_debugPlayerPrefSaved = PlayerPrefs.GetString(m_playerPerfsKey);
            m_debugPlayerPrefSaved = text;
            lockerToAffect = JsonUtility.FromJson<FilesAndDirectoryLock>(text);
            found = true;
        }
        catch (Exception e )
        {
            found = false;
            lockerToAffect = new FilesAndDirectoryLock();
            Debug.Log("Did not load json correctly:" + e.StackTrace);

        }
    }
    [ContextMenu("Load files and locker then push")]
    public void LoadFilesAndLockerThenCheckValidity() {
        LoadFilesAndFolderState();
        RestoreLockerFromMemory();
         PushLockerIsValidity();
}


    [ContextMenu("Load files as current")]
    public void LoadFilesAndFolderState()
    {
        GenerateLockStateFromFilesAndFolder(out  m_current);
        Eloi.E_FileAndFolderUtility.ExportByOverriding(m_readOnlyFileStorageLastImportedImported, JsonUtility.ToJson(m_current,true));

    }
    [ContextMenu("Restore Locker From Memory")]
    public void RestoreLockerFromMemory()
    {
        LoadLockerInMemory(out bool found, out m_lockedInfo);
        if (!found)
        {
            SetLockerWithCurrentFilesAndFolder();
        }
    }
    [ContextMenu("Push Locker Is Validity")]
    public void PushLockerIsValidity()
    {
        bool isLockerCorrupted = IsLockerIsCorrupted();
        m_isValidePush.Invoke( !isLockerCorrupted && IsLockerValide());
        if(isLockerCorrupted)
            m_possibleCheatingDetected.Invoke();

    }


    private void GenerateLockStateFromFilesAndFolder(out FilesAndDirectoryLock filesLock)
    {
        IMetaAbsolutePathDirectoryGet root = m_unityTrackInfo.m_root;
 
        filesLock = new FilesAndDirectoryLock();
        List<FileWithSizeContenKey> fList = new List<FileWithSizeContenKey>();

        foreach (var item in m_unityTrackInfo.m_files.m_filesPath)
        {
            if (Eloi.E_FileAndFolderUtility.Exists(item))
            {
                AddFileInGroup(root, item.GetPath(), ref fList);
            }
        }
        filesLock.m_files.m_lockSource.m_filesTracked.SetTo(fList.ToArray());
        filesLock.m_files.m_lockSource.GenerateSHA256(out string generated);
        fList = new List<FileWithSizeContenKey>();
        foreach (var item in m_unityTrackInfo.m_directories.m_directectoriesPath)
        {
            if (Eloi.E_FileAndFolderUtility.Exists(item))
            {
                string[] paths = Directory.GetFiles(item.GetPath(), "*", m_searchFolderOption).OrderBy(k=>k).ToArray();
                foreach (string p in paths)
                {
                    root = AddFileInGroup(root, p, ref fList);
                }

            }
        }
        filesLock.m_directories.m_lockSource.m_filesTracked.SetTo(fList.ToArray());
        filesLock.m_directories.m_lockSource.GenerateSHA256(out generated);
    }

    private static IMetaAbsolutePathDirectoryGet AddFileInGroup(
        IMetaAbsolutePathDirectoryGet root, 
        string path ,
        ref List<FileWithSizeContenKey> fList)
    {
        MetaAbsolutePathFile temp;
        Eloi.E_CodeTag.ToCodeLater.Info("Allow to have configurable table");
        Eloi.E_CodeTag.ToCodeLater.Info("Check that endswith work properly");
        bool useContent = !(path.ToLower().EndsWith(".jpeg") ||
            path.ToLower().EndsWith(".jpg") ||
            path.ToLower().EndsWith(".png"));

        temp = new MetaAbsolutePathFile(path);
        Eloi.E_FileAndFolderUtility.GetRelativePathFrom(in root, temp, out IMetaRelativePathFileGet fileAsRelative);
        FileWithSizeContenKey fSize = new FileWithSizeContenKey();
        fSize.SetWith(temp, fileAsRelative);
        if (useContent)
            fSize.SetContent(File.ReadAllText(path));
        fList.Add(fSize);
        return root;
    }

    [ContextMenu("Set Locker With Current Files And Folder")]
    public void SetLockerWithCurrentFilesAndFolder() {

        GenerateLockStateFromFilesAndFolder(out m_lockedInfo);
        SaveGivenLockerInMemory( m_lockedInfo);

    }
    public bool IsLockerValide() {
        return FilesAndDirectoryLock.AreLockEquals(in m_current, in m_lockedInfo);
    }



}


[System.Serializable]
public class FilesAndDirectoryLockInfo
{
    public Eloi.AbstractMetaAbsolutePathDirectoryMono m_root;
    public FilesLockUnityInfo m_files;
    public DirectoriesLockUnityInfo m_directories;
}
[System.Serializable]
public class FilesLockUnityInfo
{
    public List<Eloi.AbstractMetaAbsolutePathFileMono> m_filesPath;
}
[System.Serializable]
public class DirectoriesLockUnityInfo
{
    public List<Eloi.AbstractMetaAbsolutePathDirectoryMono> m_directectoriesPath;
}

[System.Serializable]
public class FilesAndDirectoryLock
{
    public FilesLock m_files= new FilesLock();
    public DirectoriesLock m_directories = new DirectoriesLock();

    public void Clear()
    {
        m_files.Clear();
        m_directories.Clear();
    }

    public static bool AreLockEquals(in FilesAndDirectoryLock a, in FilesAndDirectoryLock b) {

        return DirectoriesLock.AreLockEquals(in a.m_directories, in b.m_directories)
        && FilesLock.AreLockEquals(in a.m_files, in b.m_files);
    }
}
[System.Serializable]
public class FilesLock
{
    public GroupOfFileWithSizeSHA256 m_lockSource= new GroupOfFileWithSizeSHA256();

    public static bool AreLockEquals(in FilesLock a, in FilesLock b)
    {
        return GroupOfFileWithSizeSHA256.AreLockEquals(in a.m_lockSource, in b.m_lockSource);
    }

    public void Clear()
    {
        m_lockSource.Clear();
    }
}
[System.Serializable]
public class DirectoriesLock
{
    public GroupOfFileWithSizeSHA256 m_lockSource = new GroupOfFileWithSizeSHA256();

    public static bool AreLockEquals(in DirectoriesLock a, in DirectoriesLock b)
    {
        return GroupOfFileWithSizeSHA256.AreLockEquals(in a.m_lockSource, in b.m_lockSource);
    }
    public void Clear()
    {
        m_lockSource.Clear();
    }
}



[System.Serializable]
public class GroupOfFileWithSizeSHA256
{
    public GroupOfFileWithSize m_filesTracked = new GroupOfFileWithSize();
    public string m_textUsed = "";
    public string m_SHA256 = "";

    public void GenerateSHA256(out string generated) {
        m_textUsed = GetShaableTextFromFiles();
        m_SHA256= TextToSHA256.sha256_ASCII(m_textUsed);
        generated = m_SHA256;
    }
    public string GetShaableTextFromFiles() {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < m_filesTracked.m_files.Count; i++)
        {
            sb.AppendLine(m_filesTracked.m_files[i].m_relativePath + "_" + m_filesTracked.m_files[i].m_fileSize);
            if (m_filesTracked.m_files[i].GetContent().Length > 0 ) {
                sb.AppendLine(m_filesTracked.m_files[i].GetContent());
            }
        }
        return sb.ToString();
    }

    public void Clear()
    {
        m_filesTracked.Clear();
        m_textUsed = "";
        m_SHA256 = "";
    }


    public static bool AreLockEquals(in GroupOfFileWithSizeSHA256 a,in GroupOfFileWithSizeSHA256 b)
    {
        return Eloi.E_StringUtility.AreEquals(in a.m_SHA256, in b.m_SHA256);
    }
}

    [System.Serializable]
public class GroupOfFileWithSize
{
    public List<FileWithSizeContenKey> m_files = new List<FileWithSizeContenKey>();
    public void SetTo(FileWithSizeContenKey file)
    {
        m_files.Clear();
        m_files.Add(file);
    }
    public void SetTo(FileWithSizeContenKey [] file)
    {
        m_files.Clear();
        m_files.AddRange(file);
    }

    public void Add(params FileWithSizeContenKey[] file)
    {
        m_files.AddRange(file);
    }
    public void Clear() {
        m_files.Clear();
    }
}


[System.Serializable]
public class FileWithSizeContenKey {
    public string m_relativePath="";
    public long m_fileSize=0;
    [TextArea(0, 2)]
    [SerializeField]
    private string m_content="";
    public FileWithSizeContenKey()
    {
        m_relativePath = "";
        m_content = "";
        m_fileSize = 0;
    }
    public void SetWith(IMetaAbsolutePathFileGet directPath, IMetaRelativePathFileGet computedRelativePath) {
        m_relativePath = computedRelativePath.GetPath();
        m_fileSize = (new FileInfo(directPath.GetPath())).Length;
    }

    public void SetContentToUseInKey(string content) {
        m_content = content ;
    }

    public string GetContent()
    {
        E_StringByte64Utility.Base64DecodeUnicode(m_content, out string  text);
        return text;
    }

    public void SetContent(string content)
    {
        E_StringByte64Utility.Base64EncodeUsingUnicode(content, out m_content);
    }
}