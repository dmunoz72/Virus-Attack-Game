using UnityEngine;
using Photon.Pun;
using TMPro;

[RequireComponent(typeof(CharacterController), typeof(Animator))]

public class BasicPlayerController : MonoBehaviour {
	PhotonView view;
	public bool hasGun;
	PlayerManager playerManager;
	private CharacterController characterController;
	private Animator animator;

	// Player Game Values
	[SerializeField] TextMeshProUGUI playerHealthText;
	[SerializeField] TextMeshProUGUI ammoText;
	[SerializeField] GameObject ui;


	public const int maxHealth = 1000;
	public int currentHealth = maxHealth;
	public float walkingSpeed = 3.5f;
	public float runningSpeed = 5.5f;
	public float jumpSpeed = 8.0f;
	float gravity = 20.0f;
	bool IsMoving = false;
	//public Camera playerCamera;
	//public float lookSpeed = 2.0f;
	//public float lookXLimit = 45.0f;

	public float mouseSensitivity = 1;

	Vector3 moveDirection = Vector3.zero;
	// float rotationX = 0;
	[SerializeField] GameObject cameraHolder;
	float verticalLookRotation;

	[HideInInspector]
	bool canMove = true;

	void Start()
	{
		if(!hasGun){
            ammoText.enabled = false;
        }
		characterController = GetComponent<CharacterController>();
		view = GetComponent<PhotonView>();
		animator = GetComponent<Animator>();

		// Lock cursor
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		playerHealthText.text = "+" + currentHealth;

		if (!view.IsMine) { 
			Destroy(GetComponentInChildren<Camera>().gameObject);
            // Destroy(rb);
		}
	}

	void Update(){
		if (!view.IsMine)
		{
			return;
		}

		if (view.IsMine){
			lookAround();
			IsMoving = Input.GetKey(KeyCode.W);
			animator.SetBool("IsMoving", Input.GetKey(KeyCode.W));
	
			// We are grounded, so recalculate move direction based on axes
			Vector3 forward = transform.TransformDirection(Vector3.forward);
			Vector3 right = transform.TransformDirection(Vector3.right);
			// Press Left Shift to run		
	
			float curSpeedX = canMove ? (IsMoving ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
			float curSpeedY = canMove ? (IsMoving ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
			float movementDirectionY = moveDirection.y;
			moveDirection = (forward * curSpeedX) + (right * curSpeedY);

			if (Input.GetButton("Jump") && canMove && characterController.isGrounded) {
				moveDirection.y = jumpSpeed;
			}
			else {
				moveDirection.y = movementDirectionY;
			}

			// Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
			// when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
			// as an acceleration (ms^-2)
			if (!characterController.isGrounded) {
				moveDirection.y -= gravity * Time.deltaTime;
			}

			// Move the controller
			characterController.Move(moveDirection * Time.deltaTime);
		}
	}

	void lookAround() {
		transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);
		verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation, -80f, 80f);
		cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
	}
}
