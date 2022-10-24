using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    // slots

    [SerializeField] private List<WeaponWheelController> weaponSlots = new List<WeaponWheelController>();

    public List<WeaponWheelController> WeaponSlots
    {
        get { return weaponSlots; }
        set { weaponSlots = value; }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
