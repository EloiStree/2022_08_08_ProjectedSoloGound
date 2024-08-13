using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ImportImagePerGroupMono : MonoBehaviour
{

    public Eloi.AbstractMetaAbsolutePathDirectoryMono m_whereToFindImage;

    public List<string> m_allFiles = new List<string>();

    public List<NamedFridaTextureGroup> m_fridaTextureGroup = new List<NamedFridaTextureGroup>();

    public void GetAllTextures(out List<Texture2D> textures)
    {
        textures = new List<Texture2D>();
        for (int i = 0; i < m_fridaTextureGroup.Count; i++)
        {
            textures.AddRange(m_fridaTextureGroup[i].m_textures);
        }
       
    }
    public void GetTexturesInGroup(in string folderName,out  List<Texture2D> textures)
    {
        textures = new List<Texture2D>();
        for (int i = 0; i < m_fridaTextureGroup.Count; i++)
        {
            if(Eloi.E_StringUtility.AreEquals(m_fridaTextureGroup[i].m_groupName, folderName,true, true))
            textures.AddRange(m_fridaTextureGroup[i].m_textures);
        }

    }

    public void CreateFolderToReceiveImage(string folderName) {
        Eloi.E_FilePathUnityUtility.MeltPathTogether(out string dir, m_whereToFindImage.GetPath(), folderName);
        Directory.CreateDirectory(dir);
    }

    public List<string> m_categoryFound = new List<string>();
    [ContextMenu("Import")]
    public void Import() {
        m_categoryFound.Clear();
        Eloi.E_FileAndFolderUtility.CreateFolderIfNotThere(m_whereToFindImage.GetPath());
        string[] png = Directory.GetFiles(m_whereToFindImage.GetPath(), "*.png", SearchOption.AllDirectories);
        string[] jpg = Directory.GetFiles(m_whereToFindImage.GetPath(), "*.jpg", SearchOption.AllDirectories);
        string[] jpeg = Directory.GetFiles(m_whereToFindImage.GetPath(), "*.jpeg", SearchOption.AllDirectories);
        m_allFiles.Clear();
        m_allFiles.AddRange(png);
        m_allFiles.AddRange(jpg);
        m_allFiles.AddRange(jpeg);

        for (int i = 0; i < m_allFiles.Count; i++)
        {

           

            Texture2D texture=null;
            Eloi.IMetaAbsolutePathFileGet file = new Eloi.MetaAbsolutePathFile(m_allFiles[i]);
            bool succedImport = false;
            try
            {
                Eloi.E_FileAndFolderUtility.ImportTexture(file, out  texture);
                succedImport = true;

            }
            catch (Exception) { }
            if (succedImport && texture!=null) {
                Eloi.E_FileAndFolderUtility.SplitInfoAsString(file, 
                    out string directoryName,
                    out string fileName,
                    out string fileExtension);
                Eloi.E_FilePathUnityUtility.GetJustDirectoryName(in directoryName, out string dirName);
                texture.name = dirName+">"+fileName;
                string categoryName = dirName;
                m_categoryFound.Add(categoryName);
                GetGroup(categoryName, in m_fridaTextureGroup, out bool found, out NamedFridaTextureGroup group);
                if (found)
                {
                    group.m_textures.Add(texture);
                }
                else {
                    NamedFridaTextureGroup g = new NamedFridaTextureGroup();
                m_fridaTextureGroup.Add(g);
                    g.m_groupName = dirName;
                    g.m_textures.Add(texture);
                
                }
            }
        }
        Eloi.E_GeneralUtility.RemoveDouble(ref m_categoryFound);

    }

    private void GetGroup(in string nameToSearch, in List<NamedFridaTextureGroup> givenGroups, out bool found, out NamedFridaTextureGroup group)
    {
        found = false;
        group = null;
        for (int i = 0; i < givenGroups.Count; i++)
        {
            if (Eloi.E_StringUtility.AreEquals(givenGroups[i].m_groupName, nameToSearch)) {
                found = true;
                group = givenGroups[i];
                return;
            }
        }
    }
}
