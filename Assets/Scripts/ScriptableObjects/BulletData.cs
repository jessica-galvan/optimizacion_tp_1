using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "TP/BulletData", order = 0)]
public class BulletData : ScriptableObject
{
    public BulletType type;
    public LayerMask targets;
    public float speed;
    public bool hasLifeTimer = true;
    public float lifeTime = 5f;

}
