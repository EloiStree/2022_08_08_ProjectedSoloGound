using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JimmyScreamer : MonoBehaviour
{

    public FPSCounterA1 m_fpsState;
    public AudioSource m_jimmyPlayer;
    public AudioSource m_earHellPlayer;
    public bool autoStart = true;

    [Header("Debug")]
    public int m_boundaryJimmyStart = 60;
    public int m_boundaryJimmyMax = 40;
    public int m_boundaryHellStart = 40;
    public int m_boundaryHellMax = 2;
    [Header("Debug")]
    [SerializeField] int m_fps;
    [SerializeField] float m_jimmy;
    [SerializeField] bool m_jimmyState;
    [SerializeField] float m_hell;
    [SerializeField] bool m_hellState;

    public void Awake()
    {
        if (autoStart) 
        Invoke("StartListening", 1.5f);
    }

    public bool m_isActive;
    public void StartListening() {
        m_isActive = true;
    }
    public void StopListening() {
        m_isActive = false;
    }

    void Update()
    {
        if (!m_isActive) return;
        bool previous=false;
        int fps = m_fpsState.AverageFPS ;

        previous = m_jimmyState;
        m_jimmy = GetPourcentFrom(fps, m_boundaryJimmyMax, m_boundaryJimmyStart);
        m_jimmyState = m_jimmy > 0f;
        if (previous != m_jimmyState) {
            if (m_jimmyState)
                m_jimmyPlayer.Play();
            else m_jimmyPlayer.Stop();
        }

        previous = m_hellState;
        m_hell = GetPourcentFrom(fps, m_boundaryHellMax, m_boundaryHellStart);
        m_hellState = m_hell > 0f;
        if (previous != m_hellState)
        {
            if (m_hellState)
                m_earHellPlayer.Play();
            else m_earHellPlayer.Stop();
        }

        m_jimmyPlayer.volume = m_jimmy;
        m_earHellPlayer.volume = m_hell;

       
    }

    private float GetPourcentFrom(float fps, float critiqueFps, float OkFps)
    {
        return 1f-Mathf.Clamp(((fps - critiqueFps) / (OkFps - critiqueFps)),0f,1f);
    }
}
