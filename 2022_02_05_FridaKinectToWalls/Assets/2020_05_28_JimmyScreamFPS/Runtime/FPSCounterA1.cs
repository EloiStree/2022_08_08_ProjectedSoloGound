using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//
//Code based on this source: https://catlikecoding.com/unity/tutorials/frames-per-second/
public class FPSCounterA1 : MonoBehaviour
{
    public int FPS { get; private set; }
    public int AverageFPS { get; private set; }
    public int HighestFPS { get; private set; }
    public int LowestFPS { get; private set; }
    [SerializeField] int[] fpsBuffer;
    [SerializeField] int m_fpsBufferIndex;
    [SerializeField] int m_frameRange = 60;


    [SerializeField] Text [] m_displayFps;
    [SerializeField] string m_display;
    void Update()
    {
        FPS = (int)(1f / Time.unscaledDeltaTime);
        if (fpsBuffer == null || fpsBuffer.Length != m_frameRange)
        {
            InitializeBuffer();
        }
        UpdateBuffer();
        CalculateFPS();
        m_display = string.Format("{0:000}\t{1:000}\tl {2:00}\t H {3:00}", FPS, AverageFPS, LowestFPS, HighestFPS);
        for (int i = 0; i < m_displayFps.Length; i++)
        {
            if (m_displayFps[i])
                m_displayFps[i].text = m_display;
        }
    }

    void UpdateBuffer()
    {
        fpsBuffer[m_fpsBufferIndex++] = (int)(1f / Time.unscaledDeltaTime);
        if (m_fpsBufferIndex >= m_frameRange)
        {
            m_fpsBufferIndex = 0;
        }
    }
    void CalculateFPS()
    {
        int sum = 0;
        int highest = 0;
        int lowest = int.MaxValue;
        for (int i = 0; i < m_frameRange; i++)
        {
            int fps = fpsBuffer[i];
            sum += fps;
            if (fps > highest)
            {
                highest = fps;
            }
            if (fps < lowest)
            {
                lowest = fps;
            }
        }
        AverageFPS = sum / m_frameRange;
        HighestFPS = highest;
        LowestFPS = lowest;
    }
    void InitializeBuffer()
    {
        if (m_frameRange <= 0)
        {m_frameRange = 1;}
        fpsBuffer = new int[m_frameRange];
        m_fpsBufferIndex = 0;
    }
}
