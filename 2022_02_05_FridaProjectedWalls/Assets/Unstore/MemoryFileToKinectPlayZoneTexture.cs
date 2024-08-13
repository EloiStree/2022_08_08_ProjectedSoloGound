using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MemoryFileToKinectPlayZoneTexture : MonoBehaviour
{

    public MemoryFileConnectionMono m_memoryConnection;
    public MemoryFileConnectionMono m_memoryConnectionLastUpdate;
    public Texture2D m_fetchTexture;
    public Eloi.ClassicUnityEvent_Texture2D m_relayFetchedTexture;
    public string m_lastUpdate;
    public double m_recoveringTime;
    public double m_eventDependant;
    [ContextMenu("Try to fetch")]
    public void TryToFetchTexture() {

        StartCoroutine(TryToFetchTextureCoroutine());
    }
    public IEnumerator TryToFetchTextureCoroutine()
    {

        m_memoryConnectionLastUpdate.Connection.GetAsText(out string text);
        yield return new WaitForEndOfFrame();
        if (Eloi.E_StringUtility.AreNotEquals(text, m_lastUpdate, true, true))
        {
            m_lastUpdate = text;

            Stopwatch stp = new Stopwatch();
            stp.Restart();
            m_memoryConnection.Connection.GetAsTexture2D(out m_fetchTexture);
            stp.Stop();
            m_recoveringTime = stp.Elapsed.TotalSeconds;

            stp.Restart();
            if (m_fetchTexture != null)
                m_relayFetchedTexture.Invoke(m_fetchTexture);
            stp.Stop();
            m_eventDependant = stp.Elapsed.TotalSeconds;
        }
        yield break;
    }

}
