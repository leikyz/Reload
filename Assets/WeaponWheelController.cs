using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponWheelController : MonoBehaviour
{
    public GameObject weaponWheel;

    private float timeOnPressed;

    public event Action<float> OnTabPressed;

    [SerializeField] private bool isOpened = false;

    [SerializeField] private TimeController timeController;

    //[SerializeField] private WeaponWheelButton weapon;

    [SerializeField] private WeaponInventory weaponInventory;

    [SerializeField] private List<WeaponWheelButton> weaponsSlots = new List<WeaponWheelButton>();

    void Start()
    {
        OnTabPressed += OnOpened;
    }

    private void OnOpened(float obj)
    {
        RefreshWeapons();
        Debug.Log("number : " + obj);
        timeController.DoSlowMotion();
        weaponWheel.SetActive(true);
        isOpened = true;
    }

    private void RefreshWeapons()
    {
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

    public void Close()
    {
        timeOnPressed = 0;
        isOpened = false;
        weaponWheel.SetActive(false);
        timeController.DoBaseMotion();

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

    private void ChangeWeapon()
    {
        //WeaponWheelController
    }

}
