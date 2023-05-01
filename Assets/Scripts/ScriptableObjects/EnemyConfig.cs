using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "TP/EnemyConfig", order = 2)]
public class EnemyConfig : ScriptableObject
{
    [Header("Movement")]
    public float distanceFromCenter;
    public float maxRayDistance;
    public LayerMask raycastDectection;
    public int posibleDirectionsCount = 4;
    public Vector3[] posibleDirections = new Vector3[4];

    public class EnemyStatesWeight
    {
        public EnemyStates state;
        public int weight;
    }

    [Header("Tree Weight")]
    public EnemyStatesWeight[] enemyStatesWeight;

    [Header("Times")]
    public float shootCooldown = 3f;
    public float minIdleWaitTime = 2f;
    public float maxIdleWaitTime = 5f;
}
