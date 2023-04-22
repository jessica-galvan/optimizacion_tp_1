using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public float rotationSpeed;
    public float speed;

    [SerializeField] private GameObject model;
    private Rigidbody rb;

    public Vector3 GetForward => transform.forward;
    public float GetSpeed => rb.velocity.magnitude;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        //jess: usualmente diria de lo que es referencia a otra cosa se hace en el start y no el awake PERO el game manager esta puesto en el script execution order para que corrar primero y el UI va a buscar esta referencia en el Start
        GameManager.instance.SetPlayer(this);
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
}

