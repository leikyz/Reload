using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickingWeapon : MonoBehaviour
{
    [SerializeField] private WeaponInventory weaponInventory;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerShooter playerShooterController;
    [SerializeField] private TMPro.TextMeshProUGUI textMeshPro;


    private float pickingLayerWeight;
    private bool isPicking;
    private GameObject weapon;

    private Transform position;

    private void Update()
    {
        animator.SetLayerWeight(4, Mathf.Lerp(animator.GetLayerWeight(4), pickingLayerWeight, Time.deltaTime * 5f));
        animator.SetBool("IsPicking", isPicking);
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Weapon"))
        {
            
            textMeshPro.gameObject.SetActive(true);
            textMeshPro.text = WeaponPickingText(other.gameObject.GetComponent<Weapons>(), weaponInventory);
            // check if a same weapon type is already in weapon inventory, to get one weapon by type in weapon wheel 
            if (Input.GetKey(KeyCode.B))
            {
                weapon = other.gameObject;
                pickingLayerWeight = 1;
                
                isPicking = true;
                textMeshPro.gameObject.SetActive(false);
                if (weaponInventory.Weapons.ContainsKey(other.GetComponent<Weapons>().WeaponData.weaponType))
                {
                    weaponInventory.Weapons[other.GetComponent<Weapons>().WeaponData.weaponType].gameObject.transform.SetParent(null);
                    weaponInventory.Weapons[other.GetComponent<Weapons>().WeaponData.weaponType].gameObject.GetComponent<Weapons>().FxGround.Play();
                    weaponInventory.Weapons[other.GetComponent<Weapons>().WeaponData.weaponType].gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    weaponInventory.Weapons[other.GetComponent<Weapons>().WeaponData.weaponType].gameObject.GetComponent<BoxCollider>().enabled = true;
                    weaponInventory.Weapons[other.GetComponent<Weapons>().WeaponData.weaponType] = other.GetComponent<Weapons>();
                }
                else
                {
                    weaponInventory.Weapons.Add(other.GetComponent<Weapons>().WeaponData.weaponType, other.GetComponent<Weapons>());
                }

                //check if weapon is a same weapontype is already equiped, change position of weapon depending on the value (back position or equiped position)
                //if (playerShooterController.Weapon != null && playerShooterController.Weapon.WeaponData.weaponType == other.GetComponent<WeaponsController>().WeaponData.weaponType)
                //{
                    position = other.GetComponent<Weapons>().EquipedPosition;
                    playerShooterController.Weapon = other.GetComponent<Weapons>();
                //}
                //else
                //{
                //    position = other.GetComponent<WeaponsController>().BackPosition;
                //}

                //if (position != null)
                //    PositionChoice(position, other.gameObject);

                ChangeCharacteristic(other.gameObject);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        textMeshPro.gameObject.SetActive(false);
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
        weapon.GetComponent<Weapons>().FxGround.Stop();

    }

    private string WeaponPickingText(Weapons weapon, WeaponInventory weaponInventory)
    {
   
        var sentence = "Appuyer sur B pour prendre " + weapon.gameObject.name; ;

        if ((weaponInventory.Weapons.ContainsKey(weapon.WeaponData.weaponType)))
            sentence += "\n Attention cela remplacera l'arme actuel";
        return sentence;   
    }

    public void AddWeaponToHand()
    {
        PositionChoice(weapon.GetComponent<Weapons>().EquipedPosition, weapon);
    }

    public void AnimatorSetPicking()
    {
        isPicking = false;
        pickingLayerWeight = 0;
    }

    public void AddWeaponToBack()
    {
        if (playerShooterController.Weapon == null)
            PositionChoice(weapon.GetComponent<Weapons>().BackPosition, weapon.gameObject);
        else
            PositionChoice(weapon.GetComponent<Weapons>().BackPosition, playerShooterController.Weapon.gameObject);
    }
} 
