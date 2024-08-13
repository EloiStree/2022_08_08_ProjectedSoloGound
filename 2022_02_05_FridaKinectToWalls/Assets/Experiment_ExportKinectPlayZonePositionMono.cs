using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experiment_ExportKinectPlayZonePositionMono : MonoBehaviour
{
    public MemoryFileConnectionMono m_connection;

    public Transform m_camera;
    public Transform m_topLeftPoint;
    public Transform m_topRightPoint;
    public Transform m_downLeftPoint;
    public Transform m_downRightPoint;

    public PlayZoneInCamera m_playZoneFromCameraView;

    [ContextMenu("Push Info")]
    public void PushInfo()
    {
        m_playZoneFromCameraView.m_topLeftPointLocal = m_camera.InverseTransformPoint(m_topLeftPoint.position);
        m_playZoneFromCameraView.m_topRightPointLocal = m_camera.InverseTransformPoint(m_topRightPoint.position);
        m_playZoneFromCameraView.m_downLeftPointLocal = m_camera.InverseTransformPoint(m_downLeftPoint.position);
        m_playZoneFromCameraView.m_downRightPointLocal = m_camera.InverseTransformPoint(m_downRightPoint.position);
        m_connection.Connection.SetAsOjectInJsonFromat(m_playZoneFromCameraView) ;
    }

    [ContextMenu("Recover Info")]
    public void RecovertInfo() {

        m_connection.Connection.GetInObjectFromJsonFormat(out m_playZoneFromCameraView);
        m_topLeftPoint.position = m_camera.TransformPoint(m_playZoneFromCameraView.m_topLeftPointLocal);
        m_topRightPoint.position = m_camera.TransformPoint(m_playZoneFromCameraView.m_topRightPointLocal);
        m_downLeftPoint.position = m_camera.TransformPoint(m_playZoneFromCameraView.m_downLeftPointLocal);
        m_downRightPoint.position = m_camera.TransformPoint(m_playZoneFromCameraView.m_downRightPointLocal);
    }

}

[System.Serializable]
public class PlayZoneInCamera {

    public Vector3 m_topLeftPointLocal;
    public Vector3 m_topRightPointLocal;
    public Vector3 m_downLeftPointLocal;
    public Vector3 m_downRightPointLocal;
}
