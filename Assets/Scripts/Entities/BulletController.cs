using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum BulletType
{
    Player, 
    Enemy
}

public class BulletController : MonoBehaviour, IUpdate
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
        isActive = false;
        this.hidePoint = hidePoint;
        transform.position = hidePoint;
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
                ReturnToPool();
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

        //TODO:particle system explosion

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        isActive = false;
        GameManager.Instance.updateManager.gameplayCustomUpdate.Remove(this);
        transform.position = hidePoint;
        //Destroy(gameObject);
        //TODO: instead of destroy, we re addit to the pool or something
    }

    public void SetTarget(Transform startingPosition, Vector3 direction)
    {
        transform.position = startingPosition.position;
        transform.forward = direction;
        currentLife = 0f;
        isActive = true;

        GameManager.Instance.updateManager.gameplayCustomUpdate.Add(this);
    }
}
