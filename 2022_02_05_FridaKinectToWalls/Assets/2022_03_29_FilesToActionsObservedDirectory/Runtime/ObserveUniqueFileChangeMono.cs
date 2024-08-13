using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DefaultDirectoryFilesObserver;

public class ObserveUniqueFileChangeMono : MonoBehaviour
{

    public Eloi.AbstractMetaAbsolutePathFileMono m_filePath;
    public FileWriteChangedObserver m_observedFile;
    public ExistingFileChangedUnityEvent m_fileChanged;

    public void Start()
    {
        m_observedFile = new FileWriteChangedObserver(m_filePath);
        if (m_observedFile != null && m_observedFile.IsStillExisting() )
        {
            m_observedFile.UpdateStoreObservedDate();
            NotifyChange();
        }
    }

    private void Update()
    {
        if (m_observedFile!=null && m_observedFile.IsStillExisting() &&  m_observedFile.HasChanged()) {
            m_observedFile.UpdateStoreObservedDate();
            NotifyChange();
        }
    }

    private void NotifyChange()
    {
        m_fileChanged.Invoke(new ExistingFileChangedEvent(m_filePath));
    }
}
