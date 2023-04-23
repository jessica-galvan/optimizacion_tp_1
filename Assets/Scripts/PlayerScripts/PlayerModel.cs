using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [SerializeField] private GameObject model;

    [Header("Movement")]
    public float rotationSpeed;
    public float speed;

    [Header("Shooting")]
    public Transform firepoint;
    public int maxBullets = 6;
    public float rechargeTimeInSeconds = 1f;

    private Rigidbody rb;
    private int currentBullets;
    private float currentRechargeTime = 0f;

    public Vector3 GetForward => transform.forward;
    public float GetSpeed => rb.velocity.magnitude;
    public int CurrentBullets => currentBullets;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        //jess: usualmente diria de lo que es referencia a otra cosa se hace en el start y no el awake PERO el game manager esta puesto en el script execution order para que corrar primero y el UI va a buscar esta referencia en el Start
        GameManager.Instance.SetPlayer(this);
        currentBullets = maxBullets;
    }

    public void Move(Vector3 direction) //el modelo solo recibe la dirección
    {
        Vector3 directionSpeed = direction * speed;
        directionSpeed.y = rb.velocity.y;
        rb.velocity = direction * speed;
    }

    public void LookDirection(Vector3 dir)
    {
        if (dir == Vector3.zero) return;
        dir.y = 0; //Sacar una vez que utilizemos Y
        model.transform.forward = Vector3.RotateTowards(model.transform.forward, dir, Time.deltaTime * rotationSpeed, 0f);
    }

    public void Shoot()
    {
        if (currentBullets == 0) return; //TODO: add negative sound and feedback

        var bullet = GameManager.Instance.poolManager.GetBullet(isPlayer: true);
        bullet.SetTarget(firepoint, transform.forward);
        currentBullets -= 1;
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
}

