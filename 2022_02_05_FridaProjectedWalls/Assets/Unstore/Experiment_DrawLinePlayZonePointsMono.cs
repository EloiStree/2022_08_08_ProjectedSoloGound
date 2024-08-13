using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Experiment_DrawLinePlayZonePointsMono : MonoBehaviour
{
    public BlackWhiteTextureRepulsionQuadMono m_source;

    public int m_pixelsCount;
    public List<int> m_pixelsIndexFound= new List<int>();
    public bool m_useDebugDraw;
    public float m_height = 0.05f;
    public float m_alonePixelDistanceInMeter = 0.2f;
    public void Update()
    {
        if (Application.isEditor &&  m_useDebugDraw)
        {
            for (int i = 0; i < m_worldPosition.Length; i++)
            {
                if (m_worldPositionValide[i]>-1) 
                Debug.DrawLine(m_worldPosition[i], m_worldPosition[i] + (Vector3.up * m_height));
            }
        }
    }


    public void GetWorldPoints(out Vector3[] worldPointsOfPixels) {
        worldPointsOfPixels = m_worldPosition;
    }
    public void GetClampNumberOfRandomWorldPoints(int count, out Vector3[] points) {
        GetClampNumberOfRandomWorldPoints(count, out List<Vector3> l);
        points = l.ToArray();
    }

    public void GetPointsCount(out int numberOfPoints) => numberOfPoints = m_worldPosition.Length;
    public void GetRandomPoint(out Vector3 point)
    {
        if (m_pixelsIndexFound.Count > 0)
        {
            Eloi.E_UnityRandomUtility.GetRandomOf(out int i, m_pixelsIndexFound);
            point = m_worldPosition[i];
        }
        else point = Vector3.zero;
    }

    public void GetValidePointCount(out int count)
    {
        count = m_pixelsIndexFound.Count;
    }

    public void GetClampNumberOfRandomWorldPoints( int count, out List<Vector3> points) {

        points = new List<Vector3>();
        if (m_pixelsIndexFound.Count > 0) { 
            for (int i = 0; i < count; i++)
            {
                Eloi.E_UnityRandomUtility.GetRandomOf(out int j, m_pixelsIndexFound);
                points.Add(m_worldPosition[j]);
            }
        }

    }

    public float m_hasColor = 0.1f;


    public ComputeShader m_computeShader;
    public Vector3[] m_worldPosition = new Vector3[0];
    public ComputeBuffer m_worldPositionBuffer;

    public int[] m_worldPositionValide;
    public ComputeBuffer m_worldPositionValideBuffer;

    public int width;
    public int height;
    public int pixel1DLenght;
    public float m_distanceHalfWidth;
    public float m_distanceHalfHeight;
    public float m_distancePerPixelWidth;
    public float m_distancePerPixelHeight;
    public float m_apparitionHeight=0.5f;
    public void RefreshWorldPosition()
    {
        if (m_source == null) return;
        Texture target = m_source.GetTextureUsed();
        if (target == null) return;
        Transform rootCenter = m_source.GetZoneToUse();
        Vector3 rootPosition = rootCenter.position;
        Quaternion rootQuaterion = rootCenter.rotation;

        //Texture2D target = m_source.m_textureToUse;
        //if(target.width != m_scaleWidth || target.height != m_scaleHeight  )
        //TextureScale.Scale(target, m_scaleWidth, m_scaleHeight);
         width = m_source.GetTextureWidth();
         height = m_source.GetTextureHeight();
         pixel1DLenght = width * height;

        if (m_worldPosition.Length != pixel1DLenght) {

            if (m_worldPositionBuffer != null) m_worldPositionValideBuffer.Dispose();
            m_worldPosition = new Vector3[pixel1DLenght];
            m_worldPositionBuffer = new ComputeBuffer(pixel1DLenght, sizeof(float) * 3);
            m_worldPositionBuffer.SetData(m_worldPosition);
            if (m_worldPositionValideBuffer != null) m_worldPositionValideBuffer.Dispose();
            m_worldPositionValide = new int[pixel1DLenght];
            m_worldPositionValideBuffer = new ComputeBuffer(pixel1DLenght, sizeof(int));
            m_worldPositionValideBuffer.SetData(m_worldPositionValide);
        }

        m_distanceHalfWidth= rootCenter.lossyScale.x / 2f;
        m_distanceHalfHeight= rootCenter.lossyScale.z / 2f;
        m_distancePerPixelWidth= (rootCenter.lossyScale.x) / (float)width;
        m_distancePerPixelHeight = (rootCenter.lossyScale.z) / (float)height;

        int index = m_computeShader.FindKernel("CSMain");
        m_computeShader.SetTexture(index, "Result", target);
        m_computeShader.SetBuffer(index, "WorldPosition", m_worldPositionBuffer);
        m_computeShader.SetBuffer(index, "WorldPositionValide", m_worldPositionValideBuffer);
        m_computeShader.SetFloats("m_rootPosition", rootPosition.x, rootPosition.y, rootPosition.z);
        m_computeShader.SetFloats("m_rootRotation", rootQuaterion.x, rootQuaterion.y, rootQuaterion.z, rootQuaterion.w);
        m_computeShader.SetInt("m_width", width);
        m_computeShader.SetFloat("m_apparitionHeight", m_apparitionHeight);
        
        m_computeShader.SetFloat("m_distanceHalfWidth", m_distanceHalfWidth);
        m_computeShader.SetFloat("m_distanceHalfHeight", m_distanceHalfHeight);
        m_computeShader.SetFloat("m_distancePerPixelWidth", m_distancePerPixelWidth);
        m_computeShader.SetFloat("m_distancePerPixelHeight", m_distancePerPixelHeight);
        m_computeShader.SetFloat("m_isColorPercent", m_hasColor);
        m_computeShader.Dispatch(index, width/ 16, height / 16, 1);
        m_worldPositionBuffer.GetData(m_worldPosition);
        m_worldPositionValideBuffer.GetData(m_worldPositionValide);
        m_pixelsIndexFound.Clear();
        for (int i = 0; i < m_worldPositionValide.Length; i++)
        {
            if (m_worldPositionValide[i] > -1)
                m_pixelsIndexFound.Add(i);
        }
       

        //for (int i = 0; i < m_worldPosition.Count; i++)
        //{
        //    bool hasNeibourg = false;
        //    for (int j = 0; j < m_worldPosition.Count; j++)
        //    {
        //        if (i != j && Vector3.Distance(m_worldPosition[i], m_worldPosition[j]) < m_alonePixelDistanceInMeter)
        //        {
        //            hasNeibourg = true;
        //            break;
        //        }
        //    }
        //    if (!hasNeibourg)
        //        indexToRemove.Add(i);
        //}
        //for (int i = indexToRemove.Count - 1; i >= 0; i--)
        //{
        //    m_worldPosition.RemoveAt(indexToRemove[i]);
        //}

        m_pixelsCount = m_pixelsIndexFound.Count;


    }
    //public void RefreshWorldPosition_VOld()
    //{
    //    m_worldPosition.Clear();
    //    Transform rootCenter = m_source.GetZoneToUse();
    //    m_pixelsIndexFound.Clear();
    //    m_pixelsCount = 0;
    //    Texture2D target = m_source.m_textureToUse;
    //    TextureScale.Scale(target, m_scaleWidth, m_scaleHeight);
    //    int width = target.width;
    //    int height = target.height;

    //    int x = 0, y = 0;
    //    Color[] colors = target.GetPixels();
    //    for (int i = 0; i < colors.Length; i++)
    //    {
    //        if (colors[i].r > m_hasColor || colors[i].g > m_hasColor || colors[i].b > m_hasColor)
    //        {
    //            m_pixelsIndexFound.Add(i);
    //            GetPixel2D(in i, in width, out x, out y);
    //            GetPixel2DToPercent(in x, in y, in width, in height, out float xPct, out float yPct);
    //            GetBotTopWorldSpaceFromPixels(xPct, yPct, in rootCenter, out Vector3 position);
    //            m_worldPosition.Add(position);
    //        }
    //        else if (m_removeBlack) colors[i].a = 0;
    //    }
    //    target.SetPixels(colors);
    //    target.Apply();
    //    indexToRemove.Clear();
    //    for (int i = 0; i < m_worldPosition.Count; i++)
    //    {
    //        bool hasNeibourg = false;
    //        for (int j = 0; j < m_worldPosition.Count; j++)
    //        {
    //            if (i != j && Vector3.Distance(m_worldPosition[i], m_worldPosition[j]) < m_alonePixelDistanceInMeter)
    //            {
    //                hasNeibourg = true;
    //                break;
    //            }
    //        }
    //        if (!hasNeibourg)
    //            indexToRemove.Add(i);
    //    }
    //    for (int i = indexToRemove.Count - 1; i >= 0; i--)
    //    {
    //        m_worldPosition.RemoveAt(indexToRemove[i]);
    //    }

    //    m_pixelsCount = m_pixelsIndexFound.Count;


    //}
    //public List<int> indexToRemove = new List<int>();


    //private void GetPixel2DToPercent(in int x, in int y, in int width, in int height, out float xPct, out float yPct)
    //{
    //    xPct = x / (float)(width-1);
    //    yPct = y / (float)(height-1);
    //}

    //private void GetBotTopWorldSpaceFromPixels( float xPct,  float yPct, in Transform rootCenter, out Vector3 worldPosition)
    //{
    //    xPct -= 0.5f;
    //    yPct -= 0.5f;
    //    Vector3 local = new Vector3(xPct,0, yPct);
    //    worldPosition= rootCenter.TransformPoint(local);
    //}

    //private void GetPixel2D(in int i, in int width, out int x, out int y)
    //{
    //    x = (int)(i % width);
    //    y = (int)(i / width);
    //}
}
