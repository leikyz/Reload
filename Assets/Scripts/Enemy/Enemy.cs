using Assets.Scripts.Body;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Linq;

public abstract class Enemy : MonoBehaviour, IHealth
{
    //property
    [SerializeField] private bool isChasing;
    [SerializeField] NavMeshAgent enemy;
    [SerializeField] private  Transform playerTarget;
    [SerializeField] private Animator animator;
    

    [SerializeField] private Image healthBarSprites;
     
    [SerializeField] private PlayerDetect playerDetect;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private EnemyData enemyData;

    private float currentHealth;
    private float attackLayerWeight;
    private Vector3 bloodSpawn;

    private bool isDie;


    public EnemyData Data
    {
        get { return enemyData; }
        set { enemyData = value; }
    }
    public float AttackLayerWeight
    {
        get { return attackLayerWeight; }
        set { attackLayerWeight = value; }
    }

    public PlayerDetect PlayerDetect
    {
        get { return playerDetect; }
        set { playerDetect = value; }
    }
    public bool IsDie
    {
        get { return isDie; }
        set { isDie = value; }
    }
    public float CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }
    //public List<ParticleSystem> BloodFX
    //{
    //    get { return bloodFX; }
    //    set { bloodFX = value; }
    //}

    public Vector3 BloodSpawn
    {
        get { return bloodSpawn; }
        set { bloodSpawn = value; }
    }

    public NavMeshAgent EnemyNav
    {
        get { return enemy; }
        set { enemy = value; }
    }

    public Transform Target
    {
        get { return playerTarget; }
        set { playerTarget = value; }
    }

    public Animator Animator
    {
        get { return animator; }
        set { animator = value; }
    }

    public bool IsChasing
    {
        get { return isChasing; }
        set { isChasing = value; }
    }

   

    public float DistanceToTarget()
    {
        return Vector3.Distance(Target.transform.position, EnemyNav.gameObject.transform.position);
    }

    #region chase move
    public abstract bool CanChase();
    public void StartChase()
    {       
        IsChasing = true;

        enemy.SetDestination(playerTarget.position);
    }
    public void StopChase()
    {
        IsChasing = false;
    }

    public void ResetTargetChase()
    {
        enemy.Stop();
    }
    #endregion

    public abstract void Death();

    #region attack

    public bool CanAttack() => !isDie;
    public void Attack()
    {
        if (CanAttack()) 
        {
            if (PlayerDetect.IsInRange)
            {
                AttackLayerWeight = 1;
                if (animator.GetCurrentAnimatorStateInfo(0).length < 1f)
                    playerHealth.TakeDamage(Data.damage);
            }
            
            else
                AttackLayerWeight = 0;


            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), AttackLayerWeight, Time.deltaTime * 5f));
        }
       
    }
    public void UpdateHealthBar()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        healthBarSprites.fillAmount = currentHealth / Data.maxHealth;
        //Instantiate(BloodFX[Random.Range(0, BloodFX.Count)], BloodSpawn, Quaternion.Euler(BloodSpawn.x, BloodSpawn.y, BloodSpawn.z));
    }

    public void InfligeDamage(float damage)
    {
        throw new System.NotImplementedException();
    }

    #endregion
}
