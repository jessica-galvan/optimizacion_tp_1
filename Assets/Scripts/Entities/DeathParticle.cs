using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DeathParticle : MonoBehaviour, IPoolable
{
    public ParticleSystem particle;
    public Vector3 hidePosition;

    public void Initialize(Vector3 hidePosition)
    {
        this.hidePosition = hidePosition;
        particle.Pause();
    }

    public void Spawn(Vector3 position)
    {
        transform.position = position;
        particle.Play();
    }

    public void ReturnToPool()
    {
        transform.position = hidePosition;
        particle.Pause();
    }
}
