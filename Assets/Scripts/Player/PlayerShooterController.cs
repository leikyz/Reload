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
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private LayerMask aimColliderMask = new();
    [SerializeField] private PlayerMovementController playerMovementController;
    [SerializeField] private Transform debugTransform;
    [SerializeField] private WeaponsController weapon;

    private Animator animator;

    [SerializeField] private bool isAiming = false;
    [SerializeField] private bool isShooting = false;
    [SerializeField] private bool isReloading = false;

    private float aimRigWeight;
    private float shakeFrequency;
    private float shakeAmplitude;

    public bool IsAiming
    {
        get { return isAiming; }
        set { isAiming = value; }
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
        reloadAction.canceled += OnReloadStopped;
    }

    private void OnAimStarted(InputAction.CallbackContext obj)
    {
        animator.applyRootMotion = false;
        isAiming = true;
        crossHair.enabled = true;
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
        crossHair.enabled = false;
        aimVirtualCamera.gameObject.SetActive(false);
        playerMovementController.RotationSpeed = 5f;
        playerMovementController.RotateOnMove = true;
        aimRigWeight = 0;
       
    }

    private void OnShootStarted(InputAction.CallbackContext obj)
    {
        if (weapon.CanShoot())
        {
            isShooting = true;
            shakeFrequency = 1.2f;
            shakeAmplitude = 0.3f;
        }
        else
        {
            isShooting = false;
            weapon.StopShoot();
        }
    }
    private void OnShootStopped(InputAction.CallbackContext obj)
    {
        isShooting = false;
        weapon.StopShoot();
        shakeAmplitude = 0;
        shakeFrequency = 0;
    }

    private void OnReloadStarted(InputAction.CallbackContext obj)
    {
        aimVirtualCamera.gameObject.SetActive(false);
        isReloading = true;
        animator.SetBool("IsReloading", true);
    }

    private void OnReloadStopped(InputAction.CallbackContext obj)
    {
        isReloading = false;
        animator.SetBool("IsReloading", false);
    }

    void Update()
    {
        aimRig.weight = Mathf.Lerp(aimRig.weight, aimRigWeight, Time.deltaTime * 20f);
        RotatePlayerOnAimed(MousePosition());
        HandleAiming();
        HandleShooting();
  
    }

    private void HandleShooting()
    {
        if (weapon.CanShoot() && isShooting)
        {         
            weapon.Shoot();
        }
        //else
        //    weapon.StopShoot();

        ShakeCamera(shakeAmplitude, shakeFrequency);
    }

    private void HandleAiming()
    {
        if (isAiming)
        {
            animator.SetLayerWeight(2, Mathf.Lerp(animator.GetLayerWeight(2), 1f, Time.deltaTime * 5f));
        }
    }

    private void HandleReloading()
    {

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

    private void ShakeCamera(float amplitude, float frequency)
    {
        if (!weapon.CheckBullets())
        {
            amplitude = 0;
            frequency = 0;
        }
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = aimVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(cinemachineBasicMultiChannelPerlin.m_AmplitudeGain, amplitude, 2f);
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = Mathf.Lerp(cinemachineBasicMultiChannelPerlin.m_AmplitudeGain, frequency, 2f);
    }
}
