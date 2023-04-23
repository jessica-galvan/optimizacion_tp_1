using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//TODO implement a pool manager for bullets AND enemies.
public class PoolManager : MonoBehaviour
{
    public BulletController playerBulletPrefab;
    public BulletController enemyBulletPrefab;
    private Transform bulletContainer;

    private void Awake()
    {
        GameObject obj = new GameObject();
        obj.name = "BulletPool";
        bulletContainer = obj.transform;
    }

    public BulletController GetBullet(bool isPlayer = false)
    {
        //technically here it should get and exhisting bullet from the pool.. but. well. it's not done yet
        var auxPrefab = isPlayer ?  playerBulletPrefab : enemyBulletPrefab;
        BulletController bullet = Instantiate(auxPrefab, bulletContainer);
        bullet.Initialize();
        return bullet;
    }
}
