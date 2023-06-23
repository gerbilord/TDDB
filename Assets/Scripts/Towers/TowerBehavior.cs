using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour, ITower
{
    public Dictionary<StatType, float> stats { get; set; }
    public ITowerTargetBehavior towerTargetBehavior { get; set; }
    private float _fireCountdown;

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public void Start()
    {
        _fireCountdown = 1f/ stats[StatType.attackSpeed];
    }

    void Update()
    {
        if(getTarget() == null)
            return;

        //Shooting
        if (_fireCountdown <= 0f)
        {
            Shoot();
            _fireCountdown = 1f / stats[StatType.attackSpeed];
        }

        _fireCountdown -= Time.deltaTime;

    }

    public ICreep getTarget()
    {
        return towerTargetBehavior.getTarget(gameObject.transform.position, stats[StatType.range]);
        
    }
    public BulletPreset bulletPreset { get; set; }
        
    
      private void Shoot()
      {
          IBullet bullet = bulletPreset.makeBullet();
          //TODO:make it fire from the firepoint on a tower rather than the middle of the tower
          bullet.GetGameObject().transform.position = GraphicsUtils.GetTowerShootSpawnPoint(this);

          ICreep creepTarget = getTarget();

          if (creepTarget != null)
              bullet.Seek(creepTarget, this);
      }
}