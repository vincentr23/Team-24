using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 0;
    public float groundDrag;
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    private Rigidbody rb; 
    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    public float crouch_speed = 0;
    private bool crouching = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent <Rigidbody>(); 
        rb.freezeRotation = true;
    }
    /* void OnMove (InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>(); 
        movementX = movementVector.x; 
        movementY = movementVector.y;
    }
    */
    private void Crouch()
    {
        float temp = speed;
        speed = crouch_speed;
        crouch_speed = temp;
        crouching = !crouching;
    }
   private void FixedUpdate() 
   {
        Vector3 movement = new Vector3 (movementX, 0.0f, movementY);

        // Apply force to the Rigidbody to move the player.
        rb.AddForce(movement * speed); 

       // MovePlayer();
   }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        if (Input.GetButtonDown("Crouch"))
        {
            Crouch();
        }
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }
    private void MyInput() 
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }
    private void MovePlayer()
    {
        //moveDirection = (orientation.forward * verticalInput) + (orientation.right * horizontalInput);
        Vector3 movement = new Vector3 (moveDirection.x, 0.0f, moveDirection.z);

        rb.AddForce(movement * speed * 10f);
    }
    void OnMove(InputValue movementValue)
    {
        // Convert the input value into a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>();

        // Store the X and Y components of the movement.
        movementX = movementVector.x; 
        movementY = movementVector.y; 
    }

}
