using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponWheelController : MonoBehaviour
{

    [SerializeField] private int Id;
    [SerializeField] private string itemName;
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private WeaponTypeEnum weaponType;
    [SerializeField] private Image icon;
    [SerializeField] private bool selected = false;
    [SerializeField] Transform weapon;
    [SerializeField] private Transform weaponPosition;
    [SerializeField] private WeaponWheelInputs weaponWheelInputs;
    [SerializeField] private PlayerShooterController playerShooterController;
    // Start is called before the first frame update

    public Transform Weapon
    {
        get { return weapon; }
        set { weapon = value; }
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
        //if (selected)
        //{
            
        //}
    }
    public void Selected()
    {
        if (weapon.parent != weaponPosition)
        {
            weapon.SetParent(weaponPosition);
            weapon.position = weaponPosition.position;
            weapon.rotation = weaponPosition.rotation;
            itemText.text = itemName;
        }

        selected = true;
        weaponWheelInputs.Close();
        playerShooterController.IsArmed = true;
        playerShooterController.Weapon = Weapon.gameObject.GetComponent<WeaponsController>();
    }

    public void Deselected()
    {
        selected = false;
    }
    public void HoverEnter()
    {
        itemText.text = itemName;
    }
    public void HoverExit()
    {
        itemText.text = "";
    }
}
