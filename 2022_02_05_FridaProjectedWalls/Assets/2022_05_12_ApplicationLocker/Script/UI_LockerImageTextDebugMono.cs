using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LockerImageTextDebugMono : MonoBehaviour
{
    public Eloi.PrimitiveUnityEventExtra_Bool m_hasImage;
    public Eloi.ClassicUnityEvent_Texture2D m_imageToLoad;
    public Eloi.PrimitiveUnityEvent_String m_textIfNotImageLoaded;

    public Texture2D m_textureGiven=null;
    [TextArea(0,10)]
    public string m_textGiven="";

    public void Awake()
    {
        RefreshWithCurrentValue();
    }

    public void SetTexture(Texture2D texture)
    {
        m_textureGiven = texture;
        m_hasImage.Invoke(m_textureGiven != null);
        m_imageToLoad.Invoke(m_textureGiven);
    }
    public void SetText(string text)
    {
        m_textGiven = text;
        m_textIfNotImageLoaded.Invoke(text);
    }

    [ContextMenu("Refresh with value")]
    public void RefreshWithCurrentValue() {
        SetTexture(m_textureGiven);
        SetText(m_textGiven);
    }
}
