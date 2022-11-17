using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    // slots

    [SerializeField] private Dictionary<WeaponTypeEnum, Weapons> weapons = new();

    public Dictionary<WeaponTypeEnum, Weapons> Weapons
    {
        get { return weapons; }
        set { weapons = value; }
    }
}
