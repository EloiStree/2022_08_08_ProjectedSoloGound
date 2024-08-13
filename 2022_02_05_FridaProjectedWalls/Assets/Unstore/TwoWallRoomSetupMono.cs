using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWallRoomSetupMono : MonoBehaviour
{
    public OrthogonicQuadSyncToCameraMono m_orthogonicGround;
    public Fake2DSquareZoneMono m_maxPlayableZone;
    public Transform m_joinPoint;

    public float m_width=4f;
    public float m_height=2.5f;
    public float m_depth= 4f;

    public float m_externalPadding=0.5f;


    public void SetWidthInMM(int valueInMM) { m_width = valueInMM / 1000f; }
    public void SetDepthInMM(int valueInMM) { m_depth = valueInMM / 1000f; }
    public void SetHeightInMM(int valueInMM) { m_height = valueInMM / 1000f; }

    public void RefreshWithCurrentValue() {
        Set(m_width, m_height, m_depth);
    }

    public void Set(float xwidht, float yheight, float zdepth) {

        m_orthogonicGround.m_width = xwidht;
        m_orthogonicGround.m_depth = zdepth;

        m_orthogonicGround.Refresh();

         m_orthogonicGround.m_quadDimensionRoot.position = new Vector3(m_joinPoint.position.x, m_joinPoint.position.y, m_joinPoint.position.z - m_depth / 2f);

        Vector3 maxZone = new Vector3(m_width, 1, m_depth + m_height);

        m_maxPlayableZone.m_squareZone .position = new Vector3(m_joinPoint.position.x, m_joinPoint.position.y, (m_joinPoint.position.z - (m_depth + m_height)/2f)+m_height);
        Vector3 maxZonePad = new Vector3(maxZone.x+ m_externalPadding, 1, maxZone.z+ m_externalPadding);
        m_maxPlayableZone.m_squareZone.localScale = maxZonePad;

    }


    //private void OnValidate()
    //{
    //    RefreshWithCurrentValue();
    //}

}
