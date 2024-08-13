using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp_TextureMemoryTailShaderMono : MonoBehaviour
{
    public Texture2D m_currentMemory;
    public RenderTexture l_lastReceived;
    public Texture2D m_lastReceived;
    public Exp_TailTextureMemoryMono m_shaderController;
    public void PushNewTexture(Texture2D t)
    {
        Texture d = t;
        PushNewTexture(d, t.width, t.width);
    }
    public void PushNewTexture(RenderTexture t)
    {
        Texture d = t;
        PushNewTexture(d, t.width, t.width);
    }
    public void PushNewTexture(Texture t, int width, int height)
    {
        m_shaderController.Push(t,width, height);
    }

    public float m_fadePerSeconds = 0.1f;
    public void Update()
    {
        if (m_fadePerSeconds == 0f)
            return;
        m_shaderController.FadePixel(Time.deltaTime * m_fadePerSeconds);
    }
}
