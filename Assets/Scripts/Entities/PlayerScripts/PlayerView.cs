using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Animator animator;

    private Rigidbody rb;
    private bool hasAnimator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hasAnimator = animator != null;
    }

    //TODO cambiar a que sean eventos que escucha del PlayerModel o PlayerController en vez de correr un update
    void Update()
    {
        var velocity = rb.velocity.magnitude;

        if (hasAnimator)
            Movement(velocity);
    }

    public void Movement(float velocity)
    {
        animator.SetFloat("Vel", velocity);
    }
}

