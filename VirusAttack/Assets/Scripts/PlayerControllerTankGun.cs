using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;


[RequireComponent(typeof(CharacterController), typeof(Animator))]

public class PlayerControllerTankGun : MonoBehaviourPunCallbacks, IDamageable
{
	PhotonView view;

	PlayerManager playerManager;
    private CharacterController characterController;
    private Animator animator;

	
// Player Game Values
	[SerializeField] TextMeshProUGUI playerHealthText;
	[SerializeField] TextMeshProUGUI ammoText;
	[SerializeField] GameObject ui;

	public int magSize = 30;
	public int reservedAmmoCapacity = 270;

	public const float maxHealth = 1000f;
	public float currentHealth = maxHealth;
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    float jumpSpeed = 8.0f;
    float gravity = 20.0f;
    public Camera playerCamera;
    float lookSpeed = 2.0f;
	float lookXLimit = 45.0f;

	public float mouseSensitivity = 1;

    Vector3 moveDirection = Vector3.zero;
   // float rotationX = 0;
    [SerializeField] GameObject cameraHolder;
	float verticalLookRotation;

// Gun setup
	[SerializeField] Item[] items;
	int itemIndex;
	int previousItemIndex = -1;
	
	private int currentAmmo;
	private int ammoInReserve;

	public Vector3 normalLocalPosition;
    public Vector3 aimingLocalPosition;

	public GameObject placeholderGun;
    [HideInInspector]
    public bool canMove = true;

	//Rigidbody rb;
	void Awake(){
		view = GetComponent<PhotonView>();
		//rb  = GetComponent<Rigidbody>();
		playerManager = PhotonView.Find((int)view.InstantiationData[0]).GetComponent<PlayerManager>();
	}

    void Start(){
		currentAmmo = magSize;
		ammoInReserve = reservedAmmoCapacity;
		
        characterController = GetComponent<CharacterController>();
		
        animator = GetComponent<Animator>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
		
		ammoText.text = magSize.ToString() + " | " + reservedAmmoCapacity.ToString();
		playerHealthText.text = "+" + currentHealth;

		if(view.IsMine){
			EquipItem(0);
		}
		else{
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(ui);
			//Destroy(rb);
        }
    }

    void Update(){
		if(!view.IsMine){
            return;
        }

		if(view.IsMine){
			lookAround();
			DetermineAim();

			playerHealthText.text = "+" + currentHealth;
			ammoText.text = currentAmmo.ToString() + " | " + ammoInReserve.ToString();
			// We are grounded, so recalculate move direction based on axes
			Vector3 forward = transform.TransformDirection(Vector3.forward);
			Vector3 right = transform.TransformDirection(Vector3.right);
			// Press Left Shift to run
			bool isRunning = Input.GetKey(KeyCode.LeftShift);
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
				
			animator.SetBool("isRunning",Input.GetKey(KeyCode.LeftShift) );
		}

		for(int i = 0; i  < items.Length; i++){
			if(Input.GetKeyDown((i+1).ToString())){
				EquipItem(i);
				break;
			}
		}

		if(Input.GetAxisRaw("Mouse ScrollWheel") > 0f){
			if(itemIndex >= items.Length - 1){
				EquipItem(0);
			}
			else{
				EquipItem(itemIndex + 1);
			}
		}
		else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0f){
			if(itemIndex <= 0){
				EquipItem(items.Length - 1);
			}
			else{
				EquipItem(itemIndex - 1);
			}
		}

		if(Input.GetMouseButtonDown(0) && currentAmmo > 0){
			currentAmmo--;
			ammoText.text = currentAmmo.ToString() + " | " + ammoInReserve.ToString();
			items[itemIndex].Use();
		}
		
		else if(Input.GetKeyDown(KeyCode.R) && currentAmmo < magSize && ammoInReserve > 0){
			int amountNeeded = magSize - currentAmmo; // geting value of how much ammo is needed to fill mag
				
			if(amountNeeded >= ammoInReserve){ // if you need more ammo than you have in reserve
				currentAmmo += ammoInReserve; // add whats in reserve to mag
				ammoInReserve = 0; // set reserve ammo to 0
			}
				
			else{ // if you need less than whats in reserve then,
				currentAmmo = magSize;     // fill up mag
				ammoInReserve -= amountNeeded;  // subtract ammount needed from reserve
			}
		}

		if(transform.position.y < -10f){
			Die();
		}

	}

    void lookAround(){
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);
        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -80f, 80f);
        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

	private void DetermineAim(){
		Debug.Log("In Determine Aim");
        Vector3 target = normalLocalPosition;
        if (Input.GetMouseButton(1)) 
            target = aimingLocalPosition;

        Vector3 desiredPosition = Vector3.Lerp(placeholderGun.transform.localPosition, target, Time.deltaTime * 10);

        placeholderGun.transform.localPosition = desiredPosition;
    }

	



	public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps){
		if(!view.IsMine && targetPlayer == view.Owner){
			EquipItem((int)changedProps["itemIndex"]);
		}
	}

	void EquipItem(int _index){
		Debug.Log("Equipped Item " + _index);

		if(_index == previousItemIndex){
			return;
		}
		itemIndex = _index;
		items[itemIndex].itemGameObject.SetActive(true);

		if(previousItemIndex != -1){
			items[previousItemIndex].itemGameObject.SetActive(false);
		}

		previousItemIndex = itemIndex;

		if(view.IsMine){
			Hashtable hash = new Hashtable();
			hash.Add("itemIndex", itemIndex);
			PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
		}
	}

	public void TakeDamage(float damage){
		Debug.Log("Take Damage called");
		view.RPC("RPC_TakeDamage", RpcTarget.All, damage);
	}

	[PunRPC]
	void RPC_TakeDamage(float damage){
		if(!view.IsMine){
			return;
		}
		Debug.Log("took damage: " + damage);

		currentHealth -= damage;
		playerHealthText.text = "+" + currentHealth;

		if(currentHealth <= 0){
			Die();
		}

	}

	void Die(){
		playerManager.Die();
	}

}
