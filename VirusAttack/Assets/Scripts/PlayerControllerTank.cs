using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(CharacterController), typeof(Animator))]

public class PlayerControllerTank : MonoBehaviour
{
	PhotonView view;

    private CharacterController characterController;
    private Animator animator;

    public float walkingSpeed = 4.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

	public float mouseSensitivity = 1;

    Vector3 moveDirection = Vector3.zero;
   // float rotationX = 0;
    [SerializeField] GameObject cameraHolder;
	float verticalLookRotation;

    [HideInInspector]
    public bool canMove = true;

    void Start(){
        characterController = GetComponent<CharacterController>();
		view = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
		
		if(!view.IsMine){
            Destroy(GetComponentInChildren<Camera>().gameObject);
            //Destroy(rb);
        }
    }

    void Update(){
		if(!view.IsMine){
            return;
        }

		if(view.IsMine){
			lookAround();

			// We are grounded, so recalculate move direction based on axes
			Vector3 forward = transform.TransformDirection(Vector3.forward);
			Vector3 right = transform.TransformDirection(Vector3.right);
			// Press Left Shift to run		
			bool isWalking = Input.GetKey(KeyCode.W);
			bool isAttacking = Input.GetKey(KeyCode.V);
			bool isRunning = Input.GetKey(KeyCode.LeftShift);
			animator.SetBool("isWalking", Input.GetKey(KeyCode.W));
			animator.SetBool("isAttacking", Input.GetKey(KeyCode.V));
			animator.SetBool("isRunning",Input.GetKey(KeyCode.LeftShift) );
			float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
			float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
			float movementDirectionY = moveDirection.y;
			moveDirection = (forward * curSpeedX) + (right * curSpeedY);

			if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
			{
				moveDirection.y = jumpSpeed;
			}
			else
			{
				moveDirection.y = movementDirectionY;
			}

			// Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
			// when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
			// as an acceleration (ms^-2)
			if (!characterController.isGrounded)
			{
				moveDirection.y -= gravity * Time.deltaTime;
			}

			// Move the controller
			characterController.Move(moveDirection * Time.deltaTime);

			// Player and Camera rotation
			/*
			if (canMove){
			    rotationX += -Input.GetAxis("Mouse X") * lookSpeed;
			    rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
			    playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
			    transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse Y") * lookSpeed, 0);
			} */
				
		//	animator.SetBool("isRunning",Input.GetKey(KeyCode.LeftShift) );
		}
    }

    void lookAround(){
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);
        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -80f, 80f);
        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }


}
