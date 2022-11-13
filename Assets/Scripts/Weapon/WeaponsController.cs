using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponsController : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private int _bulletsInLoader;
    [SerializeField] private int _bulletsInAll;

    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform spawnBulletProjectile;
    [SerializeField] private Transform leftHandGrip;
    [SerializeField] private Transform backPosition;
    [SerializeField] private Transform equipedPosition;

    private PlayerShooterController shooterController;
    [SerializeField] private bool readyToShoot;

    [SerializeField] private ParticleSystem fxShoot;
    [SerializeField] private ParticleSystem fxGround;
    private AudioSource audioSource;

    private bool isUsed = false;
   [SerializeField] private Cinemachine.CinemachineVirtualCamera aimVirtualCamera;

    public ParticleSystem FxGround
    {
        get { return this.fxGround; }
        set { this.fxGround = value; }
    }

    public Transform EquipedPosition
    {
        get { return equipedPosition; }
        set { equipedPosition = value; }
    }
    public Transform BackPosition
    {
        get { return backPosition; }
        set { backPosition = value; }
    }
    public bool IsUsed
    {
        get { return isUsed; }
        set { isUsed = value; }
    }

    public Transform LeftHandGrip
    {
        get { return leftHandGrip; }
        set { leftHandGrip = value; }
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

    public int BulletsInAll
    {
        get { return _bulletsInAll; }
        set { _bulletsInAll = value; }
    }

    public AudioSource AudioSource
    {
        get { return audioSource; }
        set { audioSource = value; }
    }

    public WeaponData WeaponData
    {
        get { return weaponData; }
        set { weaponData = value; }
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //leftHandGrip = GetComponentsInChildren<Transform>().First(x => x.name == "leftHandGrip");
        audioSource = GetComponent<AudioSource>();
        shooterController = GameObject.Find("Player").gameObject.GetComponent<PlayerShooterController>();
        GetComponentsInChildren<Transform>().First(x => x.gameObject.name == "leftHandGrip");
        readyToShoot = true;
    }

    void Update()
    {
    }
    public bool CanShoot()
    {
        return _bulletsInLoader > 0 && readyToShoot && !shooterController.IsReloading;
    }

    public bool CheckBullets()
    {
        return BulletsInLoader > 0;
    }

    public void Reload()
    {
        var bulletNeed = weaponData.bulletsAmountMax - BulletsInLoader;
        if (_bulletsInAll > weaponData.bulletsAmountMax)
        {
            BulletsInLoader += bulletNeed;
            _bulletsInAll -= bulletNeed;
        }
    }

    public void Shoot()
    {
        readyToShoot = false;
        audioSource.clip = weaponData.shootSound;
        audioSource.Play(0);
        fxShoot.Play();
        BulletsInLoader--;

        var lastBullet = Instantiate(pfBulletProjectile, spawnBulletProjectile.position, Quaternion.LookRotation(((shooterController.MousePosition() + Vector3.up) - spawnBulletProjectile.position).normalized, Vector3.up));
        lastBullet.GetComponent<Rigidbody>().velocity = transform.forward * 50f;

        //if (readyToShoot)
        ShakeCamera(weaponData.shakeAmplitude, weaponData.shakeFrequency);


        Invoke("ResetShot", weaponData.bulletDelay);
    }

    public void StopShoot()
    { 
       fxShoot.Stop();
       //audioSource.Stop();
        //reset camera shake
       ShakeCamera(0f, 0f);
    }


    private void ResetShot()
    {
        readyToShoot = true;
        ShakeCamera(0, 0);
    }

    private void ShakeCamera(float amplitude, float frequency)
    {
        if (!CheckBullets())
        {
            amplitude = 0;
            frequency = 0;
        }
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = aimVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(cinemachineBasicMultiChannelPerlin.m_AmplitudeGain, amplitude, 10f);
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = Mathf.Lerp(cinemachineBasicMultiChannelPerlin.m_AmplitudeGain, frequency, 10f);
    }
}
