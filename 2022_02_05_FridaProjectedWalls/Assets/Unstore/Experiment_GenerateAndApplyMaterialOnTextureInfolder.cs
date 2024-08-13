using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experiment_GenerateAndApplyMaterialOnTextureInfolder : MonoBehaviour
{
    public int m_elementTogenerateCount = 1500;
    public DefaultQuickGeneratePrefabMono m_elementGenerator;
    public ImportImagePerGroupMono m_imageGroup;
    public Experiment_GenerateMaterialPerTexture m_materialPerTexture;

    public void SetElementToGenerateCount(int elementCount) {
        m_elementTogenerateCount = elementCount;
    }

    [ContextMenu("Test")]
    public void Test()
    {
        m_elementGenerator.Generate(m_elementTogenerateCount, true);
        Transform root = m_elementGenerator.m_where;
        m_imageGroup.GetAllTextures(out List<Texture2D> textures);
        m_materialPerTexture.GenerateMaterials(textures);

        FridaApplyToRenderer [] r= root.GetComponentsInChildren<FridaApplyToRenderer>();
        foreach (var item in r)
        {
            Eloi.E_UnityRandomUtility.GetRandomOf(out Texture2D t, textures);
            Material m  =  m_materialPerTexture.GetMaterialOf(t);
            item.SetMaterial(m);
        }

    }
}
