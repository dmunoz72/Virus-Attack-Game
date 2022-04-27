using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
[RequireComponent(typeof(CharacterController), typeof(Animator))]

public class MedicController : MonoBehaviourPunCallbacks
{
	PhotonView view;
	PlayerManager playerManager;
	private CharacterController characterController;
	private Animator animator;
	public AudioSource DamageSound;

	// Player Game Values
	[SerializeField] TextMeshProUGUI playerHealthText;
	[SerializeField] TextMeshProUGUI ammoText;
	[SerializeField] GameObject ui;


	public const float maxHealth = 1000f;
	private float currentHealth = maxHealth;
	public float walkingSpeed = 4.5f;
	public float runningSpeed = 6.5f;
	public float jumpSpeed = 10.0f;
	public float gravity = 20.0f;
	//public Camera playerCamera;
	//public float lookSpeed = 2.0f;
	//public float lookXLimit = 45.0f;
	public float mouseSensitivity = 1;

//	public bool MeleeAttackPublicData;

	Vector3 moveDirection = Vector3.zero;
	// float rotationX = 0;
	[SerializeField] GameObject cameraHolder;
	float verticalLookRotation;

	[HideInInspector]
	public bool canMove = true;

	void Awake() // i think this functino makes sure the game is running b4 accessing data
	{
		view = GetComponent<PhotonView>();
		playerManager = PhotonView.Find((int)view.InstantiationData[0]).GetComponent<PlayerManager>();
	} 


	void Start()
	{
		currentHealth = maxHealth;
		characterController = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
		//playerManager = GetComponent<PlayerManager>();

		// Lock cursor
	//	Cursor.lockState = CursorLockMode.Locked;
	//	Cursor.visible = false;
		playerHealthText.text = "+" + currentHealth;
      
		if (!view.IsMine)
		{
			Destroy(GetComponentInChildren<Camera>().gameObject);
			Destroy(ui);
		}
	}

	void Update()
	{
		if (!view.IsMine) // if not local player, we shouldnt do anything
		{
			return;
		}

		if (view.IsMine) // if local player we can move around and other stuff
		{
			lookAround();
			//bool IsIdle = true;
			//animator.SetBool("IsWalking", IsIdle);

			bool IsWalking = Input.GetKey(KeyCode.W);
			animator.SetBool("IsWalking", Input.GetKey(KeyCode.W));

		
			/*bool isIdle = false;
			animator.SetBool("isIdle", true);
			bool Lturn = Input.GetKey(KeyCode.A);
			animator.SetBool("Lturn", Input.GetKey(KeyCode.A));
			bool Rturn = Input.GetKey(KeyCode.D);
			animator.SetBool("Rturn", Input.GetKey(KeyCode.D));
			*/
			//bool MeleeAttack = Input.GetKey(KeyCode.Y);
			//animator.SetBool("MeleeAttack", MeleeAttack);

			/*	if (MeleeAttack)
				{
					MeleeAttackFUN(MeleeAttack);
				}
			*/
			// We are grounded, so recalculate move direction based on axes
			Vector3 forward = transform.TransformDirection(Vector3.forward);
			Vector3 right = transform.TransformDirection(Vector3.right);
			// Press Left Shift to run		

			float curSpeedX = canMove ? (IsWalking ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
			float curSpeedY = canMove ? (IsWalking ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
			float movementDirectionY = moveDirection.y;
			moveDirection = (forward * curSpeedX) + (right * curSpeedY);

			if (Input.GetButton("Jump") && characterController.isGrounded)//&& canMove
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

		}

		// Weapon switching
		/*for (int i = 0; i < items.Length; i++)
		{
			if (Input.GetKeyDown((i + 1).ToString()))
			{
				EquipItem(i);
				break;
			}
		}
		*/
		/* here
		if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
		{
			//if (itemIndex >= items.Length - 1)
			{
				EquipItem(0);
			}
			else
			{
				EquipItem(itemIndex + 1);
			}
		}
		else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
		{
			if (itemIndex <= 0)
			{
				EquipItem(items.Length - 1);
			}
			else
			{
				EquipItem(itemIndex - 1);
			}
		}*/

		if (transform.position.y < -10f)
		{
			playerManager.Die();
		}
	}


	/*
	public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
		if (!view.IsMine && targetPlayer == view.Owner)
		{
			EquipItem((int)changedProps["itemIndex"]);
		}
	}

	void EquipItem(int _index)
	{
		Debug.Log("Equipped Item " + _index);

		if (_index == previousItemIndex)
		{
			return;
		}
		itemIndex = _index;
		items[itemIndex].itemGameObject.SetActive(true);

		if (previousItemIndex != -1)
		{
			items[previousItemIndex].itemGameObject.SetActive(false);
		}

		previousItemIndex = itemIndex;

		if (view.IsMine)
		{
			Hashtable hash = new Hashtable();
			hash.Add("itemIndex", itemIndex);
			PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
		}
	} */

	#region Public Methods
	[PunRPC]
	public void RPC_TakeDamage(float damage)
	{
		if (!view.IsMine)
		{
			return;
		}
		Debug.Log("took damage: " + damage);
		DamageSound.Play();
		currentHealth -= damage;
		playerHealthText.text = "+" + currentHealth;

		if (currentHealth <= 0)
		{
			Debug.Log("YOU DIED");
			Die();
		}

	}
	#endregion

	void lookAround()
	{
		transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);
		verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation, -80f, 80f);
		cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
	}

	void Die()
	{
		playerManager.Die();
	}

}
