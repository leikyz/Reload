using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsController : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private int _bulletsInLoader;
    [SerializeField] private int _bulletsInAll;

    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform spawnBulletProjectile;

    private PlayerShooterController shooterController;
    [SerializeField] private bool readyToShoot;

    [SerializeField] private ParticleSystem fxShoot;
    private AudioSource audioSource;

    private bool isUsed = false;
   [SerializeField] private Cinemachine.CinemachineVirtualCamera aimVirtualCamera;

    //public Text _bulletsInLoaderText;
    //public Text _bulletsInAllText;

    private float shakeFrequency;
    private float shakeAmplitude;

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
        audioSource = GetComponent<AudioSource>();
        shooterController = GameObject.Find("Player").gameObject.GetComponent<PlayerShooterController>();
        //Cursor.lockState = CursorLockMode.Locked;
        readyToShoot = true;
    }

    void Update()
    {
        //Debug.Log(CanShoot());
        //_bulletsInLoaderText.text = _bulletsInLoader.ToString();
        //_bulletsInAllText.text = _bulletsInAll.ToString();
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

        ShakeCamera(0.2f, 1f);


        Invoke("ResetShot", 0.2f);
    }

    public void StopShoot()
    { 
       fxShoot.Stop();
       audioSource.Stop();
        //reset camera shake
       ShakeCamera(0f, 0f);
    }


    private void ResetShot()
    {
        readyToShoot = true;
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
