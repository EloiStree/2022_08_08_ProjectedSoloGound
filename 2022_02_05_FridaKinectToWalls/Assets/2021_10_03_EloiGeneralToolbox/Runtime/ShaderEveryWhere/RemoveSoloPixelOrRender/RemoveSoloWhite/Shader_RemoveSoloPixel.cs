using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shader_RemoveSoloPixel : MonoBehaviour
{
    public RenderTexture m_lastTarget;
    public ComputeShader m_computeShader;
    public int m_border = 2;
    public int m_minNeighbour = 3;
    public float m_percenToBeWhite=0.5f;
    public Color m_colorForNotValide;
    public void RemoveSoloPixel(in RenderTexture result)
    {
        m_lastTarget = result;

        int kernel = m_computeShader.FindKernel("CSMain");

        m_computeShader.SetTexture(kernel, "Result", result);
        m_computeShader.SetInt("m_border", m_border);
        m_computeShader.SetInt("m_height", result.height);
        m_computeShader.SetInt("m_width", result.width);
        m_computeShader.SetFloat("m_percentToBeWhite", m_percenToBeWhite);
        m_computeShader.SetFloats("m_colorToReplaceWith",
            m_colorForNotValide.r,
            m_colorForNotValide.g,
            m_colorForNotValide.b,
            m_colorForNotValide.a);
        m_computeShader.SetInt("m_minNeighbour", m_minNeighbour);
        m_computeShader.Dispatch(kernel, result.width/ 8,result.height/ 8, 1);

    }
}
