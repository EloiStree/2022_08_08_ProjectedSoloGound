using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class TextToSHA256Mono : MonoBehaviour
{
    [TextArea(0, 5)]
    public string m_text;
    [TextArea(0, 5)]
    public string m_lastGeneratedKeyUnicode;
    [TextArea(0, 5)]
    public string m_lastGeneratedKeyUTF8;
    public void PushTextToSha256(string text) {

        m_text = text;
        m_lastGeneratedKeyUnicode = TextToSHA256.sha256_ASCII(text);
        m_lastGeneratedKeyUTF8 = TextToSHA256.sha256_UTF8(text);
    }

    [ContextMenu("Refresh from Text")]
    public void RefreshWithText() {
        PushTextToSha256(m_text);
    }
}


public class TextToSHA256
{
   public static string sha256_UTF8(string randomString)
    {
        var crypt = new System.Security.Cryptography.SHA256Managed();
        var hash = new System.Text.StringBuilder();
        byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
        foreach (byte theByte in crypto)
        {
            hash.Append(theByte.ToString("x2"));
        }
        return hash.ToString();
    }
    public static string sha256_ASCII(string randomString)
    {
        var crypt = new SHA256Managed();
        string hash = String.Empty;
        byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(randomString));
        foreach (byte theByte in crypto)
        {
            hash += theByte.ToString("x2");
        }
        return hash;
    }

}