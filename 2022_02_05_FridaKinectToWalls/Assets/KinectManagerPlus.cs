using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinectManagerPlus : MonoBehaviour
{
    public KinectManager m_manager;

    public void SetColorMapComputeTo(bool isOn) {
        m_manager.computeColorMap = isOn;
    }
}
