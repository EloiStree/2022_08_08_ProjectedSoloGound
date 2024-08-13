using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class UdpBytesPayloadListenerThread : MonoBehaviour , IUDPByteListener
{
    public int m_portToListen=11000;
    public System.Threading.ThreadPriority m_priority = System.Threading.ThreadPriority.Normal;

    public bool m_threadStayAlive = true;
    public int m_bytesCount;
    public IUDPByteListener.PayloadListener m_listener;
    public Queue<byte[]> m_receivedOfUnityEvent= new Queue<byte[]>();

    public PayloadEvent m_onPayloadReceived;
    public byte[] m_lastReceived;
    [System.Serializable]
    public class PayloadEvent : UnityEvent<byte[]> { }
    public int m_inQueueCount;
    void OnEnable()
    {
        
        RestartServerWithPort(m_portToListen);
        InvokeRepeating("PushQueue", 0, 0.01f);
    }

    public Thread thread = null;
    
    [ContextMenu("Restart")]
    public void RestartServerWithPort()
    {
        RestartServerWithPort(m_portToListen);
    }


        public void RestartServerWithPort(int portToListent) {
        m_portToListen = portToListent;
        if (thread != null)
        {
            m_udpServer.Close();
            m_udpServer.Dispose();
            thread.Abort();
        }
        Thread.Sleep(100);
        thread = new Thread(ThreadMethode);
        thread.Priority = m_priority;
        thread.Start();
    }
    public void PushQueue() {
        while (m_receivedOfUnityEvent != null && m_receivedOfUnityEvent.Count>0) {
            byte[] p = m_receivedOfUnityEvent.Dequeue();
            Thread.Sleep(1);
            m_onPayloadReceived.Invoke(p);
        }
    }
    private void OnDestroy()
    {
        m_threadStayAlive = false;
    }

    private UdpClient m_udpServer;
    private IPEndPoint m_remoteEP;
    void ThreadMethode()
    {
         m_udpServer = new UdpClient(m_portToListen);
         m_remoteEP = new IPEndPoint(IPAddress.Any, m_portToListen);
        while (m_threadStayAlive)
        {
            byte [] receivedBytes = m_udpServer.Receive(ref m_remoteEP);
            m_bytesCount = receivedBytes.Length;
            if (m_listener!=null)
                m_listener.Invoke(in receivedBytes);
            m_receivedOfUnityEvent.Enqueue(receivedBytes);
            m_lastReceived = receivedBytes;
            m_inQueueCount = m_receivedOfUnityEvent.Count;
            Thread.Sleep(1);
        }
    }
    

    public void AddPayloadListener(IUDPByteListener.PayloadListener listener)
    {
        if (listener != null)
            m_listener += listener;
    }

    public void RemovePayloadListener(IUDPByteListener.PayloadListener listener)
    {
        if(listener!=null)
            m_listener -= listener;
    }
}
