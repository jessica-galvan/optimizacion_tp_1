using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ParticleController : MonoBehaviour, IPoolable, IUpdate
{
    public enum ParticleType
    {
        BulletImpact,
        Death
    }

    public ParticleType type;
    public ParticleSystem particle;
    public Vector3 hidePosition;

    private float maxTimeAlive;

    public void Initialize(Vector3 hidePosition)
    {
        this.hidePosition = hidePosition;
        ReturnToPool();
    }

    public void Spawn(Transform spawner)
    {
        gameObject.SetActive(true);
        transform.position = spawner.position;
        transform.rotation = spawner.rotation;
        GameManager.Instance.updateManager.gameplayCustomUpdate.Add(this);
        particle.Play();
        maxTimeAlive = GameManager.Instance.globalConfig.maxParticleLife;
    }

    public void ReturnToPool()
    {
        transform.position = hidePosition;
        particle.Pause();
        GameManager.Instance.updateManager.gameplayCustomUpdate.Remove(this);
        gameObject.SetActive(false);
    }

    public void DoUpdate()
    {
        if (!particle.isPlaying)
        {
            GameManager.Instance.poolManager.ReturnParticle(this);
        }
        else
        {
            maxTimeAlive -= Time.deltaTime;
            if (maxTimeAlive <= 0)
            {
                GameManager.Instance.poolManager.ReturnParticle(this);
            }
        }
    }
}
