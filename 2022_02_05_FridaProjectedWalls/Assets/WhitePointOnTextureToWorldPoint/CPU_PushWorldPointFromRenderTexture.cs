using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CPU_PushWorldPointFromRenderTexture : MonoBehaviour
{
    public Transform m_whereToCreateThem;
    public Transform m_whereToCreateThemTopRight;
    public RenderTexture m_renderTexture;
    Vector3[] m_pointFound;
    public Vector3[] m_pointFoundValide;
    public Vector3ArrayEvent m_onPointFoundChanged;
    [System.Serializable]
    public class Vector3ArrayEvent : UnityEvent<Vector3[]> { }

    public ComputeShader m_renderTextureWhiteToWorldPoint;
    public ComputeBuffer m_bufferWorldPoint;
    public int m_width;
    public int m_height;
    public int m_lenght;

    public Eloi.ClassicUnityEvent_RenderTexture m_onUpdateTexture;

    public void SetRenderTexture(RenderTexture target) {
        m_renderTexture = target;
        if (m_renderTexture.width != m_width || m_renderTexture.height != m_height) {
            m_width = m_renderTexture.width;
            m_height = m_renderTexture.height;
            m_lenght = m_width * m_height;
            m_pointFound = new Vector3[m_lenght];

            m_renderTexture.enableRandomWrite = true;
            Graphics.SetRandomWriteTarget(0, m_renderTexture);
            m_onUpdateTexture.Invoke(m_renderTexture);

            if (m_bufferWorldPoint != null) {
                m_bufferWorldPoint.Release();
                m_bufferWorldPoint.Dispose();
            }
            m_bufferWorldPoint = new ComputeBuffer(m_lenght, sizeof(float) * 3);
            m_bufferWorldPoint.SetData(m_pointFound);
        }
    }
    
    public void Compute()
    {
        if (m_renderTexture == null)
            return;


        Eloi.E_RelocationUtility.GetWorldToLocal_Point(m_whereToCreateThemTopRight.position, in m_whereToCreateThem, out Vector3 localTopRight);
        

        Vector3 position = m_whereToCreateThem.position;
        Quaternion rotation = m_whereToCreateThem.rotation;
        int kernel = m_renderTextureWhiteToWorldPoint.FindKernel("CSMain");
        m_renderTextureWhiteToWorldPoint.SetTexture(kernel, "Result", m_renderTexture);
        m_renderTextureWhiteToWorldPoint.SetBuffer(kernel, "m_worldPoint", m_bufferWorldPoint);
        m_renderTextureWhiteToWorldPoint.SetInt("m_width", m_width);
        m_renderTextureWhiteToWorldPoint.SetInt("m_height", m_height); 
        m_renderTextureWhiteToWorldPoint.SetFloat("m_widthSize", localTopRight.x * 2f);
        m_renderTextureWhiteToWorldPoint.SetFloat("m_heightSize", localTopRight.z * 2f);
        m_renderTextureWhiteToWorldPoint.SetFloats("m_systemPosition",  position.x, position.y, position.z);
        m_renderTextureWhiteToWorldPoint.SetFloats("m_systemRotation", rotation.x, rotation.y, rotation.z, rotation.w);
        m_renderTextureWhiteToWorldPoint.Dispatch(kernel, m_renderTexture.width/ 8, m_renderTexture.height/ 8, 1);
        m_bufferWorldPoint.GetData(m_pointFound);
        m_pointFoundValide= m_pointFound.Where(k => k != Vector3.zero).ToArray();
        m_onPointFoundChanged.Invoke(m_pointFoundValide);


    }
}
