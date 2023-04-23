using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BulletController : MonoBehaviour, IUpdate
{
    public BulletData bulletData;

    private Rigidbody body;
    private bool moving;
    private float currentLife;

    public void Initialize()
    {
        body = GetComponent<Rigidbody>();
        moving = false;
        //here we shoild instantiate them, hide their visuals and move them to a unseen place. 
    }

    public void DoUpdate()
    {
        if (GameManager.Instance.Pause) return;
        if (!moving) return;

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
        if (!MiscUtils.IsInLayerMask(other.gameObject.layer, bulletData.targets)) return;

        //TODO: destroy target && particle system explosion

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        moving = false;
        GameManager.Instance.updateManager.RemoveToGameplayUpdate(this);
        Destroy(gameObject);
        //TODO: instead of destroy, we re addit to the pool or something
    }

    public void SetTarget(Transform startingPosition, Vector3 direction)
    {
        transform.position = startingPosition.position;
        transform.forward = direction;
        currentLife = 0f;
        moving = true;

        GameManager.Instance.updateManager.AddToGameplayUpdate(this);
    }
}
