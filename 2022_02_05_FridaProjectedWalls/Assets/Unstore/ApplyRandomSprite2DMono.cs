using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyRandomSprite2DMono : MonoBehaviour
{
    public FridaTextureGroupScriptable m_givenTexture;
    public Eloi.ClassicUnityEvent_Texture2D m_applyTexture;
    public Texture2D m_debugPush;

    [ContextMenu("Apply Texture")]
    public void ApplyTexture() {
        m_givenTexture.GetRandomTexture(out Texture2D random);
        m_debugPush = random;
        m_applyTexture.Invoke(random);
    }
}


