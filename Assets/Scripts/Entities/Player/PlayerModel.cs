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
        rb = gameObject.GetComponentInParent<Rigidbody>();
        //jess: usualmente diria de lo que es referencia a otra cosa se hace en el start y no el awake PERO el game manager esta puesto en el script execution order para que corrar primero y el UI va a buscar esta referencia en el Start
    }

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
        gameObject.SetActive(false);
    }
}

