using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClaimGameObjectToSpawn {
    void ClaimNewObject(out bool found, out GameObject objectClaimed);
    void ManualUnclaim(in GameObject objectToUnclaim);
}
public abstract class AbstractClaimGameObjectToSpawn : IClaimGameObjectToSpawn
{
    public abstract void ClaimNewObject(out bool found, out GameObject objectClaimed);
    public abstract void ManualUnclaim(in GameObject objectToUnclaim);
}
public abstract class AbstractClaimGameObjectToSpawnMono :MonoBehaviour, IClaimGameObjectToSpawn
{
    public abstract void ClaimNewObject(out bool found, out GameObject objectClaimed);
    public abstract void ManualUnclaim(in GameObject objectToUnclaim);
}

