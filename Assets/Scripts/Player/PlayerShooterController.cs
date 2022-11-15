using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class PlayerShooterController : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction aimAction;
    private InputAction shootAction;
    private InputAction reloadAction;

    [SerializeField] private Image crossHair;
    [SerializeField] private Rig aimRig;
    [SerializeField] private Rig leftHandRig;
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private LayerMask aimColliderMask = new();

    [SerializeField] private Transform debugTransform;
    [SerializeField] private WeaponsController weapon;

    private Animator animator;
    private PlayerMovementController playerMovementController;

    [SerializeField] private bool isAiming = false;
    [SerializeField] private bool isShooting = false;
    [SerializeField] private bool isReloading = false;
    [SerializeField] private bool isArmed = false;
    [SerializeField] private bool isTakingWeapon = false;
    [SerializeField] private WeaponTypeEnum weaponTypeEnumActual;

    private float aimRigWeight = 0;
    private float armedRigWeight = 0;
    private float leftHandWeight = 0;

    public WeaponTypeEnum WeaponTypeEnumActual
    {
        get { return weaponTypeEnumActual; }
        set { weaponTypeEnumActual = value; }
    }
    public WeaponsController Weapon
    {
        get { return weapon; }
        set { weapon = value; }
    }

    public bool IsTakingWeapon
    {
        get { return isTakingWeapon; }
        set { isTakingWeapon = value; }
    }

    public bool IsArmed
    {
        get { return isArmed; }
        set { isArmed = value; }
    }

    public bool IsAiming
    {
        get { return isAiming; }
        set { isAiming = value; }
    }

    public bool IsReloading
    {
        get { return isReloading; }
        set { isReloading = value; }
    }
    private void Awake()
    {
        Application.targetFrameRate = 144;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        aimAction = playerInput.actions["Aim"];
        shootAction = playerInput.actions["Shoot"];
        reloadAction = playerInput.actions["Reload"];

        aimAction.performed += OnAimStarted;
        aimAction.canceled += OnAimStopped;

        shootAction.performed += OnShootStarted;
        shootAction.canceled += OnShootStopped;

        reloadAction.performed += OnReloadStarted;

        playerMovementController = GetComponent<PlayerMovementController>();
    }

    private void OnAimStarted(InputAction.CallbackContext obj)
    {

        animator.applyRootMotion = false;
        isAiming = true;
        crossHair.gameObject.SetActive(true);
        aimRigWeight = 1;
        playerMovementController.RotateOnMove = false;
        playerMovementController.RotationSpeed = 2f;
        aimVirtualCamera.gameObject.SetActive(true);
    }

    private void OnAimStopped(InputAction.CallbackContext obj)
    {
        animator.applyRootMotion = true;
        animator.SetLayerWeight(2, 0);
        isAiming = false;
        crossHair.gameObject.SetActive(false);
        aimVirtualCamera.gameObject.SetActive(false);
        playerMovementController.RotationSpeed = 5f;
        playerMovementController.RotateOnMove = true;
        aimRigWeight = 0;
       
    }

    private void OnShootStarted(InputAction.CallbackContext obj)
    {
        if (isArmed)
        {
            if (weapon.CanShoot())
            {
                isShooting = true;

            }
        }

    }
    private void OnShootStopped(InputAction.CallbackContext obj)
    {
        if (IsArmed)
        {
            isShooting = false;
            weapon.StopShoot();
        }
    }
    private void OnReloadStarted(InputAction.CallbackContext obj)
    {
        armedRigWeight = 0;
        isReloading = true;
       
        animator.SetBool("IsReloading", true);
    }
    void Update()
    {
        aimRig.weight = Mathf.Lerp(aimRig.weight, aimRigWeight, Time.deltaTime * 20f);
        leftHandRig.weight = Mathf.Lerp(leftHandRig.weight, leftHandWeight, Time.deltaTime * 20f);
        animator.SetBool("IsTakingWeapon", IsTakingWeapon);


        RotatePlayerOnAimed(MousePosition());
        if (isArmed)
        {
            HandleArmed();
            HandleAiming();
            HandleShooting();
        }
    }

    private void HandleArmed()
    {
        if (weaponTypeEnumActual == WeaponTypeEnum.ASSAULT_RIFFLE || weaponTypeEnumActual == WeaponTypeEnum.HEAVY_WEAPON)
            armedRigWeight = 1f;
        else
            armedRigWeight = 0f;

        if (!isReloading && weaponTypeEnumActual != WeaponTypeEnum.GUN)
            leftHandWeight = 1f;
        else
            leftHandWeight = 0;

        animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), armedRigWeight, Time.deltaTime * 5f));
    }
        //}
    

    private void HandleShooting()
    {
        if (weapon.CanShoot() && isShooting && !isReloading && IsAiming)
        {         
            weapon.Shoot();
        }
        else
        {
            if (!weapon.CheckBullets() || isReloading)
                weapon.StopShoot();
        }
        
    }
    private void HandleAiming()
    {
        //active l'animation de visée selon le type d'arme
        if (isAiming)
        {
            if (weaponTypeEnumActual == WeaponTypeEnum.ASSAULT_RIFFLE)
                animator.SetLayerWeight(2, Mathf.Lerp(animator.GetLayerWeight(2), 1f, Time.deltaTime * 5f));
            else if(weaponTypeEnumActual == WeaponTypeEnum.GUN)
                animator.SetLayerWeight(3, Mathf.Lerp(animator.GetLayerWeight(3), 1f, Time.deltaTime * 5f));
        }

        else
        {
            if (animator.GetLayerWeight(2) != 0 || animator.GetLayerWeight(3) != 0)
            {
                animator.SetLayerWeight(2, Mathf.Lerp(animator.GetLayerWeight(2), 0f, Time.deltaTime * 5f));
                animator.SetLayerWeight(3, Mathf.Lerp(animator.GetLayerWeight(3), 0f, Time.deltaTime * 5f));
            }

        }


    }
    public Vector3 MousePosition()
    {

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderMask))
        {
            debugTransform.position = raycastHit.point;
            return raycastHit.point;
        }
        else
            return Vector3.zero;
    }

    private void OnReloadStopped()
    {
        //reloadAction.performed -= OnReloadStarted;
        armedRigWeight = 1;
        isReloading = false;
        animator.SetBool("IsReloading", false);
        weapon.Reload();
    }

    private void RotatePlayerOnAimed(Vector3 mousePosition)
    {
        if (isAiming)
        {
            Vector3 worldAimTarget = mousePosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 10f);
        }      
    }

    public void AnimatorSetArmed()
    {
        IsTakingWeapon = false;
    }
}
