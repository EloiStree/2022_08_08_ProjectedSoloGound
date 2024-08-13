using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyScript_CreateAndDeleteParticulesMouse : MonoBehaviour
{

    public float m_timeBetweenCreation=0.1f;
    public float m_randomRadius=0.5f;
    public float m_lifeTime=9;
    public GameObject m_createPrefab;
    public Camera m_camera;
    public Transform m_parent;
    public IEnumerator Start()
    {
        while (true) {
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(m_timeBetweenCreation);
            GameObject c = Create();

        }
    }

    private GameObject Create()
    {
        GameObject c = GameObject.Instantiate(m_createPrefab);
        Vector3 position= Input.mousePosition;
        position.z = 10;
        Vector3 p= m_camera.ScreenToWorldPoint(position, Camera.MonoOrStereoscopicEye.Mono);
        Eloi.E_UnityRandomUtility.GetRandomVector3Direction(out Vector3 r);
        r *= m_randomRadius;
        p += r;
        c.transform.position = p;
        c.transform.parent = m_parent;
        Destroy(c, m_lifeTime);
        return c;

    }
}
