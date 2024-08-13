using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StupideClaimGameObjectToSpawn : AbstractClaimGameObjectToSpawn
{
    public List<GameObject> m_pool = new List<GameObject>();
    public int m_index;
    public override void ClaimNewObject(out bool found, out GameObject objectClaimed)
    {
        found = m_pool.Count > 0;
        objectClaimed = null;
        if (m_pool.Count > 0)
        {
            objectClaimed = m_pool[0];
            m_index++;
            if (m_index >= m_pool.Count)
            {
                m_index = 0;
            }
            objectClaimed = m_pool[m_index];
            found = objectClaimed != null;
            if (objectClaimed != null) { 
                objectClaimed.SetActive(true);
                IClaimableItem claimable = objectClaimed.GetComponent<IClaimableItem>();
                if (claimable != null) {
                    claimable.Unclaim();
                    claimable.Claim();
                }
            }
        }
    }
    public void AddNewObjectToStupidPool(GameObject gamo)
    {
        m_pool.Add(gamo);
        IClaimableItem claimableItem = gamo.GetComponentInChildren<IClaimableItem>();
        if (claimableItem!=null)
            claimableItem.Unclaim();
    }
    public void AddNewObjectToStupidPool(GameObject [] objects)
    {
        foreach (var item in objects)
        {
            AddNewObjectToStupidPool(item);
        }
    }
    public void AddNewObjectToStupidPool(IEnumerable<GameObject> objects)
    {
        foreach (var item in objects)
        {
            AddNewObjectToStupidPool(item);
        }
    }
    public override void ManualUnclaim(in GameObject objectToUnclaim)
    {
        objectToUnclaim.SetActive(false);
        return;
    }

    public void Shuffle()
    {
        Eloi.E_UnityRandomUtility.ShuffleRef<GameObject>(ref m_pool);
    }
}
public class StupideClaimGameObjectToSpawnMono : AbstractClaimGameObjectToSpawnMono, IClaimGameObjectToSpawn
{
    public StupideClaimGameObjectToSpawn m_spawner = new StupideClaimGameObjectToSpawn();

    public  override void ClaimNewObject(out bool found, out GameObject objectClaimed)
    {
        m_spawner.ClaimNewObject(out found, out objectClaimed);
    }
    public override void ManualUnclaim(in GameObject objectToUnclaim)
    {
        m_spawner.ManualUnclaim(in objectToUnclaim);
    }

        
    public void AddNewObjectToStupidPool(GameObject gamo)
    {
        m_spawner.AddNewObjectToStupidPool(gamo);
    }
    public void AddNewObjectToStupidPool(GameObject[] objects)
    {
        m_spawner.AddNewObjectToStupidPool(objects);
    }
    public void AddNewObjectToStupidPool(IEnumerable<GameObject> objects)
    {
        m_spawner.AddNewObjectToStupidPool(objects);
    }
    public void Shuffle()
    {
        m_spawner.Shuffle();
    }
}