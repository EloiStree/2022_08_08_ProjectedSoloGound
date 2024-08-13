using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class AnchorToKinectWorldSpaceRaycastMono : MonoBehaviour
    {
        public GetMesh3DWorldPointFromCameraMono m_source;
        public Transform m_kinectTransform;
        public Transform m_anchorToMove;

         public KinectAnchorPointKey m_savePoint;
    public string m_prefsId;
    [ContextMenu("Player PRef Id")]
    public void GenerateId() {
        //Eloi.E_GeneralUtility.GetRandomIdOf(out long randomValue);
        Eloi.E_UnityRandomUtility.GetRandomGUID(out m_prefsId);
       
    }
    public void AnchorToMousePosition()
        {
            Vector3 mousePosition = Input.mousePosition;
            m_source.GetDepthCoordinateOfScreenPixel(mousePosition, out m_savePoint.m_depthX, out m_savePoint.m_depthY);
            if (m_savePoint.m_depthX >= 0 && m_savePoint.m_depthY >= 0) { 
                m_source.GetKinectLocalSpaceFromDepth(m_savePoint.m_depthX, m_savePoint.m_depthY, out Vector3 kinectLocalPosition);
                Eloi.E_RelocationUtility.GetLocalToWorld_Point(kinectLocalPosition, m_kinectTransform, out Vector3 worldPosition);
                m_anchorToMove.position = worldPosition;
            m_savePoint.m_kinectWorldPoint = kinectLocalPosition;
                m_source.GetMeshWorldPointOfScreenPoint(m_savePoint.m_depthX, m_savePoint.m_depthY,out m_savePoint.m_unityWorldPoint);
            }
        }

        private void Reset()
        {
            m_anchorToMove = transform;
            Eloi.E_SearchInSceneUtility.TryToFetchWithActiveInScene(ref m_source,true);

        }

    private void OnEnable()
    {
        if(PlayerPrefs.HasKey(m_prefsId))
            m_savePoint =JsonUtility.FromJson<KinectAnchorPointKey>(
                PlayerPrefs.GetString(m_prefsId));
    }
    private void OnDisable()
    {
        PlayerPrefs.SetString(m_prefsId, JsonUtility.ToJson(m_savePoint));
    }
    private void OnDestroy()
    {
        PlayerPrefs.SetString(m_prefsId, JsonUtility.ToJson(m_savePoint));
    }

}
[System.Serializable]
public class KinectAnchorPointKey {

    public int m_depthX;
    public int m_depthY;
    public Vector3 m_unityWorldPoint;
    public Vector3 m_kinectWorldPoint;
}


