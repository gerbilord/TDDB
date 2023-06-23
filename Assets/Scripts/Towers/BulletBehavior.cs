using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour, IBullet
{
    public Transform target;
    public Dictionary<StatType, float> stats { get; set; }
    public ITower tower { get; set; }
    public ICreep creep { get; set; }
    
    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
    
    public void Seek(ICreep _creep, ITower _tower)
    {
        //Can pass other information to bullet here
        creep = _creep;
        tower = _tower;
        target = creep.GetGameObject().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceToTravelThisFrame = stats[StatType.moveSpeed] * Time.deltaTime;

        //dir.magnitude = distance to target
        if (dir.magnitude <= distanceToTravelThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceToTravelThisFrame, Space.World);

    }

    void HitTarget()
    {
        //On-hit effect

        //Check if Critical Strike

        //Deal damage
        //creep.Damage(tower.stats[StatType.damage]);
        creep.takeBulletHit(this);


        Destroy(gameObject);
    }
}