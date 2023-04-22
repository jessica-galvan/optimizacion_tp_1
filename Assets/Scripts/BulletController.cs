using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody body;
    [SerializeField] private float speed;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        body.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        OnCollision();
    }

    private void OnCollision()
    {
        Destroy(gameObject);
    }
}
