using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BulletController : MonoBehaviour, IUpdate
{
    public BulletData bulletData;

    private Rigidbody body;
    private bool moving;

    public void Initialize()
    {
        moving = false;
        body = GetComponent<Rigidbody>();
        //here we shoild instantiate them, hide their visuals and move them to a unseen place. 
    }

    public void DoUpdate()
    {
        if (!moving) return;
        body.velocity = transform.forward * bulletData.speed;

        //TODO ADD TIMER FOR LIFESPAWN????
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!MiscUtils.IsInLayerMask(other.gameObject.layer, bulletData.targets)) return;

        moving = false;

        GameManager.Instance.updateManager.RemoveToGameplayUpdate(this);

        //TODO: destroy target && particle system explosion

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        //TODO: instead of destroy, we re addit to the pool or something
        Destroy(gameObject);
    }

    public void SetTarget(Transform startingPosition, Vector3 direction)
    {
        transform.position = startingPosition.position;
        transform.forward = direction;
        moving = true;

        GameManager.Instance.updateManager.AddToGameplayUpdate(this);
    }
}
