using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeParticuleTextChangeMono : MonoBehaviour
{
    public ParticleSystemRenderer m_particule;
    public Material m_toUseMaterial;
    public Texture2D m_toUseTexture2D;
    public Texture2D[] m_randomTexture;
    [Header("Debug")]
    public Material m_created;
    void Awake()
    {
        Eloi.E_UnityRandomUtility.GetRandomOf(out m_toUseTexture2D ,  in m_randomTexture );
        m_created = new Material(m_toUseMaterial);
        m_created.mainTexture = m_toUseTexture2D;
        m_particule.material = m_created;

    }
    public void SetTextureToUse(Texture2D texture) {
        m_toUseTexture2D = texture;

    }

}
