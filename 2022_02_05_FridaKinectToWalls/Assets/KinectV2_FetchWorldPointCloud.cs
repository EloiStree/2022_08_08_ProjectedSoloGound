using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class KinectV2_FetchWorldPointCloud : ContainMassGroupOfVector3Mono
{
    public KinectManager m_kinectManager;
    public KinectInterop.SensorData m_sensorData;
    public Transform m_coordinateSystemContext;
    public Vector3 m_positionCamera;
    public Quaternion m_rotationCamera;
    public Vector3[] m_sensorPositions;
    public Vector3[] m_worldPoints;
    public long m_timeToCompute;
    public bool m_refreshWithUpdate;
    public bool m_refreshWhenAccessed;
    private void Awake()
    {
        m_sensorPositions = new Vector3[0];
        m_worldPoints = new Vector3[0];
    }

    public void Update()
    {
        if (m_refreshWithUpdate)
        {
            Refresh();
        }

    }

    public override void GetVector3Ref(out Vector3[] points)
    {

        if (m_refreshWhenAccessed)
        {
            Refresh();
        }

        points = m_worldPoints;
    }

    private void Refresh()
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();


        if (m_kinectManager == null)
            m_kinectManager = KinectManager.Instance;
        if (m_sensorData == null)
            m_sensorData = KinectManager.Instance.GetSensorData();

        if (m_worldPoints.Length != m_sensorData.depth2SpaceCoords.Length)
            m_worldPoints = new Vector3[m_sensorData.depth2SpaceCoords.Length];


        m_rotationCamera = m_coordinateSystemContext.rotation;
        m_positionCamera = m_coordinateSystemContext.position;
        if (m_coordinateSystemContext && m_sensorData != null && m_sensorData.depth2SpaceCoords != null)
        {
            m_sensorPositions = m_sensorData.depth2SpaceCoords;
            for (int i = 0; i < m_sensorData.depth2SpaceCoords.Length; i++)
            {
                // int pixelIndex = y * SAMPLE_SIZE * m_ushortRef.m_width + x * SAMPLE_SIZE;
                // int depth = m_sensorData.depthImage[pixelIndex];

                if (float.IsInfinity(m_sensorData.depth2SpaceCoords[i].x) || float.IsInfinity(m_sensorData.depth2SpaceCoords[i].y) || float.IsInfinity(m_sensorData.depth2SpaceCoords[i].z))
                {
                    m_worldPoints[i] = Vector3.zero;
                }
                else
                {
                    Eloi.E_RelocationUtility.GetLocalToWorld_Point(in m_sensorData.depth2SpaceCoords[i], in m_positionCamera, in m_rotationCamera,
                        out m_worldPoints[i]);
                }

            }

        }
        else throw new Exception("Should not be launch");
        watch.Stop();
        m_timeToCompute = watch.ElapsedMilliseconds;
    }
}
