using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieBoss : Enemy
{

    [SerializeField] NavMeshAgent enemy;

    public Transform playerTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(playerTarget);
        //transform.Translate(Vector3.forward * Time.deltaTime * 2);*
        enemy.SetDestination(playerTarget.position);
    }
}
