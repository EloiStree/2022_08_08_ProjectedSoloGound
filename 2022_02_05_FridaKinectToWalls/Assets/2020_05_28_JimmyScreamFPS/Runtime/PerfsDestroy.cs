using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfsDestroy : MonoBehaviour
{

    public GameObject m_prefab;
    public Transform m_where;
    public float m_timeOfDestruction=20f;
    public bool m_useMouse=true;
    public void Update()
    {
        if (m_useMouse) { 
            if (Input.GetMouseButtonDown(0))
            {
                CreateAnInstance();
            }
            if (Input.GetMouseButton(1))
            {

                CreateAnInstance();
            }
        }
    }

    public void CreateAnInstance()
    {
        GameObject gamo = GameObject.Instantiate(m_prefab, m_where.position, m_where.rotation);
        Renderer [] renderer = gamo. GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderer.Length; i++)
        {
            if (renderer[i] != null && renderer[i].material != null)
                renderer[i].material.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);

        }
        Destroy(gamo, m_timeOfDestruction);
    }
}
