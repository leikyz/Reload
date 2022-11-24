using Assets.Scripts.Body;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IHealth
{
    public const float MaxHealth = 500;

    [SerializeField] private Image healthBar;
    [SerializeField] private float currentHealth;


    void Start()
    {
        currentHealth = MaxHealth;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / MaxHealth;
        //Instantiate(BloodFX[Random.Range(0, BloodFX.Count)], BloodSpawn, Quaternion.Euler(BloodSpawn.x, BloodSpawn.y, BloodSpawn.z));
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void InfligeDamage(float damage)
    {
        throw new System.NotImplementedException();
    }
}
