using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

    [SerializeField] private List<ParticleSystem> bloodFX;
    private void OnTriggerEnter(Collider other)
    {
        //BloodSpawn = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
        //if (other.gameObject.CompareTag("Enemy"))
        //{
        //    TakeDamage(GameObject.Find("Player").GetComponent<PlayerShooter>().Weapon.WeaponData.damage);

        //    if (CurrentHealth <= 0)
        //        Death();
        //}

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
