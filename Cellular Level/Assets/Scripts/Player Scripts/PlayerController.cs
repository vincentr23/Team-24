using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using static Models;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerMovement playerMovement;
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
    public Vector3 jumpForce;
    private Vector3 jumpForceVel;


    [Header("References")]
    public Transform cameraHolder;

    [Header("Settings")]
    public PlayerSettingsModel playerSettings;
    public float viewClampYMin = -70;
    public float viewClampYMax = 80;


    private void Awake()
    {
        // hides cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerMovement = new PlayerMovement();

        playerMovement.Player.Move.performed += e => input_Move = e.ReadValue<Vector2>();
        playerMovement.Player.Look.performed += e => input_Look = e.ReadValue<Vector2>();
        playerMovement.Player.Jump.performed += e => Jump();

        playerMovement.Enable();
        newCamRotate = cameraHolder.localRotation.eulerAngles;
        newPlayerRotate = transform.localRotation.eulerAngles;
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        CalculateMove();
        CalculateLook();
        CalculateJump();
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
        var verticalSpeed = playerSettings.WalkingForwardSpeed * input_Move.y * Time.deltaTime;
        var horizontalSpeed = playerSettings.WalkingStrafeSpeed * input_Move.x * Time.deltaTime;

        var newMoveSpeed = new Vector3(horizontalSpeed, 0, verticalSpeed);
        newMoveSpeed = transform.TransformDirection(newMoveSpeed);

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
        if (!characterController.isGrounded)
        {
            return;
        }

        jumpForce = Vector3.up * playerSettings.JumpingHeight;
    }
    public PlayerMovement GetInput()
    {
        return playerMovement;
    }
    public bool Grounded()
    {
        return characterController.isGrounded;
    }
}
