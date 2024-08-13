using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthogonicQuadSyncToCameraMono : MonoBehaviour
{
    public Transform m_quadDimensionRoot;
    public Camera m_targetCamera;
    public float m_width=3;
    public float m_depth=2;
    public float m_pixelWidth=800;

    public Eloi.ClassicUnityEvent_RenderTexture m_dimensionChanged;

    public void SetWidthPixelResolutiont(int pixelWidth) {
        m_pixelWidth = pixelWidth;
    }

    public void Refresh()
    {
        // m_targetCamera.orthographicSize = (m_width / m_depth) * m_depth * 0.5f;

        float ratio = m_width / m_depth;
        m_targetCamera.orthographicSize = m_depth * 0.5f;
        m_quadDimensionRoot.localScale = new Vector3(m_width, 1, m_depth);

        //if (Application.isPlaying &&  m_targetCamera.targetTexture != null)
        //{
        //    m_targetCamera.targetTexture.Release();
        //}
        m_targetCamera.targetTexture = new RenderTexture((int)m_pixelWidth, (int)(m_pixelWidth / ratio),24 , RenderTextureFormat.ARGB32);
        m_dimensionChanged.Invoke(m_targetCamera.targetTexture);
    }
}
