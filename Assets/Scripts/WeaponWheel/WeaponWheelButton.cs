using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Animations.Rigging;

public class WeaponWheelButton : MonoBehaviour
{

    [SerializeField] private int Id;
    [SerializeField] private string itemName;
    [SerializeField] private WeaponTypeEnum weaponType;
    [SerializeField] private Image icon;
    [SerializeField] private bool isSelected = false;
    [SerializeField] Weapons weapon;
    [SerializeField] private WeaponWheel weaponWheelController;
    [SerializeField] private PlayerShooter playerShooterController;
    [SerializeField] private PlayerTakingWeapon playerSwitchWeapon;
    // Start is called before the first frame update

    public Weapons Weapon
    {
        get { return weapon; }
        set { weapon = value; }
    }

    public bool IsSelected
    {
        get { return isSelected; }
        set { isSelected = value; }
    }
    public WeaponTypeEnum WeaponType
    {
        get { return weaponType; }          
        set { weaponType = value; }
    }

    public string ItemName
    {
        get { return itemName; }
        set { itemName = value; }
    }

    public Image Icon
    {
        get { return icon; }
        set { icon = value; }
    }

    void Start()
    {
        icon = icon.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Selected()
    {
        if (weapon != null)
        {
            weaponWheelController.ShowWeaponInformations(weapon);
        }
        isSelected = true;
        weaponWheelController.ButtonSelected = Id;
        playerSwitchWeapon.IsSwitching = true;
        playerSwitchWeapon.WheelButton = this;
        //playerSwitchWeapon.AddWeaponToHand(Weapon, playerSwitchWeapon.LeftHandGrip, weaponType, 1);

    }

    public void Deselected()
    {
        isSelected = false;
    }
    public void HoverEnter()
    {
        if (weapon != null)
        {
            weaponWheelController.ShowWeaponInformations(weapon);
        }
    }
    public void HoverExit()
    {
        weaponWheelController.ResetWeaponInformations();
    }
}
