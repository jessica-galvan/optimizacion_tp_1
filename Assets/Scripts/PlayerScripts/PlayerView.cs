using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private Rigidbody rb;
    public Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var velocity = rb.velocity.magnitude;
        if (animator != null)
            animator.SetFloat("Vel", velocity);
    }
}

