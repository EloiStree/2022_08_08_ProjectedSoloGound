using Eloi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FetchKinectPointFromQuadToTexture2DMono : MonoBehaviour
{
    public KinectManager m_kinectManager;
    public Shader_WorldPointsToLocalPoints m_worldToLocalShader;
    public Shader_LocalPointsInQuadToTextureWhite m_localToTexture2D;
    public bool m_useShaderToRemoveSoloPoints = true;
    public Shader_RemoveSoloPixel m_removeTextureSoloPixel;
    public RenderTexture m_result;
    public Eloi.ClassicUnityEvent_RenderTexture m_onTextureComputed;

    public bool m_useUnityUpdate = true;
    public double m_timeInMs;

    public AnchorToKinectWorldSpaceRaycastMono m_tr;
    public AnchorToKinectWorldSpaceRaycastMono m_tl;
    public AnchorToKinectWorldSpaceRaycastMono m_dr;
    public AnchorToKinectWorldSpaceRaycastMono m_dl;

    public IrregularQuadrilateralsWorkSpace m_wordspace;
    private void Update()
    {
        if (m_useUnityUpdate)
        {
            Refresh();
        }
    }
     Vector3 [] m_localPoint;


    public int m_width;
    public int m_height;
    public int m_pointsUsed;


    public void GetCurrentTexture(out RenderTexture texture) {
        texture = m_result;
    }

    public void Refresh()
    {
        if (m_kinectManager == null || !m_kinectManager.IsInitialized()
            || m_kinectManager.GetSensorData()==null)
            return;

        m_localPoint= m_kinectManager.GetSensorData().depth2SpaceCoords;
        m_width = m_kinectManager.GetDepthImageWidth();
        m_height = m_kinectManager.GetDepthImageHeight();
        GetQuadPoint(out Vector3 tr, out Vector3 tl, out Vector3 dr, out Vector3 dl);

        Stopwatch watch = new Stopwatch();
        watch.Start();
        m_pointsUsed = m_localPoint.Length;

        IrregularQuadrilateralsUtility.GetWorkspaceFromWorldSpace(dl, tl, dr, tr, out m_wordspace);


        //m_irregularQuadUsed.GetCoordinateSystemInfo(out Vector3 worldPositionSystem, out Quaternion worldRotationSystem);
        m_worldToLocalShader.SetCoordinateSystem(in m_wordspace.m_originPosition, in m_wordspace.m_origineDirection);
        m_worldToLocalShader.SetPoints(m_localPoint);
        m_worldToLocalShader.ComputeInformation();
        SetLocalTextureLocalQuadZone(m_wordspace);
        //m_localToTexture2D.SetQuadrilateralFromFoursZeroLocalPointsXZ
        //    (dl, dr, tl, tr);
        m_localToTexture2D.SetLocalPointsMultipleOf64( in m_worldToLocalShader.m_localPointsArray);
        m_localToTexture2D.ComputeAndPushTexture2D();
        m_localToTexture2D.GetResultAsTexture2D(out m_result);

        if (m_useShaderToRemoveSoloPoints && m_removeTextureSoloPixel != null)
            m_removeTextureSoloPixel.RemoveSoloPixel(in m_result);

        m_onTextureComputed.Invoke(m_result);
        watch.Stop();
        m_timeInMs = watch.ElapsedMilliseconds;

        if(m_useDebugDraw)
            TestMesh();
    }
    public bool m_useDebugDraw=true;
    public int m_randomDraw = 1500;
    public float m_drawSize = 0.01f;
     Vector3 m_direction = Vector3.up * 0.1f;
    private void TestMesh()
    {
        GetQuadPoint(out Vector3 tr, out Vector3 tl, out Vector3 dr, out Vector3 dl);
        E_DrawingUtility.DrawLines(Time.deltaTime, Color.blue + Color.red / 2f, tr, tl, dl, dr, tr);

        GetWorkspQuadPoint(out Vector3 itr, out Vector3 itl, out Vector3 idr, out Vector3 idl);
        E_DrawingUtility.DrawLines(Time.deltaTime, Color.blue + Color.red / 2f,
            itr, itl, idl, idr, itr);

        float delta = Time.deltaTime;
        Color c = Color.white;
        Vector3 point;
        m_direction = Vector3.up * m_drawSize ;
        for (int i = 0; i < m_randomDraw; i++)
        {
            // Eloi.E_UnityRandomUtility.GetRandomOf(out point, in m_worldToLocalShader.m_localPointsArray);
            Eloi.E_UnityRandomUtility.GetRandomOf(out point, in m_localToTexture2D.m_localPoints);
            E_DrawingUtility.DrawLinesInDirection(in point, in m_direction, in delta, in c);
        }

        c = Color.blue;
        for (int i = 0; i < m_randomDraw; i++)
        {
            Eloi.E_UnityRandomUtility.GetRandomOf(out point, in m_localPoint);
            E_DrawingUtility.DrawLinesInDirection(in point, in m_direction, in delta, in c);
        }
        if (m_localToTexture2D.m_localPointsKeep.Length>0) { 
            c = Color.yellow;
            m_direction = Vector3.left * m_drawSize;
            for (int i = 0; i < m_randomDraw; i++)
            {
                Eloi.E_UnityRandomUtility.GetRandomOf(out point, in m_localToTexture2D.m_localPointsKeep);
                E_DrawingUtility.DrawLinesInDirection(in point, in m_direction, in delta, in c);
            }
        }
    }

    private void GetWorkspQuadPoint(out Vector3 tr, out Vector3 tl, out Vector3 dr, out Vector3 dl)
    {
        tr = m_wordspace.m_zeroLocalSpace.m_topRight;
        tl = m_wordspace.m_zeroLocalSpace.m_topLeft;
        dr = m_wordspace.m_zeroLocalSpace.m_downRight;
        dl = m_wordspace.m_zeroLocalSpace.m_downLeft;
    }

    private void GetQuadPoint(out Vector3 tr, out Vector3 tl, out Vector3 dr, out Vector3 dl)
    {
        tr = m_localPoint[m_tr.m_savePoint.m_depthY * m_width + m_tr.m_savePoint.m_depthX];
        tl = m_localPoint[m_tl.m_savePoint.m_depthY * m_width + m_tl.m_savePoint.m_depthX];
        dr = m_localPoint[m_dr.m_savePoint.m_depthY * m_width + m_dr.m_savePoint.m_depthX];
        dl = m_localPoint[m_dl.m_savePoint.m_depthY * m_width + m_dl.m_savePoint.m_depthX];

        tr =m_tr.m_savePoint.m_kinectWorldPoint;
        tl =m_tl.m_savePoint.m_kinectWorldPoint;
        dr =m_dr.m_savePoint.m_kinectWorldPoint;
        dl =m_dl.m_savePoint.m_kinectWorldPoint;
    }

    private void SetLocalTextureLocalQuadZone(IrregularQuadrilateralsWorkSpace quadWorkspace)
    {
        m_localToTexture2D.SetQuadrilateralFromFoursZeroLocalPointsXZ(
        quadWorkspace.m_zeroLocalSpace.m_downLeft,
        quadWorkspace.m_zeroLocalSpace.m_downRight,
        quadWorkspace.m_zeroLocalSpace.m_topLeft,
        quadWorkspace.m_zeroLocalSpace.m_topRight);

    }
}
