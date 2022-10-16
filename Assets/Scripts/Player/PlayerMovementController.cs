using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 5f;

    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isGrounded;

    private CharacterController controller;
    private PlayerShooterController shooterController;
    private Vector3 playerVelocity;

    [SerializeField] private bool rotateOnMove = true;
    private bool canJump = true;
    private Animator animator;
    [SerializeField] private float speedMaginitude;
    float turnSmoothVelocity;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;

    private Transform cameraTransform;

    #region property
    public bool RotateOnMove
    {
        get { return rotateOnMove; }
        set { rotateOnMove = value; }
    }
    public bool CanJump
    {
        get { return canJump; }
        set { canJump = value; }
    }

    public float RotationSpeed
    {
        get { return rotationSpeed; }
        set { rotationSpeed = value; }
    }
    #endregion

    private void Start()
    {
        shooterController = GetComponent<PlayerShooterController>();
        animator = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];

        moveAction.performed += OnMoveStarted;
        moveAction.canceled += OnMoveStopped;

        jumpAction.performed += OnJumpStarted;
        jumpAction.canceled += OnJumpStopped;

    }
    #region update
    void Update()
    {
        Move();
        animator.SetBool("IsGrounded", isGrounded);
    }
    #endregion

    #region events
    private void OnMoveStarted(InputAction.CallbackContext obj)
    {
        isMoving = true;
        animator.SetBool("IsMoving", isMoving);
    }
    private void OnMoveStopped(InputAction.CallbackContext obj)
    {
        isMoving = false;
        animator.SetBool("IsMoving", isMoving);
    }
    private void OnJumpStarted(InputAction.CallbackContext obj)
    {
        
        if (isGrounded && canJump)
        {
            isJumping = true;
            animator.SetBool("IsJumping", isJumping);
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
    }
    private void OnJumpStopped(InputAction.CallbackContext obj)
    {      
        isJumping = false;
        animator.SetBool("IsJumping", isJumping);
    }
    #endregion

    #region functions
    private void Move()
    {
        isGrounded = controller.isGrounded;

        speedMaginitude = Mathf.Clamp01(Velocity().magnitude);
        animator.SetFloat("Blend", speedMaginitude, 1f, Time.deltaTime);

        playerVelocity.y += gravityValue * Time.deltaTime;

        if (Velocity() != Vector3.zero)
        {
            controller.Move(playerVelocity * Time.deltaTime);

            if (shooterController.IsAiming)
            {
                Vector3 targetDirection = Quaternion.Euler(0.0f, TargetDirection(moveAction.ReadValue<Vector2>()), 0.0f) * Vector3.forward;
                controller.Move(playerSpeed * Time.deltaTime * targetDirection.normalized);
            }

            if (rotateOnMove)
                transform.rotation = Quaternion.Euler(0f, Mathf.SmoothDampAngle(transform.eulerAngles.y, TargetDirection(moveAction.ReadValue<Vector2>()), ref turnSmoothVelocity, 0.2f), 0f);
        }

        Gravity(cameraTransform.forward * Velocity().z + cameraTransform.right * Velocity().x);
    }

    private Vector3 Velocity()
    {
        return new Vector3(moveAction.ReadValue<Vector2>().x, 0, moveAction.ReadValue<Vector2>().y);   
    }

    private void Gravity(Vector3 move)
    {

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        move.y = 0f;
    }

    private float TargetDirection(Vector2 movement)
    {
        return Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

    }
    #endregion
}