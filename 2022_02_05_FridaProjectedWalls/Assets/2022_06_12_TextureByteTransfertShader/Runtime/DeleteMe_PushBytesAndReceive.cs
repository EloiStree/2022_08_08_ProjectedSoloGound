using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeleteMe_PushBytesAndReceive : MonoBehaviour
{

    public MemoryFileConnectionMono m_connection;
    public BytesEvent m_fetchBytes;
    public byte[] m_connectionBytes;

    [System.Serializable]
    public class BytesEvent : UnityEvent<byte[]> { }


    public void PushAndReceive(byte [] bytes) {

        if (m_connectionBytes == null || 
            m_connectionBytes.Length != bytes.Length)
            m_connectionBytes = new byte[bytes.Length];

        m_connection.SetAsBytes(bytes);
        m_connection.Connection.GetAsBytes(out m_connectionBytes);
        m_fetchBytes.Invoke(m_connectionBytes);
    }

    

}
