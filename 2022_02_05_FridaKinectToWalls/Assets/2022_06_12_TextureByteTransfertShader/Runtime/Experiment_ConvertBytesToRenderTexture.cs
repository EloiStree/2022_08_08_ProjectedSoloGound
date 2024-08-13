
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Experiment_ConvertBytesToRenderTexture : MonoBehaviour
{

    public int m_size = 64;
    public RenderTexture m_renderTextureOut;
    public ComputeShader m_convertShader;
    public ComputeBuffer m_recovertFloat;
    public int m_width;
    public int m_height;
    public int m_lenght;
    public int m_numberOfFloat;
    public int m_numberOfBytes;
    public int m_bytesReceived;
    public int m_floatReceived;
     float[] m_colorFloat;
    public float[] m_debugFloat = new float[50];
    public float[] m_debugByte = new float[50];


    public long m_byteToFloat;
    public long m_floatToTexture;

    public Eloi.ClassicUnityEvent_RenderTexture m_updatedTexture;


    public void Push(byte[] bytes)
    {
        if (m_renderTextureOut == null)
        {
            m_renderTextureOut = new RenderTexture(m_size, m_size, 0);
            m_renderTextureOut.enableRandomWrite = true;
            Graphics.SetRandomWriteTarget(0, m_renderTextureOut);
            m_updatedTexture.Invoke(m_renderTextureOut);
        }
        if (m_colorFloat == null || m_colorFloat.Length==0) { 
            m_width = m_size;
            m_height = m_size;
            m_lenght = m_width * m_height;
            m_numberOfFloat = m_lenght * 4;
            m_numberOfBytes = m_numberOfFloat * 4;
            m_colorFloat = new float[m_numberOfFloat];
        }

        ConvertFloat2Byte.ConvertBytesToFloat(in bytes, ref m_colorFloat, out m_byteToFloat);

        m_bytesReceived = bytes.Length;
        m_floatReceived = m_colorFloat.Length;


        Stopwatch watch = new Stopwatch();
        watch.Start();

        if (m_recovertFloat == null)
        {
            m_recovertFloat = new ComputeBuffer(m_numberOfFloat, sizeof(float));
        }

        m_recovertFloat.SetData(m_colorFloat);

        int kernel = m_convertShader.FindKernel("CSMain");
        m_convertShader.SetBuffer(kernel, "TextureAsFloat", m_recovertFloat);
        m_convertShader.SetTexture(kernel, "Result", m_renderTextureOut);
        m_convertShader.SetInt( "m_width", m_width);
        m_convertShader.Dispatch(kernel, 16, 16, 1);
        watch.Stop();
        m_floatToTexture = watch.ElapsedMilliseconds;

        m_updatedTexture.Invoke(m_renderTextureOut);

        for (int i = 0; i < m_debugFloat.Length; i++)
        {
            m_debugFloat[i] = m_colorFloat[i];
        }
        for (int i = 0; i < m_debugByte.Length; i++)
        {
            m_debugByte[i] = bytes[i];
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
