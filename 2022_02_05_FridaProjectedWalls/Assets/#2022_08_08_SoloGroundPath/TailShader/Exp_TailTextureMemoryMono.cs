using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Exp_TailTextureMemoryMono : MonoBehaviour
{

    public Texture m_renderTextureIn;
    public RenderTexture m_renderTextureDebugReturn;
    public RenderTexture m_debugColor;
    public ComputeShader m_convertShader;
    public ComputeShader m_fadeShader;
    public ComputeBuffer m_recovertFloat;
    public int m_width;
    public int m_height;
    public Eloi.ClassicUnityEvent_RenderTexture m_renderTextureChanged;
    public Eloi.ClassicUnityEvent_RenderTexture m_debugColorChanged;

    public float m_isWhitePercent = 0.95f;

    public float m_fadeRatioGlobal=1;
    public float m_fadeRatio1White=9;
    public float m_fadeRatio3White=3;
    public float m_fadeRatio6White=1;
    public float m_fadeRatio7White=2;
    public float m_fadeRatioMostlyWhite=0;
    public float m_fadeRatioContinulsy=0.1f;
    public void SetFadeGlobalMultiplicator(float ratio) => m_fadeRatioGlobal = ratio;
    public void SetFadeRatioFor1WhitePixel(float ratio) => m_fadeRatio1White = ratio;
    public void SetFadeRatioFor3WhitePixel(float ratio) => m_fadeRatio3White = ratio;
    public void SetFadeRatioFor6WhitePixel(float ratio) => m_fadeRatio6White = ratio;
    public void SetFadeRatioFor7WhitePixel(float ratio) => m_fadeRatio7White = ratio;
    public void SetFadeRatioForMostlyWhite(float ratio) => m_fadeRatioMostlyWhite = ratio;
    public void SetContinueFade(float ratio) => m_fadeRatioContinulsy = ratio;



    public long m_timeUsedToFade;
    public void FadePixel(in float toRemove)
    {
        if (toRemove == 0f)
            return;
        if (m_renderTextureDebugReturn == null)
            return;
        Stopwatch w = new Stopwatch();
        w.Start();
        int kernel = m_fadeShader.FindKernel("CSMain");
        m_fadeShader.SetTexture(kernel, "Memory", m_renderTextureDebugReturn);
        m_fadeShader.SetTexture(kernel, "DebugColor", m_debugColor);
        m_fadeShader.SetFloat("m_toRemove", toRemove);
        m_fadeShader.SetInt("m_width", m_width);
        m_fadeShader.SetInt("m_height", m_height);
        m_fadeShader.SetFloat("m_globalRatio", m_fadeRatioGlobal);
        m_fadeShader.SetFloat("m_fadeRatio1White",      m_fadeRatio1White   );
        m_fadeShader.SetFloat("m_fadeRatio3White",      m_fadeRatio3White   );
        m_fadeShader.SetFloat("m_fadeRatio6White", m_fadeRatio6White);
        m_fadeShader.SetFloat("m_fadeRatio6White", m_fadeRatio6White);
        m_fadeShader.SetFloat("m_fadeRatio7White", m_fadeRatio7White);
        m_fadeShader.SetFloat("m_fadeRatioMostlyWhite", m_fadeRatioMostlyWhite);
        m_fadeShader.SetFloat("m_fadeRatioContinulsy", m_fadeRatioContinulsy);
        m_fadeShader.Dispatch(kernel, 16, 16, 1);
        w.Stop();
        m_timeUsedToFade = w.ElapsedMilliseconds;
        m_renderTextureChanged.Invoke(m_renderTextureDebugReturn);
        m_debugColorChanged.Invoke(m_debugColor);
    }


    public void Push(in Texture textureToPrint, in int width, int height)
    {
        m_renderTextureIn = textureToPrint;
        if (m_renderTextureIn == null)
            return;
        Stopwatch watch = new Stopwatch();
        watch.Start();
        if (m_renderTextureDebugReturn == null 
            || m_renderTextureDebugReturn.width!= width
            || m_renderTextureDebugReturn.height != height)
        {

            m_renderTextureDebugReturn = new RenderTexture(width, height, 0);
            m_width = width; m_height = height;
            m_renderTextureDebugReturn.enableRandomWrite = true;
            Graphics.SetRandomWriteTarget(0, m_renderTextureDebugReturn);
            m_debugColor = new RenderTexture(width, height, 0);
            m_width = width; m_height = height;
            m_debugColor.enableRandomWrite = true;
            Graphics.SetRandomWriteTarget(0, m_debugColor);

            
        }
        
        int kernel = m_convertShader.FindKernel("CSMain");
        m_convertShader.SetTexture(kernel, "ToPush", m_renderTextureIn);
        m_convertShader.SetTexture(kernel, "Memory", m_renderTextureDebugReturn);
        m_convertShader.SetFloat("m_isWhite", m_isWhitePercent);
        m_convertShader.Dispatch(kernel, 16, 16, 1);
        watch.Stop();

        m_renderTextureChanged.Invoke(m_renderTextureDebugReturn);
    }

}

