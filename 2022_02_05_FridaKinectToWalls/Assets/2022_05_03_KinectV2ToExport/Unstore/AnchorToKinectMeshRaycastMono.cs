using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorToKinectMeshRaycastMono : MonoBehaviour
{
    public GetMesh3DWorldPointFromCameraMono m_source;
    public Transform m_anchorToMove;
    public int m_depthX;
    public int m_depthY;

    public void AnchorToMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        m_source.GetDepthCoordinateOfScreenPixel(mousePosition, out m_depthX, out m_depthY);
        m_source.GetMeshWorldPointOfScreenPoint((int)mousePosition.x, (int)mousePosition.y, out Vector3 position);
        m_anchorToMove.position = position;
    }

    private void Reset()
    {
        m_anchorToMove = transform;
        
    }
}
