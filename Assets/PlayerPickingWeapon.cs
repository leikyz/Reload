using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickingWeapon : MonoBehaviour
{
    [SerializeField] private List<WeaponWheelController> weaponSlots = new List<WeaponWheelController>();
    [SerializeField] private PlayerShooterController playerMovementController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            foreach (var weaponSlot in weaponSlots)
            {
                if (weaponSlot.WeaponType == other.GetComponent<WeaponsController>().WeaponData.weaponType)
                {
                    if (weaponSlot.Weapon != null)
                        weaponSlot.Weapon.gameObject.active = false;
                    other.GetComponent<BoxCollider>().enabled = false;
                    weaponSlot.ItemName = other.name;
                    weaponSlot.Icon.sprite = other.GetComponent<WeaponsController>().WeaponData.visual;
                    weaponSlot.Weapon = other.gameObject.transform;
                    //other.transform.SetParent(riflePosition);
                    //other.transform.position = riflePosition.position;
                    //other.transform.rotation = riflePosition.rotation;
                    //playerMovementController.IsArmed = true;
                }
                    
            }
        }
            
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
