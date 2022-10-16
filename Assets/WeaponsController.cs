using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsController : MonoBehaviour
{
    public float test;
    [SerializeField] private WeaponData weapon;
    [SerializeField] private int _bulletsInLoader;
    [SerializeField] private int _bulletsInAll;

    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform spawnBulletProjectile;

    [SerializeField] private PlayerShooterController shooterController;
    [SerializeField] private bool readyToShoot;

    [SerializeField] private ParticleSystem fxShoot;
    [SerializeField] private AudioSource audioSource;

    private bool isUsed = false;

    //public Text _bulletsInLoaderText;
    //public Text _bulletsInAllText;

    public bool IsUsed
    {
        get { return isUsed; }
        set { isUsed = value; }
    }

    public bool ReadyToShoot
    {
        get { return readyToShoot; }
        set { readyToShoot = value; }  
    }

    public int BulletsInLoader
    {
        get { return _bulletsInLoader; }
        set { _bulletsInLoader = value; }
    }
    private void Start()
    {
        fxShoot.Stop();
        Cursor.lockState = CursorLockMode.Locked;
        readyToShoot = true;
    }

    void Update()
    {
        Debug.Log(CanShoot());
        //_bulletsInLoaderText.text = _bulletsInLoader.ToString();
        //_bulletsInAllText.text = _bulletsInAll.ToString();
    }

    public bool CanShoot()
    {
        return _bulletsInLoader > 0 && readyToShoot;
    }

    public bool CheckBullets()
    {
        return BulletsInLoader > 0;
    }

    private void Reload()
    {
        
    }

    public void Shoot()
    {
        readyToShoot = false;
        audioSource.clip = weapon.shootSound;
        audioSource.Play(0);
        fxShoot.Play();
        BulletsInLoader--;
        var lastBullet = Instantiate(pfBulletProjectile, spawnBulletProjectile.position, Quaternion.LookRotation(((shooterController.MousePosition() + Vector3.up) - spawnBulletProjectile.position).normalized, Vector3.up));
        lastBullet.GetComponent<Rigidbody>().velocity = transform.forward * 50f;
        Invoke("ResetShot", 0.2f);
    }

    public void StopShoot()
    { 
       fxShoot.Stop();
    }


    private void ResetShot()
    {
        readyToShoot = true;
    }
}
