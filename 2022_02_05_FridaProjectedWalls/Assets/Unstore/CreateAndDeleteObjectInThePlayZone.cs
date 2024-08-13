using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAndDeleteObjectInThePlayZone : MonoBehaviour
{
    public Experiment_DrawLinePlayZonePointsMono m_playZone;
    public float m_timeBetweenCreation = 0.1f;
    public float m_creationMultiplicator = 2;
    public float m_toCreatePerGroup = 5;
    public float m_groupOfPixel= 100;
    public int m_maxCreationPerCall=60;
    public int m_minPixelToCreate = 300;
    public AbstractClaimGameObjectToSpawnMono m_claimPool;
    public Transform m_parent;

    public void SetCreationMultiplicator(float multiplicator)
    {
        m_creationMultiplicator = multiplicator;
    }
    public void SetTimeBetweenCreationWave(float timeBetweenWave)
    {
        m_timeBetweenCreation = timeBetweenWave;
    }

    public void SetGroupOfPixelCount(int pixelCount) => m_groupOfPixel = pixelCount;
    public void SetCreationPerPixelGroup(int toCreateCount) => m_toCreatePerGroup = toCreateCount;
    public void SetGroupOfPixelCount(float pixelCount) => m_groupOfPixel = pixelCount;
    public void SetCreationPerPixelGroup(float toCreateCount) => m_toCreatePerGroup = toCreateCount;
    public void SetMaxCreationPerCall(int max) => m_maxCreationPerCall = max;

    public IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(m_timeBetweenCreation);
            m_playZone.GetValidePointCount(out int count);
            if (count > m_minPixelToCreate) { 
            int c = (int)(((float)count) / (m_groupOfPixel * m_toCreatePerGroup));
            int max = m_maxCreationPerCall;
            if (c == 0)
                c = 1;

            for (int j = 0; j < m_creationMultiplicator; j++)
            {
                if (max < 1)
                {
                    break;
                }
                for (int i = 0; i < c; i++)
                {
                    if (max <1)
                    {
                        break;
                    }
                    else
                    {
                        Create();
                        max--;
                    }
                }
               
            }
            }
        }
    }

    private GameObject Create()
    {
        m_playZone.GetPointsCount(out int count);
        if (count > 0)
        {
            m_claimPool.ClaimNewObject(out bool found, out GameObject c);
            if (found) { 
                GetRandomPoint(out Vector3 p);
                c.transform.position = p;
                c.transform.parent = m_parent;
                return c;
            }
        }
        return null;
    }
    public void GetRandomPoint(out Vector3 point) {
        m_playZone.GetRandomPoint( out  point);

    }

}
