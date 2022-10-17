using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/New weapon")]
public class WeaponData : ScriptableObject
{
    public string named;
    public Sprite visual;
    public GameObject prefab;
    public int bulletsAmountMax;
    public int bulletsAmountAllMax;
    public GameObject bulletPrefab;
    public int bulletSpeed;
    public AudioClip shootSound;
    public WeaponTypeEnum weaponType;
    public float shakeAmplitude;
    public float shakeFrequency;
}
