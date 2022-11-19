using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerTakingWeapon : MonoBehaviour
{
    [SerializeField] private WeaponInventory weaponInventory;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerShooter playerShooter;
    [SerializeField] private TMPro.TextMeshProUGUI textMeshPro;
    [SerializeField] private Transform leftHandGrip;
    private float switchLayerWeight;
    private float pickingLayerWeight;
    private bool isPicking;
    private bool isSwitching;
    private GameObject weapon;
    [SerializeField] private bool canResetRig;

    private Transform position;
    [SerializeField] private RigBuilder rigB;

    public Transform LeftHandGrip
    {
        get { return leftHandGrip; }
        set { leftHandGrip = value; }
    }

    private void Start()
    {
        //rigB.Build();
    }


    private void Update()
    {
        if (canResetRig)
            rigB.Build();
        animator.SetLayerWeight(5, Mathf.Lerp(animator.GetLayerWeight(5), switchLayerWeight, Time.deltaTime * 5f));
        animator.SetLayerWeight(4, Mathf.Lerp(animator.GetLayerWeight(4), pickingLayerWeight, Time.deltaTime * 5f));
        animator.SetBool("IsPicking", isPicking);
        animator.SetBool("IsSwitching", isSwitching);
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
                //    position = other.GetComponent<Weapons>().EquipedPosition;
                //playerShooter.Weapon = other.GetComponent<Weapons>();
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

    public void AddWeaponToHand(Weapons weapon, Transform leftHGrip, WeaponTypeEnum weaponTypeEnum, bool onSwitching)
    {
        //UnequipWeapon();
        //switchLayerWeight = 1;
        //rigB.Build();
        if (onSwitching)
        {
            isSwitching = true;
            switchLayerWeight = 1;
        }
            
        // si n'a jamais été équiper, l'ajoute à l'endroit dédier au type d'arme
        if (weapon.gameObject.transform.position != weapon.EquipedPosition.position)
        {
            weapon.gameObject.GetComponent<BoxCollider>().enabled = false;
            weapon.gameObject.transform.SetParent(weapon.EquipedPosition);
            weapon.gameObject.transform.position = weapon.EquipedPosition.position;
            weapon.gameObject.transform.rotation = weapon.EquipedPosition.rotation;
        }

        // actualise le type / l'arme / le grip de la main gauche et active l'arme
        //animator.SetBool("IsTakingWeapon", true);
        playerShooter.WeaponTypeEnumActual = weaponTypeEnum;
        playerShooter.Weapon = weapon;
        leftHGrip.gameObject.GetComponent<TwoBoneIKConstraint>().data.target = weapon.LeftHandGrip;
        playerShooter.IsArmed = true;
        isPicking = false;
        //canResetRig = true;

    }
    public void ResetCanBuildRig()
    {
        canResetRig = false;
        isPicking = false;
        isSwitching = false;
        pickingLayerWeight = 0;
        switchLayerWeight = 0;
        //isPicking = false;
    }

    private string WeaponPickingText(Weapons weapon, WeaponInventory weaponInventory)
    {
   
        var sentence = "Appuyer sur B pour prendre " + weapon.gameObject.name; ;

        if ((weaponInventory.Weapons.ContainsKey(weapon.WeaponData.weaponType)))
            sentence += "\n Attention cela remplacera l'arme actuel";
        return sentence;   
    }

    private void EquipWeapon(bool onSwitch)
    {
        AddWeaponToHand(weapon.GetComponent<Weapons>(), leftHandGrip, weapon.GetComponent<Weapons>().WeaponData.weaponType, onSwitch);
        //PositionChoice(weapon.GetComponent<Weapons>().EquipedPosition, weapon);
        //playerShooter.IsArmed = true;
        //isPicking = false;
        //pickingLayerWeight = 0;
    }

    public bool tee(bool onSwtich)
    {
        return true;
    }

    public void test(bool onSwitch)
    {
        AddWeaponToHand(weapon.GetComponent<Weapons>(), leftHandGrip, weapon.GetComponent<Weapons>().WeaponData.weaponType, onSwitch);
    }

    public void AddWeaponToBack()
    {
        //playerShooter.Weapon.gameObject
        //if (playerShooter.Weapon == null)
        //    PositionChoice(weapon.GetComponent<Weapons>().BackPosition, weapon.gameObject);
        //else
        PositionChoice(playerShooter.Weapon.BackPosition, playerShooter.Weapon.gameObject);
        playerShooter.Weapon = null;
        playerShooter.IsArmed = false;

      
    }
} 
