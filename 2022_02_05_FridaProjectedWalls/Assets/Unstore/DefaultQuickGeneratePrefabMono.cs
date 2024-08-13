using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DefaultQuickGeneratePrefabMono : MonoBehaviour
{

    public Transform m_where;
    public GameObject m_what;


    public UnityEvent m_newElementGenerated;
    public CreateGameObject m_created;
    [System.Serializable]
    public class CreateGameObject : UnityEvent<GameObject> { 
    
    }

    public int m_groupGeneration = 100;
    public float m_minSize = 0.1f;
    public float m_maxSize = 0.6f;

    public void GenerateOneElement(bool notify)
    {

        GenerateOneElement();

        if (notify)
            m_newElementGenerated.Invoke();
    }

    public void SetMinSize(float minSizeInMeter) { m_minSize = minSizeInMeter; }
    public void SetMaxSize(float maxSizeInMeter) { m_maxSize = maxSizeInMeter; }

    public float m_minZRandomness=0.1f;
    public GameObject GenerateOneElement()
    {
        GameObject created = GameObject.Instantiate(m_what);
        created.SetActive(true);
        created.transform.parent = m_where;
        created.transform.position = m_where.position;
        Eloi.E_UnityRandomUtility.GetRandomN2M(-m_minZRandomness, m_minZRandomness, out float randomZ);
        created.transform.Translate(Vector3.up * randomZ, Space.Self);
        Eloi.E_UnityRandomUtility.GetRandomN2M(in m_minSize, in m_maxSize, out float size);
        created.transform.localScale = Vector3.one * size;
        m_created.Invoke(created);
        return created;
    }

    public void NotifyElementsGenerated()
    {
            m_newElementGenerated.Invoke();
    }

    [ContextMenu("Generate")]
    public void Generate()
    {
        GenerateOneElement(true);
    }
    [ContextMenu("Generate Multiple")]
    public void GenerateByDefaultGroup() {
        Generate(m_groupGeneration,true);
    }
    public void Generate(int number, bool notify=true)
    {
        for (int i = 0; i < number; i++)
        {
            GenerateOneElement(false);
        }
        if (notify)
            m_newElementGenerated.Invoke();
    }
}
