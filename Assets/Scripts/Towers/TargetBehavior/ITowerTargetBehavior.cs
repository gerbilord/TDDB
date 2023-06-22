
using UnityEngine;

public interface ITowerTargetBehavior
{
    public ICreep getTarget(Vector3 pos, float range);
}
