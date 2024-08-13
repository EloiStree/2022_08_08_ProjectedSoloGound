using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticuleControlsAtStartMono : MonoBehaviour
{
    public ParticleSystem m_systemParticule;
    public int m_toEmitAtStart = 5;


    public void Push() {
        m_systemParticule.Emit(m_toEmitAtStart);


    }

    internal void SetEmitAtStart(int emitAtStart)
    {
        m_toEmitAtStart = emitAtStart;
    }
}
