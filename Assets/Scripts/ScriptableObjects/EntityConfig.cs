using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementConfig", menuName = "TP/MovementConfig", order = 3)]
public class EntityConfig : ScriptableObject
{
    [Header("Movement")]
    public float maxRayDistance = 2.5f;
    public float distanceFromCenter = 0.5f;
    public LayerMask raycastDectection;
    public float speed = 5f;

    [Header("Collision")]
    public float collisionDistance = 1f;
    public LayerMask collisionDectection;
    public float collisionCheckTimer = 0.5f;

    [Header("Shooting")]
    public float cooldownShooting = 1f;
    public BulletType bulletType;
}
