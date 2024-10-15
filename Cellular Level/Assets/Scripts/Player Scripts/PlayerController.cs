using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using static Models;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    //private PlayerMovement playerMovement;
    public Vector2 input_Move;
    public Vector2 input_Look;

    private Vector3 newCamRotate;
    private Vector3 newPlayerRotate;

    [Header("Gravity")]
    public float gravityAmount;
    public float gravityMin;
    public float playerGravity;

    [Header("Jump")]
    // jumping variables
    private bool jumped;
    public Vector3 jumpForce;
    private Vector3 jumpForceVel;
    [SerializeField] int jumpStam;
    [SerializeField] int minJumpStam;


    [Header("Sprint")]
    private bool sprinting;
    public int sprintMin;
    private float forwardSpeed;
    public int maxStam;
    [SerializeField] int stamina;
    public int stamWaitVal;
    [SerializeField] int stamWait;

    [Header("References")]
    public Transform cameraHolder;

    [Header("Settings")]
    public PlayerSettingsModel playerSettings;
    public float viewClampYMin = -70;
    public float viewClampYMax = 80;
    public Transform m_initialPosition;
    private bool m_isInitialPositionSet = false;

    Animator anim;


    private void Awake()
    {
        // hides cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        newCamRotate = cameraHolder.localRotation.eulerAngles;
        newPlayerRotate = transform.localRotation.eulerAngles;
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        sprinting = false;
        stamina = maxStam;
    }

    private void Update()
    {
        CalculateMove();
        CalculateLook();
        CalculateJump();
    }
    private void FixedUpdate()
    {
        RegainStam();
    }
    void LateUpdate()
    {
        if (m_initialPosition != null && !m_isInitialPositionSet)
        {
            transform.position = m_initialPosition.position;
            m_isInitialPositionSet = true;
        }
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (stamina > sprintMin)
        {
            sprinting = !sprinting;
            stamWait = stamWaitVal;
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        input_Move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        input_Look = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumped = context.action.triggered;
    }

    private void CalculateLook()
    {
        newPlayerRotate.y += playerSettings.ViewXSens * input_Look.x * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(newPlayerRotate);

        newCamRotate.x += -playerSettings.ViewYSens * input_Look.y * Time.deltaTime;
        newCamRotate.x = Mathf.Clamp(newCamRotate.x, viewClampYMin, viewClampYMax);
        cameraHolder.localRotation = Quaternion.Euler(newCamRotate);
    }
    private void CalculateMove()
    {
        var verticalSpeed = input_Move.y * Time.deltaTime;
        Sprint();
        if (verticalSpeed > 0) verticalSpeed *= forwardSpeed;
        else
        {
            verticalSpeed *= playerSettings.WalkingBackwardSpeed;
            sprinting = false;
        }

        var horizontalSpeed = playerSettings.WalkingStrafeSpeed * input_Move.x * Time.deltaTime;

        var newMoveSpeed = new Vector3(horizontalSpeed, 0, verticalSpeed);
        newMoveSpeed = transform.TransformDirection(newMoveSpeed);

        if (jumped) Jump();

        if (playerGravity > gravityMin && jumpForce.y < 0.1f)
        {
            playerGravity -= gravityAmount * Time.deltaTime;
        }
        if (playerGravity < -1 && characterController.isGrounded)
        {
            playerGravity = -1;
        }
        if (jumpForce.y > 0.1f)
        {
            playerGravity = 0;
        }
        newMoveSpeed.y += playerGravity;
        newMoveSpeed += jumpForce * 0.1f;
        characterController.Move(newMoveSpeed);
    }
    public bool IsRunning()
    {
        if ((input_Move.y > -0.1) && (input_Move.y < 0.1) &&
            (input_Move.x > -0.1) && (input_Move.x < 0.1))
            return false;
        else return true;
    }
    public float GetXVel()
    {
        return input_Move.x;
    }
    public float GetYVel()
    {
        return input_Move.y;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
            other.gameObject.SetActive(false);
    }
    private void CalculateJump()
    {
        jumpForce = Vector3.SmoothDamp(jumpForce, Vector3.zero,
            ref jumpForceVel, playerSettings.JumpingFalloff);
    }
    private void Jump()
    {
        if ((!characterController.isGrounded) || (stamina < minJumpStam))
        {
            return;
        }
        anim.SetTrigger("Jump");
        jumpForce = Vector3.up * playerSettings.JumpingHeight;
        stamina -= jumpStam;
        stamWait = stamWaitVal;
        if (stamina < 0) stamina = 0;
    }
    public bool Grounded()
    {
        return characterController.isGrounded;
    }
    private void RegainStam()
    {
        if (!sprinting)
        {
            if ((stamWait == 0) && (stamina < maxStam))
            {
                stamina++;
            }
            else if (stamWait > 0)
            {
                stamWait--;
            }
        }
        else stamWait = stamWaitVal;
    }
    private void Sprint()
    {
        if (sprinting)
        {
            stamina--;
            forwardSpeed = playerSettings.RunningForwardSpeed;
            if (stamina == 0) sprinting = false;
        }
        else
        {
            forwardSpeed = playerSettings.WalkingForwardSpeed;
        }
    }
}
