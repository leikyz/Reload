using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickingWeapon : MonoBehaviour
{
    [SerializeField] private WeaponInventory weaponInventory;
    [SerializeField] private PlayerShooterController playerMovementController;

    private void OnTriggerEnter(Collider other)
    {
        weaponInventory.Weapons.Add(other.GetComponent<WeaponsController>().WeaponData.weaponType, other.GetComponent<WeaponsController>());        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      Debug.Log(weaponInventory.Weapons.Count);
    }
}
