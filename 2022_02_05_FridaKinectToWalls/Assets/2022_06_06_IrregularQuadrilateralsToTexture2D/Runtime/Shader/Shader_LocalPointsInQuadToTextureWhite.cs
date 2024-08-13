using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shader_LocalPointsInQuadToTextureWhite : MonoBehaviour
{
    public Vector2 m_p0DL ;
    public Vector2 m_p1DR ;
    public Vector2 m_p2TR ;
    public Vector2 m_p3TL ;

    public ComputeShader m_irregularQuadrilToTextureShader;
    public Shader_FlushToColorTargetTexture m_setColorOfTexture;
    

    [HideInInspector]
    public Vector3[] m_localPoints;

    [HideInInspector]
    public Vector3[] m_localPointsKeep;

    public int m_width=16;
    public int m_height=16;
    public bool m_inverseHorizontal;
    public bool m_inverseVertical;
    public RenderTexture m_renderTexture;
    public Eloi.ClassicUnityEvent_RenderTexture m_createdRenderer;

    public float m_ignoreMinHeightInMeter = 0.1f;
    public float m_ignoreMaxHeightInMeter = 0.3f;


    public void SetInverseHorizontal(bool isInverse) { m_inverseHorizontal = isInverse; }
    public void SetInverseVertical(bool isInverse) { m_inverseVertical = isInverse; }
    public void SetMinHeight(float minHeight) { m_ignoreMinHeightInMeter = minHeight; }
    public void SetMaxHeight(float maxHeight) { m_ignoreMaxHeightInMeter = maxHeight; }
    public void SetMinHeightMM(int minHeight) { m_ignoreMinHeightInMeter = minHeight / 100f; }
    public void SetMaxHeightMM(int maxHeight) { m_ignoreMaxHeightInMeter = maxHeight / 100f; }
    public void SetExportWidth(int width) { m_width = width; }
    public void SetExportHeight(int height) { m_height = height; }


    public Vector3[] m_debugReceived = new Vector3[5000];
    public bool m_useDebugReturn;
    public Vector3[] m_debugFiltered = new Vector3[5000];


    public int m_threadUsedInShader = 64;
    public void SetLocalPointsMultipleOf64(in Vector3[] points) {
        m_localPoints = points;

    }
    public void SetQuadrilateralFromFoursZeroLocalPointsXZ(Vector3 downLeft, Vector3 downRight, Vector3 topLeft, Vector3 topRight)
    {

        m_p0DL = new Vector2(downLeft.x, downLeft.z);
        m_p1DR = new Vector2(downRight.x, downRight.z);
        m_p2TR = new Vector2(topRight.x, topRight.z);
        m_p3TL = new Vector2(topLeft.x, topLeft.z);

    }
    public void SetQuadrilateralFromFoursZeroLocalPoints(Vector2 downLeft, Vector2 downRight, Vector2 topLeft, Vector2 topRight)
    {

        m_p0DL = downLeft;
        m_p1DR = downRight;
        m_p2TR = topRight;
        m_p3TL = topLeft;
    }

    public ComputeBuffer bufferSet;
    public int m_previousLenght=0;
    public float m_ignoreLeft;
    public float m_ignoreRight;
    public float m_ignoreTop;
    public float m_ignoreDown;

    [ContextMenu("Compute Given Data")]
    public void ComputeAndPushTexture2D()
    {
        if (m_p0DL == Vector2.zero &&
            m_p1DR == Vector2.zero &&
            m_p2TR == Vector2.zero &&
            m_p3TL == Vector2.zero)
            return;

        if (m_renderTexture == null) { 
            m_renderTexture = new RenderTexture(m_width, m_height,0);
            m_renderTexture.enableRandomWrite = true;
            Graphics.SetRandomWriteTarget(0, m_renderTexture);
            m_createdRenderer.Invoke(m_renderTexture);
            Eloi.E_DebugLog.A("Hum");
        }
        if (m_localPoints == null || m_localPoints.Length <= 0)
            return;

        bool changeOfLenght = m_previousLenght != m_localPoints.Length;
        if (changeOfLenght || bufferSet==null) {
            if (bufferSet != null)
            {
                bufferSet.Release();
                bufferSet.Dispose();
            }

//            bufferSet = new ComputeBuffer(m_localPoints.Length, sizeof(float) * 3, ComputeBufferType.Constant);
            bufferSet = new ComputeBuffer(m_localPoints.Length, sizeof(float) * 3,ComputeBufferType.Default);
            m_previousLenght = m_localPoints.Length;
            m_localPointsKeep =new Vector3[ m_localPoints.Length];
            Eloi.E_DebugLog.B("Hum");

        }

        if (m_setColorOfTexture)
            m_setColorOfTexture.SetColorOnTexture(m_renderTexture);
        int kernel = m_irregularQuadrilToTextureShader.FindKernel("CSMain");

        bufferSet.SetData(m_localPoints);
        //int moduloOfGroup = m_localPoints.Count % m_threadUsedInShader;
        //if (moduloOfGroup != 0)
        //{
        //    for (int i = 0; i < (m_threadUsedInShader - moduloOfGroup); i++)
        //    {
        //        m_localPoints.Add(Vector3.zero);
        //    }
        //}

        m_ignoreLeft = m_p0DL.x < m_p3TL.x ? m_p0DL.x : m_p3TL.x;
        m_ignoreRight = m_p1DR.x > m_p2TR.x ? m_p1DR.x : m_p2TR.x;
        m_ignoreTop = m_p3TL.y > m_p2TR.y ? m_p3TL.y : m_p2TR.y;
        m_ignoreDown = m_p0DL.y < m_p1DR.y ? m_p0DL.y : m_p1DR.y;


        m_irregularQuadrilToTextureShader.SetBuffer(kernel, "m_localPointsArray", bufferSet);
        m_irregularQuadrilToTextureShader.SetTexture(kernel, "Result", m_renderTexture);
        m_irregularQuadrilToTextureShader.SetFloats("m_p0DL", m_p0DL.x, m_p0DL.y);
        m_irregularQuadrilToTextureShader.SetFloats("m_p1DR", m_p1DR.x, m_p1DR.y);
        m_irregularQuadrilToTextureShader.SetFloats("m_p2TR", m_p2TR.x, m_p2TR.y);
        m_irregularQuadrilToTextureShader.SetFloats("m_p3TL", m_p3TL.x, m_p3TL.y);
        m_irregularQuadrilToTextureShader.SetFloat("m_ignoreLeft",  m_ignoreLeft);
        m_irregularQuadrilToTextureShader.SetFloat("m_ignoreRight", m_ignoreRight);
        m_irregularQuadrilToTextureShader.SetFloat("m_ignoreTop",   m_ignoreTop);
        m_irregularQuadrilToTextureShader.SetFloat("m_ignoreDown ", m_ignoreDown);
        m_irregularQuadrilToTextureShader.SetFloat("m_ignoreMinHeight", m_ignoreMinHeightInMeter);
        m_irregularQuadrilToTextureShader.SetFloat("m_ignoreMaxHeight", m_ignoreMaxHeightInMeter);
        m_irregularQuadrilToTextureShader.SetInt("m_textureWidth", m_renderTexture.width);
        m_irregularQuadrilToTextureShader.SetInt("m_textureHeight", m_renderTexture.height);
        m_irregularQuadrilToTextureShader.SetInt("m_inverseHorizontal", m_inverseHorizontal?1:0);
        m_irregularQuadrilToTextureShader.SetInt("m_inverseVertical", m_inverseVertical ? 1 : 0);

        

        m_irregularQuadrilToTextureShader.Dispatch(kernel, m_localPoints.Length / m_threadUsedInShader, 1, 1);  

        if(m_useDebugReturn)
            bufferSet.GetData(m_localPointsKeep);





        m_countTest++;
        m_createdRenderer.Invoke(m_renderTexture);
        if (m_debugReceived.Length < m_localPoints.Length)
        {
            for (int i = 0; i < m_debugReceived.Length; i++)
            {
                m_debugReceived[i] = m_localPoints[i];
            }
        }
        if (m_debugFiltered.Length < m_localPointsKeep.Length)
        {
            for (int i = 0; i < m_debugFiltered.Length; i++)
            {
                m_debugFiltered[i] = m_localPointsKeep[i];
            }
        }
    }
    private void OnDestroy()
    {

        bufferSet.Release();
        bufferSet.Dispose();
    }

    public void GetResultAsTexture2D(out RenderTexture result)
    {
        result = m_renderTexture;
    }
    public ulong m_countTest;
}