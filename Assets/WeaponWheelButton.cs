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
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private WeaponTypeEnum weaponType;
    [SerializeField] private Image icon;
    [SerializeField] private bool selected = false;
    [SerializeField] WeaponsController weapon;
    [SerializeField] private Transform weaponPosition;
    [SerializeField] private WeaponWheelController weaponWheelInputs;
    [SerializeField] private PlayerShooterController playerShooterController;
    [SerializeField] private Transform leftHandGrip;
    [SerializeField] private RigBuilder rigB;
    [SerializeField] private Animator animator;
    // Start is called before the first frame update

    public WeaponsController Weapon
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
        selected = true;
        //if (weapon.parent != weaponPosition)
        //{
        //    weapon.SetParent(weaponPosition);
        //    weapon.position = weaponPosition.position;
        //    weapon.rotation = weaponPosition.rotation;
        //    itemText.text = itemName;
        //}
        //leftHandGrip.gameObject.GetComponent<TwoBoneIKConstraint>().data.target = weapon.Find("leftHandGrip").transform;
        
        //weaponWheelInputs.Close();
        //playerShooterController.IsArmed = true;
        //playerShooterController.Weapon = Weapon.gameObject.GetComponent<WeaponsController>();
        //rigB.Build();
        //animator.Rebind();
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
