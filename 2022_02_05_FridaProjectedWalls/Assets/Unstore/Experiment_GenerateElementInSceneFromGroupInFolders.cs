using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experiment_GenerateElementInSceneFromGroupInFolders : MonoBehaviour { 

    public Experiment_ImportPoolConfigurationJson m_poolGroupConfiguration;
    public DefaultQuickGeneratePrefabMono m_elementGenerator;
    public ImportImagePerGroupMono m_imageGroup;
    public Experiment_GenerateMaterialPerTexture m_materialPerTexture;


    [ContextMenu("Import")]
    public void ImportAndGenerateTextureAndConfiguration()
    {
        foreach (PoolCategoryOfElementsInfo item in m_poolGroupConfiguration.m_importedValue.m_categorySetup)
        {
            m_imageGroup.CreateFolderToReceiveImage(item.m_folderCategoryName);
            m_imageGroup.GetTexturesInGroup(item.m_folderCategoryName, out List<Texture2D> textures);
            Transform root = m_elementGenerator.m_where;
            m_materialPerTexture.GenerateMaterials(textures);
            for (int i = 0; i < item.m_numberToGenerate; i++)
            {
                if (textures.Count > 0) {

                   
                   //m_lifeTimeSecond = 4;
                   //m_lifeTimeshorten = 1;

                    Eloi.E_UnityRandomUtility.GetRandomOf(out Texture2D t, textures);
                    Material m = m_materialPerTexture.GetMaterialOf(t);
                    m_elementGenerator.SetMinSize(item.m_minSizeMm / 1000f);
                    m_elementGenerator.SetMaxSize(item.m_maxSizeMm / 1000f);
                    GameObject created = m_elementGenerator.GenerateOneElement();
                    FridaApplyToRenderer f = created.GetComponentInChildren<FridaApplyToRenderer>();
                    if (f)
                        f.SetMaterial(m);
                    FridaApplyMatToParticule fm = created.GetComponentInChildren<FridaApplyMatToParticule>();
                    if (fm)
                        fm.SetMaterial(m);
                    FridaApplyTextureRelay fa = created.GetComponentInChildren<FridaApplyTextureRelay>();
                    if(fa)
                    fa.SetWithTexture(t);
                    ParticuleEmitControlsMono e = created.GetComponentInChildren<ParticuleEmitControlsMono>();
                    if (e)
                    {
                        e.SetLifeTime(item.m_lifeTimeSecond);
                        e.SetGridXY(item.m_gridX, item.m_gridY);
                        e.SetEmitPerSeconds(item.m_emitPerSeconds);
                        e.SetSpeed(item.m_minSpeedMm / 1000f, item.m_maxSpeedMm / 1000f);
                        e.SetRotation(item.m_negativeRotationDegree, item.m_posifiveRotationDegree);
                    }
                    ParticuleControlsAtStartMono es = created.GetComponentInChildren<ParticuleControlsAtStartMono>();
                    if (es)
                    {
                        es.SetEmitAtStart(item.m_emitAtStart);
                    }
                    PoolItemLifeTimeMono d= created.GetComponentInChildren<PoolItemLifeTimeMono>();
                    if (d)
                    {
                        d.m_lifeTime = item.m_lifeTimeSecond;
                        d.m_executionWarningAt = item.m_lifeTimeShorten;
                    }
                    //m_emitAtStart = 2;
                    //m_emitPerSeconds = 2;
                }
            }
        }

        m_elementGenerator.NotifyElementsGenerated();


    }
}
