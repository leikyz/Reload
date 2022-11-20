using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class ZombieBoss : Enemy
{  
    [SerializeField] private EnemyData enemyData;

    private void Start()
    {
        CurrentHealth = enemyData.maxHealth;
    }
    void Update()
    {
        if (PlayerDetect.IsInRange)
            Attack();

        if (CanChase())
            StartChase();
        else
            StopChase();

        //Debug.Log(DistanceToTarget() + " t");
        Animator.SetBool("IsDying", IsDie);
        Animator.SetFloat("Distance", DistanceToTarget());
        Animator.SetBool("IsChasing", IsChasing);

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
            TakeHit(GameObject.Find("Player").GetComponent<PlayerShooter>().Weapon.WeaponData.damage);
            UpdateHealthBar(enemyData.maxHealth, CurrentHealth);

            if (CurrentHealth <= 0)
                Death();
        }

    }


    public override void TakeHit(int damage)
    {
        CurrentHealth -= damage;
        Instantiate(BloodFX[Random.Range(0, BloodFX.Count)], BloodSpawn, Quaternion.Euler(BloodSpawn.x, BloodSpawn.y, BloodSpawn.z));
    }

    public override void Death()
    {
        //Animator.applyRootMotion = false;
        //ResetTargetChase();
        IsDie = true;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<CapsuleCollider>().enabled = false;
    }

    public override void Attack()
    {
        Debug.Log("test");
    }
}
