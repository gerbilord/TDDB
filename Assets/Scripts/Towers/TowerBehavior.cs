using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior :MonoBehaviour, ITower
{
    public Dictionary<StatType, float> stats { get; set; }
    public ITowerTargetBehavior towerTargetBehavior { get; set; }
    private float fireCountdown;
    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public void Start()
    {
        fireCountdown = 1f/ stats[StatType.attackSpeed];
    }

    void Update()
    {
        if(getTarget() == null)
            return;
        
        GameObject target = getTarget().GetGameObject();

        //Shooting
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / stats[StatType.attackSpeed];
        }

        fireCountdown -= Time.deltaTime;

    }

    public ICreep getTarget()
    {
        return towerTargetBehavior.getTarget(gameObject.transform.position, stats[StatType.range]);
        
    }
    public BulletPreset bulletPreset { get; set; }
        
    
      void Shoot()
      {
          GameObject bulletObj = bulletPreset.makeBullet();
          //TODO:make it fire from the firepoint on a tower rather than the middle of the tower
          bulletObj.transform.position = GraphicsUtils.GetTopOf(this.GetGameObject());
          // bulletObj.transform.position = this.GetGameObject().transform.position;
          IBullet bullet = bulletObj.GetComponent<IBullet>();

          ICreep creepTarget = getTarget();
          if (bullet != null)
              bullet.Seek(creepTarget, this);
      }
}