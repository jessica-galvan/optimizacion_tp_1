using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public Pool playerBulletPool;
    public Pool enemyBulletPool;
    public Pool enemyPool;

    public void Initialize()
    {
        playerBulletPool.Initialize();
        enemyBulletPool.Initialize();
        enemyPool.Initialize();
    }

    public BulletController GetBullet(BulletType bulletType)
    {
        IPoolable bullet = null;

        switch (bulletType)
        { 
            case BulletType.Player:
                bullet = playerBulletPool.Spawn();
                break;
            case BulletType.Enemy:
                bullet = enemyBulletPool.Spawn();
                break;
        }

        return (BulletController)bullet;
    }

    public void ReturnBullet(BulletController bullet)
    {
        IPoolable poolable = (IPoolable)bullet;
        switch (bullet.bulletData.type)
        {
            case BulletType.Player:
                playerBulletPool.BackToPool(poolable);
                break;
            case BulletType.Enemy:
                enemyBulletPool.BackToPool(poolable);
                break;
        }
    }

    public EnemyController GetEnemy()
    {
        return (EnemyController)enemyPool.Spawn();
    }

    public void ReturnEnemy(EnemyController enemy)
    {
        enemyPool.BackToPool(enemy);
    }


}
