using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : EntityModel
{
    [Header("Shooting")]
    public int maxBullets = 6;
    public float rechargeTimeInSeconds = 1f;

    private int currentBullets;
    private float currentRechargeTime = 0f;

    public int CurrentBullets => currentBullets;
    public bool Alive { get; private set; }

    public override void Initialize()
    {
        base.Initialize();
        rb = gameObject.GetComponentInParent<Rigidbody>();
        //jess: usualmente diria de lo que es referencia a otra cosa se hace en el start y no el awake PERO el game manager esta puesto en el script execution order para que corrar primero y el UI va a buscar esta referencia en el Start
        currentBullets = maxBullets;
    }

    public override void Spawn(GridCell spawn)
    {
        base.Spawn(spawn);
        Alive = true;
    }

    public override void Shoot()
    { 
        //if (currentBullets == 0) return;
        base.Shoot();
        //currentBullets -= 1;
    }

    public override bool ValidateCell(GridCell targetCell)
    {
        bool answer = true;

        if (currentCell != targetCell)
        {
            if (targetCell.IsOcupied && targetCell.Entity != null)
            {
                answer = false;
            }
        }

        return answer;
    }

    public void UpdateBulletCounter()
    {
        if (currentBullets == maxBullets) return;

        currentRechargeTime += Time.deltaTime;
        if (currentRechargeTime > rechargeTimeInSeconds)
        {
            currentBullets++;
            currentRechargeTime = 0f;
        }
    }

    public override void TakeDamage()
    {
        Alive = false;
        base.TakeDamage();
        //TODO in case of player respawn
    }
}

