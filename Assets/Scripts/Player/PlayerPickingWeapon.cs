using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickingWeapon : MonoBehaviour
{
    [SerializeField] private WeaponInventory weaponInventory;
    [SerializeField] private PlayerShooterController playerMovementController;

    private Transform position;

    private void OnTriggerStay(Collider other)
    {
        // check if a same weapon type is already in weapon inventory, to get one weapon by type in weapon wheel 
        Debug.Log(other.gameObject.name);

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (weaponInventory.Weapons.ContainsKey(other.GetComponent<WeaponsController>().WeaponData.weaponType))
            {
                weaponInventory.Weapons[other.GetComponent<WeaponsController>().WeaponData.weaponType].gameObject.transform.SetParent(null);
              
                weaponInventory.Weapons[other.GetComponent<WeaponsController>().WeaponData.weaponType].gameObject.GetComponent<Rigidbody>().isKinematic = false;
                weaponInventory.Weapons[other.GetComponent<WeaponsController>().WeaponData.weaponType].gameObject.GetComponent<BoxCollider>().enabled = true;
                weaponInventory.Weapons[other.GetComponent<WeaponsController>().WeaponData.weaponType] = other.GetComponent<WeaponsController>();
            }
            else
            {
                weaponInventory.Weapons.Add(other.GetComponent<WeaponsController>().WeaponData.weaponType, other.GetComponent<WeaponsController>());
            }

            //check if weapon is a same weapontype is already equiped, change position of weapon depending on the value (back position or equiped position)
            if (playerMovementController.Weapon != null && playerMovementController.Weapon.WeaponData.weaponType == other.GetComponent<WeaponsController>().WeaponData.weaponType)
            {
                position = other.GetComponent<WeaponsController>().EquipedPosition;
                playerMovementController.Weapon = other.GetComponent<WeaponsController>();
            }
            else
            {
                position = other.GetComponent<WeaponsController>().BackPosition;         
            }

            if (position != null)
                PositionChoice(position, other.gameObject);

            ChangeCharacteristic(other.gameObject);          
        }
        

    }
    // set parent / position and rotation to weapon type position
    private void PositionChoice(Transform position, GameObject weapon)
    {
        weapon.transform.SetParent(position);
        weapon.transform.rotation = position.rotation;
        weapon.transform.position = position.position;
    }

    private void ChangeCharacteristic(GameObject weapon)
    {
        weapon.GetComponent<BoxCollider>().enabled = false;
        weapon.GetComponent<Rigidbody>().isKinematic = true;

    }
}
