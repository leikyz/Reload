using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    //property
    [SerializeField] private bool isChasing;
    [SerializeField] NavMeshAgent enemy;
    [SerializeField] private  Transform playerTarget;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private Animator animator;
    [SerializeField] private List<ParticleSystem> bloodFX;
    [SerializeField] private Vector3 bloodSpawn;
    [SerializeField] private Image healthBarSprites;
    [SerializeField] private float currentHealth;
    [SerializeField] private PlayerDetect playerDetect;
    private float attackLayerWeight;

    private bool isDie;

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
    public List<ParticleSystem> BloodFX
    {
        get { return bloodFX; }
        set { bloodFX = value; }
    }

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

    #region take damage

    public abstract void TakeHit(int damage); 
    
    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        healthBarSprites.fillAmount = currentHealth / maxHealth;
    }

    public abstract void Death();

    #endregion

    #region attack

    public bool CanAttack() => !isDie;
    public void Attack()
    {
        if (CanAttack()) 
        {
            if (PlayerDetect.CanAttack)
                AttackLayerWeight = 1;
            else
                AttackLayerWeight = 0;

            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), AttackLayerWeight, Time.deltaTime * 5f));
        }
       
    }

    #endregion
}
