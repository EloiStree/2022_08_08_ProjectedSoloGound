
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dirty_PushKinectAsRendererMono : MonoBehaviour
{
    public int m_width = 512;
    public int m_height = 256;
    public uint m_contentId = 5;
    public RenderTexture m_texture;

    public RenderTextureEvent m_renderTextureEvent;
    [System.Serializable]
    public class RenderTextureEvent : UnityEvent<RenderTexture> { };

    public TextureWithIdEvent m_textureEvent;
    [System.Serializable]
    public class TextureWithIdEvent : UnityEvent<Texture, uint> { };

    
    [ContextMenu("Push")]
    public void Push(RenderTexture texture)
    {
        m_texture = texture;
        if (m_texture == null)
            return;
        m_width = m_texture.width;
        m_height = m_texture.height;
        m_texture.enableRandomWrite = true;
        Graphics.SetRandomWriteTarget(0, m_texture);
       
        m_renderTextureEvent.Invoke(m_texture);
        m_textureEvent.Invoke(m_texture, m_contentId);

    }
}

