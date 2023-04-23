using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "Stats/BulletData", order = 4)]
public class BulletData : ScriptableObject
{
    public LayerMask layer;
    public float speed;
}
