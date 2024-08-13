using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RefreshWhenKinectV2RefreshMono : MonoBehaviour
{
    public KinectManager m_kinectManager;

    public Vector3[] m_100First= new Vector3[100];
    public UnityEvent m_changeHappens;
    public DateTime m_lastTime;
    public double m_timeBetween;
    private void Awake()
    {
        m_lastTime = DateTime.Now;
    }
    public void Update()
    {

        bool changeHappend = false;
        KinectManager manager = KinectManager.Instance;
        if (manager && manager.IsInitialized() )
        {
            KinectInterop.SensorData data = manager.GetSensorData();
            if(data!=null && data.depth2SpaceCoords.Length>100)
            for (int i = 0; i < 100; i++)
            {
                    if (data.depth2SpaceCoords[i].x != m_100First[i].x) {
                        changeHappend = true;
                    } 
                    m_100First[i]= data.depth2SpaceCoords[i];
            }

        }
        if (changeHappend) { 
        
            m_changeHappens.Invoke();
            DateTime now = DateTime.Now;
            m_timeBetween = (now - m_lastTime).TotalMilliseconds;
            m_lastTime = now;
        }
    }
}
