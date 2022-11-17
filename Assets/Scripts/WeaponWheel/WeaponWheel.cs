using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using TMPro;

public class WeaponWheel : MonoBehaviour
{
    public GameObject weaponWheel;

    private float timeOnPressed;

    [SerializeField] private RigBuilder rigB;

    public event Action<float> OnTabPressed;

    [SerializeField] private float buttonSelected;

    [SerializeField] private bool isOpened = false;

    [SerializeField] private TimeController timeController;

    [SerializeField] private WeaponInventory weaponInventory;

    [SerializeField] private PlayerShooter playerShooterController;

    [SerializeField] private List<WeaponWheelButton> weaponsSlots = new List<WeaponWheelButton>();

    [SerializeField] private Animator animator;

    //weapon info
    [SerializeField] private TextMeshProUGUI weaponNameText;
    [SerializeField] private TextMeshProUGUI weaponTypeText;
    [SerializeField] private TextMeshProUGUI firingRateText;
    [SerializeField] private TextMeshProUGUI bulletsInLoaderText;
    [SerializeField] private TextMeshProUGUI BulletsInAllText;
    [SerializeField] private UnityEngine.UI.Image visual;
    [SerializeField] private Sprite transparent;


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
        Cursor.lockState = CursorLockMode.Confined;
        RefreshWeapons();
        timeController.DoSlowMotion();
        weaponWheel.SetActive(true);
        isOpened = true;
    }

    public void Close()
    {
        Cursor.lockState = CursorLockMode.Locked;
        timeOnPressed = 0;
        isOpened = false;
        weaponWheel.SetActive(false);
        timeController.DoBaseMotion();
        UnselectButton();
    }

    void Update()
    {
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


    public void RefreshWeapons()
    {
        // permet d'actualiser le continue de la roue grâce a l'inventaire d'arme
        foreach (var weaponSlot in weaponsSlots)
        {
            if (weaponInventory.Weapons.ContainsKey(weaponSlot.WeaponType))
            {
                weaponSlot.Weapon = weaponInventory.Weapons[weaponSlot.WeaponType];
                weaponSlot.ItemName = weaponInventory.Weapons[weaponSlot.WeaponType].WeaponData.name;
                weaponSlot.Icon.sprite = weaponInventory.Weapons[weaponSlot.WeaponType].WeaponData.visualIcon;
            }
                
        }
    }

    private void UnselectButton()
    {
        foreach (var weaponSlot in weaponsSlots)
            weaponSlot.IsSelected = false;
    }

    public void EquipWeapon(Weapons weapon, Transform leftHandGrip, WeaponTypeEnum weaponTypeEnum)
    {
        //UnequipWeapon();
        animator.SetLayerWeight(5, 1);

        // si n'a jamais été équiper, l'ajoute à l'endroit dédier au type d'arme
        if (weapon.gameObject.transform.position != weapon.EquipedPosition.position)
        {
            weapon.gameObject.GetComponent<BoxCollider>().enabled = false;
            weapon.gameObject.transform.SetParent(weapon.EquipedPosition);
            weapon.gameObject.transform.position = weapon.EquipedPosition.position;
            weapon.gameObject.transform.rotation = weapon.EquipedPosition.rotation;                       
        }

        // actualise le type / l'arme / le grip de la main gauche et active l'arme
        //playerShooterController.IsArmed = true;
        playerShooterController.IsTakingWeapon = true;
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
            var weaponEquiped = playerShooterController.gameObject.GetComponentsInChildren<Weapons>(true).First(x => x.gameObject.name == playerShooterController.Weapon.WeaponData.name);
            if (weaponEquiped != null)
            {
                weaponEquiped.gameObject.transform.SetParent(weaponEquiped.BackPosition);
                weaponEquiped.gameObject.transform.position = weaponEquiped.BackPosition.position;
                weaponEquiped.gameObject.transform.rotation = weaponEquiped.BackPosition.rotation;
            }
        }
    }

    public void ShowWeaponInformations(Weapons weapon)
    {
        if (weapon != null)
        {
            weaponNameText.text = weapon.WeaponData.name;
            weaponTypeText.text = weapon.WeaponData.weaponType.ToString();
            firingRateText.text = weapon.WeaponData.weaponFiringRate.ToString();
            bulletsInLoaderText.text = weapon.BulletsInLoader.ToString();
            BulletsInAllText.text = weapon.BulletsInAll.ToString();
            visual.sprite = weapon.WeaponData.visual;
        }    
    }

    public void ResetWeaponInformations()
    {
        weaponNameText.text = "";
        weaponTypeText.text = "";
        firingRateText.text = "";
        bulletsInLoaderText.text = "";
        BulletsInAllText.text = "";
        visual.sprite = transparent;
    }
}
