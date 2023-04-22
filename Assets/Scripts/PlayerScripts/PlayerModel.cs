using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private GameObject model;
    public float rotationSpeed;
    public float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 direction) //el modelo solo recibe la dirección
    {
        Vector3 directionSpeed = direction * speed;
        directionSpeed.y = rb.velocity.y;
        rb.velocity = direction * speed;
    }

    public void LookDirection(Vector3 dir)
    {
        if (dir == Vector3.zero) return;
        dir.y = 0; //Sacar una vez que utilizemos Y
        model.transform.forward = Vector3.RotateTowards(model.transform.forward, dir, Time.deltaTime * rotationSpeed, 0f);
    }

    public Vector3 GetForward => transform.forward;
    public float GetSpeed => rb.velocity.magnitude;
}

