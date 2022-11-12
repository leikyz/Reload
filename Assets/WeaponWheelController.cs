using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class WeaponWheelController : MonoBehaviour
{
    public GameObject weaponWheel;

    private float timeOnPressed;

    [SerializeField] private RigBuilder rigB;

    public event Action<float> OnTabPressed;

    [SerializeField] private float buttonSelected;

    [SerializeField] private bool isOpened = false;

    [SerializeField] private TimeController timeController;

    //[SerializeField] private Transform back;

    //[SerializeField] private WeaponWheelButton weapon;

    [SerializeField] private WeaponInventory weaponInventory;

    [SerializeField] private PlayerShooterController playerShooterController;

    [SerializeField] private List<WeaponWheelButton> weaponsSlots = new List<WeaponWheelButton>();

    [SerializeField] private Animator animator;

    void Start()
    {
        OnTabPressed += OnOpened;
    }

    public float ButtonSelected
    {
        get { return buttonSelected; }
        set { buttonSelected = value; }
    }

    private void OnOpened(float obj)
    {
        RefreshWeapons();
        timeController.DoSlowMotion();
        weaponWheel.SetActive(true);
        isOpened = true;
    }

    public void Close()
    {
        timeOnPressed = 0;
        isOpened = false;
        weaponWheel.SetActive(false);
        timeController.DoBaseMotion();
        UnselectButton();
    }

    void Update()
    {
        //Debug.Log(ButtonSelected.ItemName);
        if (Input.GetKey(KeyCode.Tab))
        {
            OnTabPressed?.Invoke(timeOnPressed += 1 * Time.deltaTime);
        }

        else
        {
            if (isOpened)
                Close();
        }
    }


    private void RefreshWeapons()
    {
        // permet d'actualiser le continue de la roue grâce a l'inventaire d'arme
        foreach (var weaponSlot in weaponsSlots)
        {
            if (weaponInventory.Weapons.ContainsKey(weaponSlot.WeaponType))
            {
                weaponSlot.Weapon = weaponInventory.Weapons[weaponSlot.WeaponType];
                weaponSlot.ItemName = weaponInventory.Weapons[weaponSlot.WeaponType].WeaponData.name;
                weaponSlot.Icon.sprite = weaponInventory.Weapons[weaponSlot.WeaponType].WeaponData.visual;
            }
                
        }
    }

    private void UnselectButton()
    {
        foreach (var weaponSlot in weaponsSlots)
            weaponSlot.IsSelected = false;
    }

    public void EquipWeapon(WeaponsController weapon, Transform weaponPosition, Transform leftHandGrip, WeaponTypeEnum weaponTypeEnum)
    {
        UnequipWeapon();


        // si n'a jamais été équiper, l'ajoute à l'endroit dédier au type d'arme
        if (weapon.gameObject.transform.position != weaponPosition.position)
        {
            weapon.gameObject.GetComponent<BoxCollider>().enabled = false;
            weapon.gameObject.transform.SetParent(weaponPosition);
            weapon.gameObject.transform.position = weaponPosition.position;
            weapon.gameObject.transform.rotation = weaponPosition.rotation;                       
        }

        // actualise le type / l'arme / le grip de la main gauche et active l'arme
        playerShooterController.IsArmed = true;
        playerShooterController.WeaponTypeEnumActual = weaponTypeEnum;
        playerShooterController.Weapon = weapon;
        leftHandGrip.gameObject.GetComponent<TwoBoneIKConstraint>().data.target = weapon.LeftHandGrip;
        weapon.gameObject.SetActive(true);
        rigB.Build();
    }



    public void UnequipWeapon()
    {
        // retrouve l'arme active pour la désactiver quand une nouvelle est sélectionnée
        if (playerShooterController.Weapon != null)
        {
            var weaponEquiped = playerShooterController.gameObject.GetComponentsInChildren<WeaponsController>(true).First(x => x.gameObject.name == playerShooterController.Weapon.WeaponData.name);
            if (weaponEquiped != null)
            {
                weaponEquiped.gameObject.transform.SetParent(weaponEquiped.BackPosition);
                weaponEquiped.gameObject.transform.position = weaponEquiped.BackPosition.position;
                weaponEquiped.gameObject.transform.rotation = weaponEquiped.BackPosition.rotation;
            }
        }
    }
}
