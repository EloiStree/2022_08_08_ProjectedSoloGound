using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackWhiteTextureRepulsionQuadMono : MonoBehaviour
{
    public Transform m_zoneTransform=null;
    public RenderTexture m_textureToUse=null;
    void Awake()
    {
        CreateIfNull(256, 256);
    }

    public void SetTextureToUse(RenderTexture textureToUse)
    {
        
        m_textureToUse = textureToUse;
        m_textureToUse.enableRandomWrite = true;
        Graphics.SetRandomWriteTarget(0, m_textureToUse);
        //Eloi.E_Texture2DUtility.RenderTextureToTexture2D(in textureToUse, out Texture2D t);
        
    }

    private void CreateIfNull(int widht, int height)
    {

        if (m_textureToUse == null || 
            m_textureToUse.width != widht ||
            m_textureToUse.height != height) {
            if (m_textureToUse != null)
                m_textureToUse.Release();
            m_textureToUse = new RenderTexture(widht, height, 0);
            m_textureToUse.enableRandomWrite = true;
            Graphics.SetRandomWriteTarget(0, m_textureToUse);
        }


    }

    public void SetTextureToUse(Texture2D textureToUse) {
        CreateIfNull(textureToUse.width, textureToUse.height);
        Eloi.E_Texture2DUtility.Texture2DInRenderTexture(in textureToUse, ref m_textureToUse);
    }

    public void GetTextureUsed(out Texture texture) { texture= m_textureToUse ; }
    public Texture GetTextureUsed() { return m_textureToUse; }

    public void GetZoneToUse(out Transform transform) { transform = m_zoneTransform; }
    public Transform GetZoneToUse() { return m_zoneTransform; }


    private void Reset()
    {
        m_zoneTransform = transform;
    }

    public int GetTextureWidth()
    {
       return  m_textureToUse.width;
    }

    public int GetTextureHeight()
    {
        return m_textureToUse.height;
    }

}
