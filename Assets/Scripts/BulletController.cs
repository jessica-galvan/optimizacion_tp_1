using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour, IUpdate
{
    [SerializeField] private float speed;
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
        body.velocity = transform.forward * speed;

        //TODO ADD TIMER FOR LIFESPAWN????
    }

    private void OnTriggerEnter(Collider other)
    {
        moving = false;

        GameManager.Instance.updateManager.RemoveToGameplayUpdate(this);

        //TODO: instead of destroy, we re addit to the pool or something
        Destroy(gameObject);
    }

    public void SetTarget(Transform startingPosition, Vector3 direction, BulletData bulletData)
    {
        transform.position = startingPosition.position;
        transform.forward = direction;
        speed = bulletData.speed;
        moving = true;

        GameManager.Instance.updateManager.AddToGameplayUpdate(this);
        //set direction and rotation
        //set speed and layer so that it doesn't hurt the current target? 
    }
}
