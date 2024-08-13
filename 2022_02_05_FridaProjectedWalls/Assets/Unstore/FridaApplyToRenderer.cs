using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridaApplyToRenderer : MonoBehaviour
{
    public Renderer m_renderer;
    public Texture2DToRendererMono m_applyTexture;

    public void SetMaterial(Material material) {
        if(m_renderer)
        m_renderer.material = material;
    }
    public void SetTexture(Texture2D texture) {
        if (m_applyTexture != null)
            m_applyTexture.PushTexture2D(texture);
    }
}
public class FridaApplyMatToParticule : MonoBehaviour {
    public ParticleSystemRenderer m_renderer;

    public void SetMaterial(Material material)
    {
        if (m_renderer)
            m_renderer.material = material;
    }
    //public void SetTexture(Texture2D texture)
    //{
    //    if (m_applyTexture != null)
    //        m_applyTexture.PushTexture2D(texture);
    //}
}