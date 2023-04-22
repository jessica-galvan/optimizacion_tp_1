using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private Rigidbody rb;
    public Animator animator;

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
            animator.SetFloat("Vel", velocity);
    }
}

