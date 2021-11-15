using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GunController : MonoBehaviour {
	// Variables don't change through out code, create menu in unity as well
	
	PhotonView view;
	
	[Header("Gun Settings")]
	public float fireRate = 0.1f;
	public int magSize = 30;
	public int reservedAmmoCapacity = 70;
	public int damageDone = 10;
	public float range = 100f;
	
	public Camera fpsCam;
	
	
	// Variables that change throughout code
	bool _canShoot;
	public int _currentAmmoInMag; // public so that it is accessible to UI
	public int _ammoInReserve;
	
	//Muzzle Flash
	public Image muzzleFlashImage;
	public Sprite[] flashes;
	
	//When game starts these are the values of the variables
	private	void Start(){
		_currentAmmoInMag = magSize;               // mag is full
		_ammoInReserve = reservedAmmoCapacity;     // total ammo is full
		_canShoot = true;                          // player can shoot
		
		view = GetComponent<PhotonView>();
	}
	
	//Aiming
    public Vector3 normalLocalPosition;
    public Vector3 aimingLocalPosition;

    public float aimSmoothing = 10;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 1;
    private Vector2 _currentRotation;
    public float weaponSwayAmount = 10;

    //Weapon Recoil
    public bool randomizeRecoil;
    public Vector2 randomRecoilConstraints;
    //You only need to assign this if randomize recoil is off
    public Vector2[] recoilPattern;
	
	private void Update(){ // when game is running
	
       if(view.IsMine){
			DetermineAim();
			DetermineRotation();
			
			if(Input.GetMouseButton(0) && _canShoot && _currentAmmoInMag > 0){ // if player presses left click and they can shoot, and they have ammo
				_canShoot = false;                      // so you don't shoot, while shooting, AKA infinite loop sort of
				_currentAmmoInMag--; 					// subtract 1 from ammo
				StartCoroutine(ShootGun());             // Calls a function separate from main thread, work with timers that don't work on unity's main one
			}
			
			else if(Input.GetKeyDown(KeyCode.R) && _currentAmmoInMag < magSize && _ammoInReserve > 0){
				int amountNeeded = magSize - _currentAmmoInMag; // geting value of how much ammo is needed to fill mag
				
				if(amountNeeded >= _ammoInReserve){ // if you need more ammo than you have in reserve
					_currentAmmoInMag += _ammoInReserve; // add whats in reserve to mag
					_ammoInReserve = 0; // set reserve ammo to 0
				}
				
				else{ // if you need less than whats in reserve then,
					_currentAmmoInMag = magSize;     // fill up mag
					_ammoInReserve -= amountNeeded;  // subtract ammount needed from reserve
				}
			}
		}
	}
	
	    private void DetermineRotation(){
			
        Vector2 mouseAxis = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        mouseAxis *= mouseSensitivity;
        _currentRotation += mouseAxis;

        _currentRotation.y = Mathf.Clamp(_currentRotation.y, -90, 90);

        transform.localPosition += (Vector3)mouseAxis * weaponSwayAmount / 1000;

        transform.root.localRotation = Quaternion.AngleAxis(_currentRotation.x, Vector3.up);
        transform.parent.localRotation = Quaternion.AngleAxis(-_currentRotation.y, Vector3.right);


    }
    private void DetermineAim(){
		
        Vector3 target = normalLocalPosition;
        if (Input.GetMouseButton(1)) 
            target = aimingLocalPosition;

        Vector3 desiredPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * aimSmoothing);

        transform.localPosition = desiredPosition;
    }

    private void DetermineRecoil(){
		
        transform.localPosition -= Vector3.forward * 0.1f;

        if (randomizeRecoil){
			
            float xRecoil = Random.Range(-randomRecoilConstraints.x, randomRecoilConstraints.x);
            float yRecoil = Random.Range(-randomRecoilConstraints.y, randomRecoilConstraints.y);

            Vector2 recoil = new Vector2(xRecoil, yRecoil);

            _currentRotation += recoil;
        }
		
        else{
			
            int currentStep = magSize + 1 - _currentAmmoInMag;
            currentStep = Mathf.Clamp(currentStep, 0, recoilPattern.Length - 1);

            _currentRotation += recoilPattern[currentStep];
        }
    }
	
	IEnumerator ShootGun(){
		
		DetermineRecoil();
		StartCoroutine(MuzzleFlash());
		
	/*	RaycastHit hit;
		if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)){
			
			Debug.Log(hit.transform.name);
			PlayerManager target = hit.transform.GetComponent<PlayerManager>();
			
			if(target != null){
				target.takeDamage(damageDone);
			}
		}
	*/
		yield return new WaitForSeconds(fireRate); // fires as fast as fireRate setting
		_canShoot = true;                          // after shooting, player can shoot again
	}
	
	IEnumerator MuzzleFlash(){
		muzzleFlashImage.sprite = flashes[Random.Range(0, flashes.Length)];
		muzzleFlashImage.color = Color.white;
		yield return new WaitForSeconds(0.05f);
		muzzleFlashImage.sprite = null;
		muzzleFlashImage.color = new Color(0, 0, 0, 0);
	}
}
