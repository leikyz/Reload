using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    //property
    [SerializeField] private bool IsChasing;
    [SerializeField] NavMeshAgent enemy;

    public void ChasePlayer()
    {
        enemy.SetDestination(playerTarget.position);
    }
}
