using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public Pool reference;

    [ReadOnly] public Pool playerBulletPool;
    [ReadOnly] public Pool enemyBulletPool;
    [ReadOnly] public Pool enemyPool;
    [ReadOnly] public Pool deathParticlePool;
    [ReadOnly] public Pool bulletImpactParticlePool;

    public void Initialize()
    {
        playerBulletPool = Instantiate(reference, transform);
        playerBulletPool.gameObject.name = "PlayerBulletPool";
        enemyBulletPool = Instantiate(reference, transform);
        enemyBulletPool.gameObject.name = "EnemyBulletPool";
        deathParticlePool = Instantiate(reference, transform);
        deathParticlePool.gameObject.name = "DeathParticlePool";
        bulletImpactParticlePool = Instantiate(reference, transform);
        bulletImpactParticlePool.gameObject.name = "BulletImpactParticlePool";
        enemyPool = reference;
        reference.gameObject.name = "EnemyPool";

        playerBulletPool.Initialize(GameManager.Instance.prefabReferences.playerBulletPrefab.gameObject, GameManager.Instance.globalConfig.initialPoolBullet);
        enemyBulletPool.Initialize(GameManager.Instance.prefabReferences.enemyBulletPrefab.gameObject, GameManager.Instance.globalConfig.initialPoolBullet);
        enemyPool.Initialize(GameManager.Instance.prefabReferences.enemyPrefab.gameObject, GameManager.Instance.globalConfig.maxEnemiesInLevelAtAllTimes);
        deathParticlePool.Initialize(GameManager.Instance.prefabReferences.deathParticle.gameObject, GameManager.Instance.globalConfig.particlePool);
        bulletImpactParticlePool.Initialize(GameManager.Instance.prefabReferences.bulletImpactParticle.gameObject, GameManager.Instance.globalConfig.initialPoolBullet);
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

    public ParticleController GethParticle(ParticleController.ParticleType type)
    {
        ParticleController particle = null;
        switch (type)
        {
            case ParticleController.ParticleType.BulletImpact:
                particle = (ParticleController)bulletImpactParticlePool.Spawn();
                break;
            case ParticleController.ParticleType.Death:
                particle = (ParticleController)deathParticlePool.Spawn();
                break;
        }
        return particle;
    }

    public void ReturnParticle(ParticleController particle)
    {
        switch (particle.type)
        {
            case ParticleController.ParticleType.BulletImpact:
                bulletImpactParticlePool.BackToPool(particle);
                break;
            case ParticleController.ParticleType.Death:
                deathParticlePool.BackToPool(particle);
                break;
        }
    }
}
