using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Experiment_GenerateMaterialPerTexture : MonoBehaviour
{

    public Material m_materialToCopy;
    public Dictionary<Texture2D, Material> m_textureToMaterial = new Dictionary<Texture2D, Material>();
    public List<Material> test;
    public void GenerateMaterials(List<Texture2D> textures) {
        for (int i = 0; i < textures.Count; i++)
        {
            if (!m_textureToMaterial.ContainsKey(textures[i])) { 
                Material m = new Material(m_materialToCopy);
                m.name = "M_" + textures[i].name;
                m_textureToMaterial.Add(textures[i], m);
                m.mainTexture = textures[i];
            }
        }

        test = m_textureToMaterial.Values.ToList();
    }

    public Material GetMaterialOf(Texture2D t)
    {
        if (m_textureToMaterial.ContainsKey(t))
            return m_textureToMaterial[t];
        else return null;
    }
}
