using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridaApplyTextureRelay : MonoBehaviour
{
    public Texture2D m_received;
    public Eloi.ClassicUnityEvent_Texture2D m_toApply;
    public void SetWithTexture(Texture2D texture) {
        m_received = texture;
        m_toApply.Invoke(texture);
    }
}
