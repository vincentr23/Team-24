using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Models;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerInput playerInput;
    public Vector2 input_Move;
    public Vector2 input_Look;

    private Vector3 newCamRotate;
    private Vector3 newPlayerRotate;

    [SerializeField] int moveLockout;
    [SerializeField] int moveWait;

    [Header("Gravity")]
    public float gravityAmount;
    public float gravityMin;
    public float playerGravity;

    // jumping variables
    [Header("Jump")]
    public Vector3 jumpForce;
    [SerializeField] int jumpStam;
    [SerializeField] int minJumpStam;
    private bool jumped;
    private Vector3 jumpForceVel;

    [Header("Sprint")]
    public int sprintMin;
    public int maxStam;
    [SerializeField] int stamina;
    public int stamWaitVal;
    [SerializeField] int stamWait;
    public Image Stambar;
    [SerializeField] float SlowMult;
    [SerializeField] float SlowedBase;
    private float SlowTime;
    private bool sprinting;
    private float forwardSpeed;

    [Header("Damage")]
    bool damaged;
    [SerializeField] GameObject deathScreen;

    [Header("References")]
    public Transform cameraHolder;
    [SerializeField] Camera targetCamera;

    [Header("Settings")]
    public PlayerSettingsModel playerSettings;
    public float viewClampYMin = -70;
    public float viewClampYMax = 80;
    //[SerializeField] GameObject[] playerSpawns;
    [SerializeField] GameObject playerSpawn;
    public Transform m_initialPosition = null;
    public bool m_isInitialPositionSet = false;
    [SerializeField] bool setPosition = false;

    Animator anim;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        // hides cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        newCamRotate = cameraHolder.localRotation.eulerAngles;
        newPlayerRotate = transform.localRotation.eulerAngles;
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        sprinting = false;
        stamina = maxStam;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        // camera stuff
        gameObject.layer = LayerMask.NameToLayer(DecipherLayer(PlayerNum()));
        FixLayers();
        //Setting spawn
        transform.position =
            GameObject.FindGameObjectsWithTag("Player Spawn")[PlayerNum()].
            transform.position;

    }

    private void Update()
    {
        CalculateMove();
        CalculateLook();
        CalculateJump();
        CalculateStam();
    }
    private void FixedUpdate()
    {
        RegainStam();
        MoveLockout();
    }
    void LateUpdate()
    {
        if (!m_isInitialPositionSet)
        {
            playerSpawn = GameObject.FindGameObjectsWithTag("Player Spawn")[PlayerNum()];
            if (playerSpawn == null) return;
            if (m_initialPosition == null)
            {
                Debug.Log("Here");
                m_initialPosition = playerSpawn.transform;
                gameObject.transform.position = m_initialPosition.position;
                m_isInitialPositionSet = true;
            }
        }
        else if (!setPosition)
        {
            if (transform.position == m_initialPosition.position)
                setPosition = true;
            transform.position = m_initialPosition.position;
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
    public void OnGather(IInteractable pickup)
    {
        anim.SetTrigger("Gather");
        moveLockout = moveWait;
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
        if (moveLockout > 0)
        {
            return;
        }
        var verticalSpeed = input_Move.y * Time.deltaTime;
        Sprint();
        if (verticalSpeed > 0) verticalSpeed *= forwardSpeed;
        else
        {
            verticalSpeed *= playerSettings.WalkingBackwardSpeed;
            sprinting = false;
        }

        var horizontalSpeed = playerSettings.WalkingStrafeSpeed * input_Move.x * Time.deltaTime;

        if (SlowTime > 0)
        {
            verticalSpeed *= SlowMult;
            horizontalSpeed *= SlowMult;
            SlowTime--;
        }
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
    //public void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("PickUp"))
    //        other.gameObject.SetActive(false);
    //}
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
        forwardSpeed = playerSettings.WalkingForwardSpeed;
        if (sprinting)
        {
            stamina--;
            forwardSpeed *= playerSettings.RunningMultiplier;
            if (stamina == 0) sprinting = false;
        }
    }

    public void Slow()
    {
        SlowTime = SlowedBase;
    }
    private void MoveLockout()
    {
        if (moveLockout > 0) moveLockout--;
    }
    private void Show(string layerName)
    {
        targetCamera.cullingMask |= 1 << LayerMask.NameToLayer(layerName);
    }

    // Turn off the bit using an AND operation with the complement of the shifted int:
    private void Hide(string layerName)
    {
        targetCamera.cullingMask &= ~(1 << LayerMask.NameToLayer(layerName));
    }

    // Toggle the bit using a XOR operation:
    private void Toggle(string layerName)
    {
        targetCamera.cullingMask ^= 1 << LayerMask.NameToLayer(layerName);
    }
    public string DecipherLayer(int num)
    {
        switch (num)
        {
            case 0: return "Player 1";
            case 1: return "Player 2";
            case 2: return "Player 3";
            case 3: return "Player 4";
            case 4: return "Guide 1";
            case 5: return "Guide 2";
            case 6: return "Guide 3";
            case 7: return "Guide 4";
            default: return "Invalid";
        }
    }
    public int PlayerNum()
    {
        return playerInput.playerIndex;
    }
    private void FixLayers()
    {
        for (int i = 0; i < 4; i++)
        {
            if (PlayerNum() != i) 
            {
                Hide(DecipherLayer(4 + i)); 
            }
            else Hide(DecipherLayer(PlayerNum()));
        }
    }
    public void ToggleSpawn()
    {
        m_isInitialPositionSet = false;
        m_initialPosition = null;
        setPosition = false;
    }
    private void CalculateStam()
    {
        Stambar.fillAmount = (float)stamina / (float)maxStam;
    }

    public void IsHit()
    {
        if (damaged)
        {
            anim.SetTrigger("Death");
            gameObject.tag = "Dead";
            DisablePlayer();
        }
        else
        {
            damaged = true;
            anim.SetTrigger("Hit");
        }

    }

    private void DisablePlayer()
    {
        GetComponent<PlayerController>().enabled = false;
        GetComponentInChildren<MeshCollider>().enabled = false;
        GetComponent<GuidesController>().enabled = false;
        GetComponentInChildren<CapsuleCollider>().enabled = false;
        FindObjectOfType<HeartBeat>().RebasePlayers();
        deathScreen.SetActive(true);
    }
}
