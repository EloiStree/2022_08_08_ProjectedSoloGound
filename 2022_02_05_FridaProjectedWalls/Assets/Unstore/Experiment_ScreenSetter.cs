using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experiment_ScreenSetter : MonoBehaviour
{
    public int m_displayForGround=1;
    public int m_displayForWall=2;

    public Camera m_groundCamera;
    public Camera m_wallCamera;


    public void SetGroundIndex(int index) { 
        m_displayForGround = index;
    }
    public void SetWallIndex(int index) { 
        m_displayForWall = index; 
    }

    [ContextMenu("Refresh with Value")]
   public void ActivateAndSetDisplay()
    {
        if (Display.displays.Length > 1)
            Display.displays[1].Activate();
        if (Display.displays.Length > 2 && (m_displayForWall>1 || m_displayForGround>1) )
            Display.displays[2].Activate();
        m_groundCamera.targetDisplay = m_displayForGround;
        m_wallCamera.targetDisplay = m_displayForWall;
    }

}
