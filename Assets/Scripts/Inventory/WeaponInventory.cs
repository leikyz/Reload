using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    // slots

    [SerializeField] private Dictionary<WeaponTypeEnum, WeaponsController> weapons = new();

    public Dictionary<WeaponTypeEnum, WeaponsController> Weapons
    {
        get { return weapons; }
        set { weapons = value; }
    }
}
