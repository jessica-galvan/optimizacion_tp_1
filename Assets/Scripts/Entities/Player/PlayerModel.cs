using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : EntityModel
{
    public Vector3 spawnLookDirection = new Vector3(0, 0, 1);
    public bool Alive { get; private set; }

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Spawn(GridCell spawn)
    {
        base.Spawn(spawn);
        Alive = true;
        LookDirection(spawnLookDirection);
    }

    public override void Move(Vector3 direction)
    {
        //if (!CanMoveFoward()) return;
        rb.velocity = direction * entityConfig.speed;
    }

    public override void TakeDamage()
    {
        Alive = false;
        base.TakeDamage();
        gameObject.SetActive(false);
    }
}

