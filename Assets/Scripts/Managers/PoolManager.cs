using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public BulletController bulletPrefab;

    private Transform bulletContainer;

    private void Awake()
    {
        GameObject obj = new GameObject();
        obj.name = "BulletPool";
        bulletContainer = obj.transform;
    }

    //TODO implement a pool manager for bullets AND enemies.
    public BulletController GetBullet()
    {
        //technically here it should get and exhisting bullet from the pool.. but. well. there
        var bullet = Instantiate(bulletPrefab, bulletContainer);
        bullet.Initialize();
        return bullet;
    }
}
