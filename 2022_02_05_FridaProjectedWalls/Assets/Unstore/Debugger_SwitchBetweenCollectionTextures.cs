using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger_SwitchBetweenCollectionTextures : MonoBehaviour
{

    public Texture2D[] m_textures;
    public int m_index = 0;

    public Eloi.ClassicUnityEvent_Texture2D m_onTexturePushed;

    [ContextMenu("Next")]
    public void NextTexture()
    {
        if (m_textures.Length == 0)
            return;
        m_index++;
        if (m_index >= m_textures.Length)
            m_index = 0;
        m_onTexturePushed.Invoke(m_textures[m_index]);
    }
    [ContextMenu("Previous")]
    public void PreviousTexture()
    {
        if (m_textures.Length == 0)
            return;
        m_index--;
        if (m_index < 0)
            m_index = m_textures.Length-1;
        m_onTexturePushed.Invoke(m_textures[m_index]);
    }
    [ContextMenu("Random")]
    public void Random()
    {
        if (m_textures.Length == 0)
            return;
        Eloi.E_UnityRandomUtility.GetRandomN2M(0, m_textures.Length, out m_index);
        m_onTexturePushed.Invoke(m_textures[m_index]);
    }
}
