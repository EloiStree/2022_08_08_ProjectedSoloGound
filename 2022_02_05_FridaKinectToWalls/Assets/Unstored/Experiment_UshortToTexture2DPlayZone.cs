using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class Experiment_UshortToTexture2DPlayZone : MonoBehaviour
{

    public WorldPositionToPlayZoneSquarePourcentMono m_playZoneConverter;
    public GetMesh3DWorldPointFromCameraMono m_ushortToWorldSpace;
    public KinectFourPointsSquareCalibrationZoneMono m_playZone;
    public KinectUShortDepthRef m_ushortRef;
    public Texture2D m_playZoneTexture;
    public RawImage m_textureDebugUI;
    public int m_numberOfPositionToDealWith;
    Vector3[] m_worldPosition = new Vector3[512 * 512];
    public bool m_inverse;
    public int m_textureWidht=512;
    public int m_textureHeight=512;

    public bool m_inverseVerticalAtEnd;
    public bool m_inverseHorizontalAtEnd;
    public Eloi.ClassicUnityEvent_Texture2D m_generatedTexture;

    public double m_computeTimeDebug;
    public void SetWith(KinectUShortDepthRef ushortRef)
    {
        if (m_playZone == null || !m_playZone.IsPointDefined())
            return;
        m_ushortRef = ushortRef;


        if (m_playZoneTexture==null)
            m_playZoneTexture = new Texture2D(m_textureWidht, m_textureHeight);
        Color[] pixelColors = m_playZoneTexture.GetPixels();
        m_numberOfPositionToDealWith = 0;
        //5ms
        for (int i = 0; i < ushortRef.GetLength(); i++)
        {
            if (ushortRef.m_arrayRef[i] > 0) {
                GetPositionOf(in i, out m_worldPosition[m_numberOfPositionToDealWith]);
                m_numberOfPositionToDealWith++;
            }
        }
        //2-4 ms
        for (int i = m_numberOfPositionToDealWith; i < m_worldPosition.Length; i++)
        {
            m_worldPosition[i] = Vector3.zero;
        }
        Color black = Color.black;
        Color white = Color.white;
        //2-4 ms
        for (int i = 0; i < pixelColors.Length; i++)
        {
            pixelColors[i] = black;
        }
        Stopwatch stopWatch = new Stopwatch();
        Eloi.StopWatchSingleton.StartWatch();
        //200 ms
        float horizontal=0, vertical=0;
        for (int i = 0; i < m_worldPosition.Length; i++)
        {
            m_playZoneConverter.GetPercent(in m_worldPosition[i],in m_inverse, out  horizontal, out vertical);
            if(horizontal >= 0f && horizontal <= 1f && vertical >= 0f && vertical <= 1f)
                pixelColors[Get1DFrom2D(in horizontal, in vertical)] = white ;
        }
        Eloi.StopWatchSingleton.StopTrackAndDisplay();

        stopWatch.Stop();
        m_computeTimeDebug = stopWatch.Elapsed.TotalMilliseconds;

        if (m_computeTimeDebug > 20000.0)
        {
            UnityEngine.Debug.LogError("Ok there is a big bug here", this.gameObject);
            UnityEngine.Debug.Break();
        }

        if (m_inverseHorizontalAtEnd)
            Eloi.E_Texture2DColorUtility.InverseHorizontal(ref pixelColors, m_playZoneTexture.width, m_playZoneTexture.height);
        if(m_inverseVerticalAtEnd)
            Eloi.E_Texture2DColorUtility.InverseVertical(ref pixelColors, m_playZoneTexture.width, m_playZoneTexture.height);

        m_playZoneTexture.SetPixels(pixelColors);


        m_playZoneTexture.Apply();
        if (m_textureDebugUI)
            m_textureDebugUI.texture = m_playZoneTexture;

        

        m_generatedTexture.Invoke(m_playZoneTexture);
    }
    public int Get1DFrom2D(in float pH, in  float pV)
    {
        return Get1DFrom2D((int)(m_textureWidht * pH),(int)(m_textureHeight * pV), m_textureWidht);
    }
    public int Get1DFrom2D(in int x, in int y, in int w) {
        return y * w + x; 
    }

    public void SetInverseHorizontal(bool value) => m_inverseHorizontalAtEnd = value;
    public void SetInverseVertical(bool value) => m_inverseVerticalAtEnd = value;


    public Transform m_root;
    public KinectManager m_kinectManager;
    KinectInterop.SensorData m_sensorData;

    [Header("Debug TDD value")]
    public int x=100, y=100;
    public Vector3 m_positionInRoot;
    public void Update()
    {

        GetPositionOf(x, y, out m_positionInRoot);
        if (  !float.IsInfinity(m_positionInRoot.x) && !float.IsInfinity(m_positionInRoot.y) && !float.IsInfinity(m_positionInRoot.z) )
        {
            UnityEngine.Debug.DrawLine(m_root.position, m_positionInRoot, Color.blue+Color.red);
        }
    }

    public void GetPositionOf(in int xDepth, in int yDepth, in int depthWidth, out Vector3 whereToStorePosition)
    {

        GetPositionOf(yDepth * depthWidth + xDepth, out whereToStorePosition);

    }
    public void GetPositionOf(in int xDepth, in int yDepth, out Vector3 whereToStorePosition)
    {
        if (m_ushortRef == null)
        {
            whereToStorePosition = Vector3.zero;
            return;
        }
        GetPositionOf(yDepth * m_ushortRef.m_width + xDepth, out whereToStorePosition);

    }
    public void GetPositionOf(in int depthIndex, out  Vector3 whereToStorePosition) {

        if (m_kinectManager == null)
            m_kinectManager = KinectManager.Instance;
        if (m_sensorData == null)
            m_sensorData = KinectManager.Instance.GetSensorData();
        // int pixelIndex = y * SAMPLE_SIZE * m_ushortRef.m_width + x * SAMPLE_SIZE;
        // int depth = m_sensorData.depthImage[pixelIndex];
        if (m_root && m_sensorData != null && m_sensorData.depth2SpaceCoords != null)
            whereToStorePosition = m_root.TransformPoint(m_sensorData.depth2SpaceCoords[depthIndex]);
        else whereToStorePosition = Vector3.zero;
        Eloi.E_CodeTag.DirtyCode.Info("Should sent exception for for testing I just returned zero");
    }
}
