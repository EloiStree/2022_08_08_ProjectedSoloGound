using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FetchDepthWithoutBackgroundMono : MonoBehaviour
{
    public KinectManager m_kinectManager;
    public int m_width;
    public int m_height;
    public int m_lenght;
    private KinectUShortDepthRef m_original;
    public KinectUShortDepthRef m_frameDelta;
    private KinectUShortDepthRef m_backgroundDepth;
    private KinectUShortDepthRef m_result;
    public KinectUshortDepthRefEvent m_originalChanged;
    public KinectUshortDepthRefEvent m_backgroundDepthChanged;
    public KinectUshortDepthRefEvent m_resultChanged;

    public bool m_useTextureDebug;
    public Texture2D m_originalDebug;
    public Texture2D m_backgroundDebug;
    public Texture2D m_resultDebug;
    public Texture2D m_frameDeltaDebug;



    public ushort m_ignoreLenghtMm=100;
    public int m_previousLengt;
    public double m_computeTimeDebug;

    [ContextMenu("Fetch Refresh")]
    public void FetchRefresh() {

        m_width = m_kinectManager.GetDepthImageWidth();
        m_height = m_kinectManager.GetDepthImageHeight();
        m_lenght = (m_width * m_height);

        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        if (m_previousLengt != m_lenght) {
            m_previousLengt = m_lenght;
            m_original = new KinectUShortDepthRef(m_width, m_height, m_lenght);
            m_backgroundDepth = new KinectUShortDepthRef(m_width, m_height, m_lenght);
            m_result = new KinectUShortDepthRef(m_width, m_height, m_lenght);
            m_frameDelta = new KinectUShortDepthRef(m_width, m_height, m_lenght);
        }

     
        ushort [] depth= m_kinectManager ? m_kinectManager.GetRawDepthMap() : new ushort[0];

        for (int i = 0; i < m_lenght; i++)
        {
            m_frameDelta.m_arrayRef[i] = (ushort)((m_original.m_arrayRef[i] - depth[i] ) + 2000);
            m_original.m_arrayRef[i] = depth[i];
            if (depth[i] < m_backgroundDepth.m_arrayRef[i]-m_ignoreLenghtMm)
                m_result.m_arrayRef[i] = (ushort)(m_original.m_arrayRef[i]);
            else m_result.m_arrayRef[i] = 0;
        }
        stopWatch.Stop();
        m_computeTimeDebug = stopWatch.Elapsed.TotalMilliseconds;
        if (stopWatch.Elapsed.TotalSeconds > 30f) {
            UnityEngine.Debug.LogError("Ok there is a big bug here", this.gameObject);
            UnityEngine.Debug.Break();
        }

        if (m_useTextureDebug) 
        {
            UShortArrayUtility.SetTextureWithColorFromTo(in m_original.m_arrayRef, m_width, m_height, Color.green, Color.red, 4500, ref m_originalDebug);
            UShortArrayUtility.SetTextureWithColorFromTo(in m_result.m_arrayRef, m_width, m_height, Color.green, Color.red, 4500, ref m_resultDebug);
            UShortArrayUtility.SetTextureWithColorFromTo(in m_frameDelta.m_arrayRef, m_width, m_height, Color.green, Color.red, 4500, ref m_frameDeltaDebug);
            UShortArrayUtility.SetTextureWithColorFromTo(in m_backgroundDepth.m_arrayRef, m_width, m_height, Color.green, Color.red, 4500, ref m_backgroundDebug);
        }

        m_originalChanged.Invoke(m_original);
        m_resultChanged.Invoke(m_result);

    }
    public void SetMMToIgnoreFromBackground(ushort valueInMM)
    {
        m_ignoreLenghtMm = valueInMM;
    }
    public void SetMMToIgnoreFromBackground(int valueInMM)
    {
        m_ignoreLenghtMm = (ushort)valueInMM;
    }
    [ContextMenu("Save As Background")]
    public void SaveDepthAsBackground()
    {


        m_width = m_kinectManager.GetDepthImageWidth();
        m_height = m_kinectManager.GetDepthImageHeight();
        m_lenght = (m_width * m_height);
        if (m_backgroundDepth == null)
            m_backgroundDepth = new KinectUShortDepthRef(m_width, m_height, m_lenght);
        if (m_lenght != m_backgroundDepth.GetLength())
        { m_backgroundDepth.Set(m_width, m_height, m_lenght); }


        ushort[] depth = m_kinectManager ? m_kinectManager.GetRawDepthMap() : new ushort[0];
        for (int i = 0; i < m_lenght; i++)
        {
            m_backgroundDepth.m_arrayRef[i] = depth[i];
        }

        m_backgroundDepthChanged.Invoke(m_backgroundDepth);
        if (m_useTextureDebug)
        {
            UShortArrayUtility.SetTextureWithColorFromTo(in m_backgroundDepth.m_arrayRef, m_width, m_height, Color.green, Color.red, 4500, ref m_backgroundDebug);
        }
    }
}
