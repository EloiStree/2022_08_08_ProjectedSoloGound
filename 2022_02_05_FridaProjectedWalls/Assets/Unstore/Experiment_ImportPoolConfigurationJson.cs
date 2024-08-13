using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experiment_ImportPoolConfigurationJson : JsonDataPathStorageMono<PoolConfiguration>
{}

public class JsonDataPathStorageMono <T>: MonoBehaviour {
    public Eloi.AbstractMetaAbsolutePathFileMono m_configurationPath;
    public T m_defautlValueIfNotExisting;
    public T m_importedValue;

    public void GetImported(out T imported ) { imported = m_importedValue; }

    [ContextMenu("Import")]
    public void Import()
    {
        try
        {
            Eloi.E_FileAndFolderUtility.ImportOrCreateThenImport(out string json, m_configurationPath, GetDefaultText);
            m_importedValue = JsonUtility.FromJson<T>(json);
        }
        catch (Exception)
        {
            Debug.Log("Fail importing. Create default", this.gameObject);
            m_importedValue = m_defautlValueIfNotExisting;
        }
    }

    public void ExportByOverride(T value)
    {
        Eloi.E_FileAndFolderUtility.ExportByOverriding(m_configurationPath, JsonUtility.ToJson(m_defautlValueIfNotExisting,true));
    }

    private void GetDefaultText(out string textToUse)
    {
        textToUse = JsonUtility.ToJson(m_defautlValueIfNotExisting,true);
    }
}

[System.Serializable]
public class PoolConfiguration {
    public List<PoolCategoryOfElementsInfo> m_categorySetup= new List<PoolCategoryOfElementsInfo>();
}

[System.Serializable]
public class PoolCategoryOfElementsInfo {
    public bool m_use = true;
    public string m_folderCategoryName = "Default";
    public int m_numberToGenerate=400;
    public float m_minSizeMm = 100;
    public float m_maxSizeMm = 400;
    public float m_minSpeedMm = 10;
    public float m_maxSpeedMm = 100;
    public float m_negativeRotationDegree = -90;
    public float m_posifiveRotationDegree = 90;
    public int m_emitAtStart = 2;
    public float m_emitPerSeconds = 2;
    public int m_gridX = 2;
    public int m_gridY = 2;
    public float m_lifeTimeSecond = 4;
    public float m_lifeTimeShorten = 1;
}