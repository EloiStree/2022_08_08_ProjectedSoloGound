using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FridaTextureGroup
{
    public List<Texture2D> m_textures = new List<Texture2D>();
}
[CreateAssetMenu(fileName = "FridaTextureGroup", menuName = "ScriptableObjects/Frida Texture Group", order = 1)]
public class FridaTextureGroupScriptable : ScriptableObject
{
    public FridaTextureGroup m_textureGroup;

    public void GetRandomTexture(out Texture2D random)
    {
        Eloi.E_UnityRandomUtility.GetRandomOf(out random, m_textureGroup.m_textures);
    }
}

[System.Serializable]
public class NamedFridaTextureGroup 
{
    public string m_groupName= "";
    public List<Texture2D> m_textures = new List<Texture2D>();
}
