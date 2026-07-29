using System;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementManager : SerializedMonoBehaviour
{
    public float defaultMoveSpeed,moveSpeed;
    public float  sprintSpeed,sprintMultiplier;
    public float crouchSpeed,crouchDivisor;
    public float jumpStrength;
    public bool isOnGround;
    public bool isInAir;
    [SerializeField] private float airTime;
    
    [SerializeField] Collider playerCollider;

    InputAction moveAction;
    InputAction SprintAction;
    InputAction CrouchAction;   
    InputAction JumpAction;
    
    public Vector2 directionTest;
    public Rigidbody rb;
    [SerializeField] LayerMask groundLayer;
    private RaycastHit hit;
    public Transform groundCheckEmpty;
    
    public Transform CinemachineCameraHolderEmpty;
    public float elapsedTime;
    
    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        SprintAction = InputSystem.actions.FindAction("Sprint");
        CrouchAction = InputSystem.actions.FindAction("Crouch");
        JumpAction = InputSystem.actions.FindAction("Jump");
        
        sprintSpeed = moveSpeed * sprintMultiplier;
        crouchSpeed = moveSpeed / crouchDivisor;

    }

    private void Update()
    {
        UpdateMoveSpeed();
        Jump();
        
        
    }

    private void FixedUpdate()
    {
        #region player Y rotation to camera

        float camY = Camera.main.transform.eulerAngles.y; //---|player y rotation to camera
        transform.rotation = Quaternion.Euler(0, camY, 0); //----|player y rotation to camera

        #endregion


        Vector2 moveDirection = moveAction.ReadValue<Vector2>();
        directionTest = moveDirection; // Just to see how the vectors behave without affecting gameplay

        Vector3 convertYtoZ = new Vector3(moveDirection.x, 0, moveDirection.y);

        Vector3 lookDirectionz = transform.forward * convertYtoZ.z;
        Vector3 lookDirectionx = transform.right * convertYtoZ.x;

        rb.AddForce((lookDirectionz + lookDirectionx) * moveSpeed, ForceMode.Acceleration);

        
        if (moveDirection.magnitude > 0)
        {   
            elapsedTime += Time.fixedDeltaTime;
            WalkingUpDownEffect(elapsedTime / 3 + moveSpeed * 0.1f, 0.05f);
        }

        if (moveDirection.magnitude == 0)
        {
            elapsedTime = Mathf.Lerp(elapsedTime, 0, 1f);       
            CinemachineCameraHolderEmpty.transform.position = new Vector3(transform.position.x, Mathf.Lerp(CinemachineCameraHolderEmpty.transform.position.y, transform.position.y + 1.5f,1 * Time.fixedDeltaTime * 0.7f), transform.position.z);
        }
    }

    public void WalkingUpDownEffect(float time, float length)
    {
       CinemachineCameraHolderEmpty.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f + Mathf.PingPong(time, length),transform.position.z);
    }


    private void UpdateMoveSpeed()
    {
        if (CrouchAction.IsInProgress() && isOnGround)
        {
            moveSpeed = crouchSpeed;
        }
        else if (SprintAction.IsInProgress() && isOnGround)
        {
            moveSpeed = sprintSpeed;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
        }
    }

    public void Jump()
    {
        isOnGround = Physics.Raycast(groundCheckEmpty.position,Vector3.down,0.6f, groundLayer );
        isInAir = !isOnGround;
        if (JumpAction.WasPressedThisFrame() && isOnGround && !isInAir)
        {
           rb.AddForce(transform.up * jumpStrength, ForceMode.Impulse);
        }
        
    }
}

