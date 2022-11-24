using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class ZombieBoss : Enemy
{  
    private void Start()
    {
        CurrentHealth = Data.maxHealth;
    }

    void Update()
    {
        Attack();

        if (CanChase())
            StartChase();
        else
            StopChase();

        //Debug.Log(DistanceToTarget() + " t");
        Animator.SetBool("IsDying", IsDie);
        Animator.SetFloat("Distance", DistanceToTarget());
        Animator.SetBool("IsChasing", IsChasing);
        Animator.SetBool("IsAttacking", PlayerDetect.IsInRange);

    }
    public override bool CanChase()
    {
       return DistanceToTarget() < 40;
    }

    private void OnTriggerEnter(Collider other)
    {
        BloodSpawn = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
        if (other.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(GameObject.Find("Player").GetComponent<PlayerShooter>().Weapon.WeaponData.damage);

            if (CurrentHealth <= 0)
                Death();
        }

    }



    public override void Death()
    {
        //Animator.applyRootMotion = false;
        ResetTargetChase();
        IsDie = true;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<CapsuleCollider>().enabled = false;
    } 
}
