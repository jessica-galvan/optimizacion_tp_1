using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum BulletType
{
    Player, 
    Enemy
}

public class BulletController : MonoBehaviour, IUpdate, IPoolable
{
    public BulletData bulletData;

    private Rigidbody body;
    private float currentLife;
    private bool isActive;
    private Vector3 hidePoint;

    public void Initialize(Vector3 hidePoint)
    {
        //here we instantiate them, hide their visuals and move them to a unseen place. 
        body = GetComponent<Rigidbody>();
        this.hidePoint = hidePoint;
        transform.position = hidePoint;
        SetVisibility(false);
    }

    public void DoUpdate()
    {
        if (GameManager.Instance.Pause) return;
        if (!isActive) return;

        body.velocity = transform.forward * bulletData.speed;

        if (bulletData.hasLifeTimer)
        {
            currentLife += Time.deltaTime;
            if (currentLife > bulletData.lifeTime)
            {
                Die();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (!MiscUtils.IsInLayerMask(other.gameObject.layer, bulletData.targets)) return;
        if (!isActive) return;

        var entity = other.gameObject.GetComponent<IDamagable>();
        if(entity != null)
        {
            entity.Die();
        }

        print($"{other.gameObject.name} triggered the bullet");
        Die();
    }

    private void Die()
    {
        print("Bullet die");
        //TODO:particle system explosion
        GameManager.Instance.poolManager.ReturnBullet(this);
    }

    public void ReturnToPool()
    {
        SetVisibility(false);
        GameManager.Instance.updateManager.gameplayCustomUpdate.Remove(this);
        transform.position = hidePoint;
    }

    public void SetTarget(Transform startingPosition, Vector3 direction)
    {
        print("bullet shooted");
        transform.position = startingPosition.position;
        transform.forward = direction;
        currentLife = 0f;
        SetVisibility(true);
        GameManager.Instance.updateManager.gameplayCustomUpdate.Add(this);
    }

    private void SetVisibility(bool value)
    {
        isActive = value;
        gameObject.SetActive(value);
    }
}
