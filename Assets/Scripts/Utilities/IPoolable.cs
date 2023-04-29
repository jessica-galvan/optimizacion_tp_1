using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable 
{
    GameObject gameObject { get; }
    void Initialize(Vector3 hidePosition);
    void ReturnToPool();
}
