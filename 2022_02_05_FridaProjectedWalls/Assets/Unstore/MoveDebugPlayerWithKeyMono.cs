using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDebugPlayerWithKeyMono : MonoBehaviour
{
    public KeyCode m_forward= KeyCode.UpArrow;
    public KeyCode m_backward = KeyCode.DownArrow;
    public KeyCode m_left = KeyCode.LeftArrow;
    public KeyCode m_right = KeyCode.RightArrow;

    public Transform m_target;
    public float m_speed=1f;

    void Update()
    {

        Vector3 p = m_target.position;
        if (Input.GetKey(m_forward))
            p.z += Time.deltaTime * m_speed;
        if (Input.GetKey(m_backward))
            p.z -= Time.deltaTime * m_speed;
        if (Input.GetKey(m_right))
            p.x += Time.deltaTime * m_speed;
        if (Input.GetKey(m_left))
            p.x -= Time.deltaTime * m_speed;
        m_target.position = p;

    }

    private void Reset()
    {
        m_target = transform;
    }
}
