using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experiment_BlackWhiteTextureFromDepth : MonoBehaviour
{
    public KinectUShortDepthRef m_kinectDepthUshort;
    public Texture2D m_texture;

    public int m_width;
    public int m_height;

    private Color[] m_colors;
    private Color m_black = Color.black;
    private Color m_white = Color.white;
    public void SetRef(KinectUShortDepthRef depthRef) {
        m_kinectDepthUshort = depthRef;
    }

    [ContextMenu("Refresh Texture")]
    public void RefreshTextureFromSource() { 
    
            
    }
}

public class UShortArrayUtility {

    public static Color m_black=Color.black;
    public static Color m_white= Color.white;
    public static Color m_green= Color.green;
    public static void SetTextureWithBlackAndWhite(in ushort[] array, in int width, in int height, in ushort whiteMinValue, ref Texture2D texture)
    {

        if (array != null && array.Length > 0)
        {

            Color[] m_colors = texture.GetPixels();
            if (m_colors == null || m_colors.Length != array.Length)
            {
                m_colors = new Color[array.Length];
                texture = new Texture2D(width, height);
            }

            if (texture != null)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    m_colors[i] = array[i] > whiteMinValue ? m_white : m_black;
                }
                texture.SetPixels(m_colors);
                texture.Apply();
            }
        }
    }
    public static void SetTextureWithColorFromTo(in ushort[] array, in int width, in int height, in Color from, in Color to , in ushort max, ref Texture2D texture)
    {


        if (array != null && array.Length > 0)
        {
            if (texture == null)
                texture = new Texture2D(width, height);
            
            Color[] colors = texture.GetPixels();
            if (colors == null || colors.Length != array.Length)
            {
                colors = new Color[array.Length];
                texture = new Texture2D(width, height);
            }
            
            float maxValue = (float)max;
            if (texture != null)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    colors[i] = Color.Lerp(from, to, array[i] / maxValue);
                }
                texture.SetPixels(colors);
                texture.Apply();
            }
        }
    }


}
