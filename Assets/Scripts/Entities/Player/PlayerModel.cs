using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : EntityModel
{
    public Vector3 spawnLookDirection = new Vector3(0, 0, 1);
    public bool Alive { get; private set; }

    public override void Spawn(GridCell spawn)
    {
        base.Spawn(spawn);
        Alive = true;
        LookDirection(spawnLookDirection);
    }

    public override void TakeDamage()
    {
        Alive = false;
        base.TakeDamage();
        AudioManager.instance.PlayPlayerSound(AudioManager.instance.soundReferences.playerDeath);
        gameObject.SetActive(false);
    }

    public override void Shoot()
    {
        if (!canShoot)
        {
            AudioManager.instance.PlayPlayerSound(AudioManager.instance.soundReferences.negativeShootSound);
            return;
        }
        base.Shoot();
        AudioManager.instance.PlayPlayerSound(AudioManager.instance.soundReferences.playerShoot);
    }


#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(firepoint.position, firepoint.position + (transform.forward * entityConfig.maxRayDistance));
    }
#endif
}

