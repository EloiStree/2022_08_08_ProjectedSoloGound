using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstratKinectTexturePusherMono : MonoBehaviour
{
    public RenderTexture m_renderer;
    public Eloi.ClassicUnityEvent_RenderTexture m_onRendererChanged;

    public void PushTextureToRenderer(Texture2D texture) {
        if (texture == null)
            return;
        if (m_renderer == null )
        {
            m_renderer = new RenderTexture(texture.width, texture.height, 0);
            m_renderer.enableRandomWrite = true;
        }
        Eloi.E_Texture2DUtility.Texture2DInRenderTexture(in texture, ref m_renderer);
        m_onRendererChanged.Invoke(m_renderer);
    }

    public void PushTextureToRenderer(RenderTexture texture)
    {
        if (texture == null)
            return;
        m_onRendererChanged.Invoke(texture);
    }
    public void PushTextureToRenderer(Texture texture)
    {
        if (texture is RenderTexture) {
            RenderTexture t = (RenderTexture)texture;
            PushTextureToRenderer(t);
        }
        if (texture is Texture2D) {
            Texture2D t = (Texture2D)texture;
            PushTextureToRenderer(t);
        }
    }


}
