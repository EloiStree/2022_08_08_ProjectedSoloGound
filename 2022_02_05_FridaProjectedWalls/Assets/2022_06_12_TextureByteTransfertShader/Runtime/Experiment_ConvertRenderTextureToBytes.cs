using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class Experiment_ConvertRenderTextureToBytes : MonoBehaviour
{

    public int m_size = 64;
    public bool m_useUpdate;
    public RenderTexture m_renderTextureIn;
    public RenderTexture m_renderTextureDebugReturn;
    public ComputeShader m_convertShader;
    public ComputeBuffer m_recovertFloat;
    public int m_width;
    public int m_height;
    public int m_lenght;
    public int m_numberOfFloat;
    public int m_numberOfBytes;
      float[] m_colorFloat;
     byte[] m_colorAsByte;
    public float[] m_debugFloat = new float[50];
    public float[] m_debugByte = new float[50];

    // int[] m_index1D;

    public long m_texutreToFloat;
    public long m_floatToByte;
    //public long m_byteToFloat;

    public Eloi.ClassicUnityEvent_RenderTexture m_renderTextureChanged;
    public FloatEvent m_appliedFloat;
    public BytesEvent m_appliedBytes;
    [Header("If you need source to debug")]
    public Camera m_cameraInForDebug;

    [System.Serializable]
    public class FloatEvent : UnityEvent<float[]> { }
    [System.Serializable]
    public class BytesEvent : UnityEvent<byte[]> { }


    void Update()
    {
        if (m_useUpdate)
            Push();
        
    }

    public void SetRenderTextureWith(RenderTexture renderTexture) {
        m_renderTextureIn = renderTexture;
        m_renderTextureIn.enableRandomWrite = true;
        Graphics.SetRandomWriteTarget(0, m_renderTextureIn);
        m_renderTextureChanged.Invoke(m_renderTextureIn);

    }

    private void Push()
    {

        Stopwatch watch = new Stopwatch();
        watch.Start();
        if (m_renderTextureIn == null)
        {
           
            m_renderTextureIn = new RenderTexture(m_size, m_size, 0);
            m_renderTextureIn.enableRandomWrite = true;
            Graphics.SetRandomWriteTarget(0, m_renderTextureIn);
            if(m_cameraInForDebug)
                m_cameraInForDebug.targetTexture = m_renderTextureIn;
            m_renderTextureChanged.Invoke(m_renderTextureIn);

        }
        if (m_colorFloat == null || m_colorFloat.Length == 0)
        { 
            m_width = m_size;
            m_height = m_size;
            m_lenght = m_width * m_height;
            m_numberOfFloat = m_lenght * 4;
            m_numberOfBytes = m_numberOfFloat * 4;
            m_colorFloat = new float[m_numberOfFloat];
            m_colorAsByte = new byte[m_numberOfBytes];
        }

        if (m_recovertFloat == null) {
            m_recovertFloat = new ComputeBuffer(m_numberOfFloat, sizeof(float));
            m_recovertFloat.SetData(m_colorFloat);
        }
        int kernel = m_convertShader.FindKernel("CSMain");
        m_convertShader.SetTexture(kernel, "Result", m_renderTextureIn);
        
        m_convertShader.SetBuffer(kernel, "TextureAsFloat", m_recovertFloat);
        m_convertShader.SetInt("m_width", m_width);
        m_convertShader.Dispatch(kernel, 16, 16, 1);
        m_recovertFloat.GetData(m_colorFloat);
        watch.Stop();
        m_texutreToFloat = watch.ElapsedMilliseconds;

        m_appliedFloat.Invoke(m_colorFloat);

        ConvertFloat2Byte.ConvertFloatsToBytes(in m_colorFloat, ref m_colorAsByte, out m_floatToByte);
       
        m_appliedBytes.Invoke(m_colorAsByte);

       // ConvertFloat2Byte.ConvertBytesToFloat(in m_colorAsByte, ref m_colorFloat, out m_byteToFloat);
      

        for (int i = 0; i < m_debugFloat.Length; i++)
        {
            m_debugFloat[i] = m_colorFloat[i];
        }
        for (int i = 0; i < m_debugByte.Length; i++)
        {
            m_debugByte[i] = m_colorAsByte[i];
        }

    }

}


/*
 //https://stackoverflow.com/questions/40759199/sending-a-byte-to-the-gpu
 //encoding on the cpu

int myInt = 0;
myInt += (int)myByte1;
myInt += (int)(myByte2 << 8);
myInt += (int)(myByte3 << 16);
myInt += (int)(myByte4 << 24);

//decoding on the gpu

myByte1 = myInt & 0xFF;
myByte2 = (myInt >> 8) & 0xFF;
myByte3 = (myInt >> 16) & 0xFF;
myByte4 = (myInt >> 24) & 0xFF;
 
 */
