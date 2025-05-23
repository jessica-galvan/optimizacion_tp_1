using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "TP/EnemyConfig", order = 2)]
public class EnemyConfig : ScriptableObject
{
    [Header("Collision")]
    public LayerMask collisionDectection;
    public float collisionRadious = 1f;
    public float preCollisionDetection = 2f;
    public Vector3 collisionBox = new Vector3(1, 1, 1);
    public Vector3 precollisionBox = new Vector3(1.5f, 1.5f, 1.5f);
    public Vector3 offset;

    [Header("Movement")]
    public List<Vector3> posibleDirections = new List<Vector3>(); 
    public List<Vector3> PosibleDirections => posibleDirections;


[Serializable]
    public class EnemyStatesWeight
    {
        public EnemyStates state;
        public int weight;
    }

    [Header("Tree Weight")]
    public EnemyStatesWeight[] enemyStatesWeight;

    [Header("Times")]
    public float minIdleWaitTime = 2f;
    public float maxIdleWaitTime = 5f;

    [Header("Performance")]
    public bool enemyColliderSlicesFrames = true;
    public int slicesColliderQuantity = 2;
    public int slicesCellLocationCheckQuantity = 15;
}

