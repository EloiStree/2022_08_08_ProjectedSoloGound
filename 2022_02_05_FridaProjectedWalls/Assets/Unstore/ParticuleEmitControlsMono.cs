using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticuleEmitControlsMono : MonoBehaviour
{
    public ParticleSystem m_system;
    public ParticleSystemRenderer m_render;



    public void SetEmitRateOverTime(float constValue)
    {
        var a = m_system.emission;
        a.rateOverTime = constValue;
        //var t = m_system.emission.rateOverTime;
        //t.constant = constValue;
        //m_system.emission.rateOverTime = t;
    }
    public void SetLifeTime(float timeToLiveMax)
    {
        var a = m_system.main.startLifetime ;
        a.constant = timeToLiveMax;
        //var t = m_system.emission.rateOverTime;
        //t.constant = constValue;
        //m_system.emission.rateOverTime = t;
    }

    public void SetGridX(int xCount)
    {
        //var a = new ParticleSystem.TextureSheetAnimationModule();
        //m_system.textureSheetAnimation =a;
        var ts = m_system.textureSheetAnimation;
        ts.enabled = true;
        ts.numTilesX = xCount;
        ts.rowMode = ParticleSystemAnimationRowMode.Random;
    }
    public void SetGridY(int yCount)
    {
        var ts = m_system.textureSheetAnimation;
        ts.enabled = true;
        ts.numTilesY = yCount;
        ts.rowMode = ParticleSystemAnimationRowMode.Random;
    }


    void Reset()
    {
        m_system = GetComponent<ParticleSystem>();
        m_render = GetComponent<ParticleSystemRenderer>();
    }

    public void SetGridXY(int m_gridX, int m_gridY)
    {
        SetGridX(m_gridX);
        SetGridY(m_gridY);
    }

    public void SetEmitPerSeconds(float m_emitPerSeconds)
    {
        var ts = m_system.emission;
       
        ts.enabled = m_emitPerSeconds >0;
        ts.rateOverTime = m_emitPerSeconds;
    }

    public void SetSpeed(float min, float max)
    {
        var a = m_system.main;
        a.startSpeed= new ParticleSystem.MinMaxCurve(min, max);
    }

    public void SetRotation(float negativeRotationDegree, float posifiveRotationDegree)
    {
        var a = m_system.rotationOverLifetime;
        a.enabled=true;
        a.separateAxes = true;
        //
        //a.z = new ParticleSystem.MinMaxCurve(negativeRotationDegree / 360f, posifiveRotationDegree / 360f);
        a.z = new ParticleSystem.MinMaxCurve(
            negativeRotationDegree / 57.296f, 
            posifiveRotationDegree / 57.296f);

        /*
         
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0.0f, negativeRotationDegree);
        curve.AddKey(0.75f, posifiveRotationDegree);

        AnimationCurve curve2 = new AnimationCurve();
        curve2.AddKey(0.0f, negativeRotationDegree);
        curve2.AddKey(0.5f, posifiveRotationDegree);

        a.z = new ParticleSystem.MinMaxCurve(2.0f, curve, curve2);
         */
    }

    internal void SetEmitAtStart(float m_emitPerSeconds)
    {
        throw new NotImplementedException();
    }
}
