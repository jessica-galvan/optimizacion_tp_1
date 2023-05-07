using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ParticleController : MonoBehaviour, IPoolable
{
    public ParticleSystem particle;
    public Vector3 hidePosition;

    public void Initialize(Vector3 hidePosition)
    {
        this.hidePosition = hidePosition;
        particle.Pause();
    }

    public void Spawn(Transform spawner)
    {
        transform.position = spawner.position;
        transform.rotation = spawner.rotation;
        particle.Play();
    }

    public void ReturnToPool()
    {
        transform.position = hidePosition;
        particle.Pause();
    }
}
