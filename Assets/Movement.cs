using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Movement : MonoBehaviour
{

    public Transform playerCamera;
    [SerializeField]
    [Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] bool cursorLock = true;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float Speed = 6.0f;
    [SerializeField]
    [Range(0.0f, 0.5f)] float moveSmoothTime = 0.03f;
    [SerializeField] float gravity = 30f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;

    

    public float itemPickupDistance;
     public Transform attachedObject = null;
   // public Transform attach;
  
    float attachedDistance = 0f;
    public Transform head;

    public float jumpHeight = 6f;
    float velocityY;
    bool isGrounded;

    float cameraCap;
    Vector2 currentMouseDelta;
    Vector2 currentMouseDeltaVelocity;

    CharacterController controller;
    Vector2 currentDir;
    Vector2 currentDirVelocity;
    Vector3 velocity;



    void Start()
    {
        
        controller = GetComponent<CharacterController>();

        if (cursorLock) {
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = true; 
            } }
    }

    
    void Update()
    {
        UpdateMouse();
        UpdateMove();
        // picking up objects

        RaycastHit hit;
        bool cast = Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, itemPickupDistance);

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (attachedObject != null)
            {
                attachedObject.SetParent(null);

                if (attachedObject.GetComponent<Rigidbody>() != null)
                {
                    attachedObject.GetComponent<Rigidbody>().isKinematic = false;
                }

                if (attachedObject.GetComponent<Collider>() != null)
                {
                    attachedObject.GetComponent<Collider>().enabled = true;
                }
            
       
                attachedObject = null;
               //attach = null;
            }

            else 
            {
                if (cast)
                {


                    if (hit.transform.CompareTag("pickup"))
                    {
                        
                        attachedObject = hit.transform;
                        attachedObject.SetParent(transform);

                        if (attachedObject.GetComponent<Rigidbody>() != null)
                        {
                            attachedObject.GetComponent<Rigidbody>().isKinematic = true;
                        }

                        if (attachedObject.GetComponent<Collider>() != null)
                        {
                            attachedObject.GetComponent<Collider>().enabled = false;
                        }
                    }

                        }
            }
        }

        if (attachedObject != null)
        {
           
            attachedObject.position = head.position + head.forward * attachedDistance;
            attachedObject.Rotate(transform.right * Input.mouseScrollDelta.y * 30f, Space.World);

        }
        
    }

    void UpdateMouse()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraCap -= currentMouseDelta.y * mouseSensitivity;

        cameraCap = Mathf.Clamp(cameraCap, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraCap;

        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void UpdateMove()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, ground);

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        velocityY += gravity * 2f * Time.deltaTime;

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (isGrounded && controller.velocity.y < -1f)
        {
            velocityY = -8f;
        }

        Vector3 inputDir = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        Vector3 moveDirection = transform.TransformDirection(inputDir);
        controller.Move(moveDirection * Speed * Time.deltaTime + Vector3.up * velocityY * Time.deltaTime);
    }
}
