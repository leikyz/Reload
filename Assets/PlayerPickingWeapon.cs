using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickingWeapon : MonoBehaviour
{
    [SerializeField] private WeaponInventory weaponInventory;
    [SerializeField] private PlayerShooterController playerMovementController;

    private void OnTriggerEnter(Collider other)
    {
        // v�rifie si une arme du m�me type est dans l'inventaire, la change sinon l'ajoute
        other.gameObject.SetActive(false);

        if (weaponInventory.Weapons.ContainsKey(other.GetComponent<WeaponsController>().WeaponData.weaponType))
        {
            weaponInventory.Weapons[other.GetComponent<WeaponsController>().WeaponData.weaponType].gameObject.transform.SetParent(null);      
            weaponInventory.Weapons[other.GetComponent<WeaponsController>().WeaponData.weaponType].gameObject.GetComponent<Rigidbody>().isKinematic = false;
            weaponInventory.Weapons[other.GetComponent<WeaponsController>().WeaponData.weaponType] = other.GetComponent<WeaponsController>();
        }            
        else
            weaponInventory.Weapons.Add(other.GetComponent<WeaponsController>().WeaponData.weaponType, other.GetComponent<WeaponsController>());        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      //Debug.Log(weaponInventory.Weapons.Count);
    }
}