
using UnityEngine;

public interface ITowerTargetBehavior: IHasIGameEngine
{
    public ICreep getTarget(Vector3 pos, float range);
}
