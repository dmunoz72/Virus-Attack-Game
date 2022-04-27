using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System.Collections;

public class BasicGunController : MonoBehaviour {
    public int magSize = 30;
    public int reservedAmmoCapacity = 270;
    public int currentAmmo;
	public int ammoInReserve;
    public Vector3 normalLocalPosition;
    public Vector3 aimingLocalPosition;
    public AudioSource ShootingSound;
    [SerializeField] TextMeshProUGUI ammoText;
    public GameObject placeholderGun;
    [SerializeField] Item Gun;

    PhotonView view;
    // Start is called before the first frame update
    void Awake(){
        view = GetComponent<PhotonView>();	

    }
    void Start() {
        currentAmmo = magSize;
		ammoInReserve = reservedAmmoCapacity;
    }

    // Update is called once per frame
    void Update(){
        if(!view.IsMine){ // if not local player, we shouldnt do anything 
            return;
        }

		if(view.IsMine){
            DetermineAim();

            if(Input.GetMouseButtonDown(0) && currentAmmo > 0){
                ShootingSound.Play();
                
                currentAmmo--;
                ammoText.text = currentAmmo.ToString() + " | " + ammoInReserve.ToString();
                Gun.Use();
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
        }
    }
	private void DetermineAim() {
		//Debug.Log("In Determine Aim");
        Vector3 target = normalLocalPosition;
        if (Input.GetMouseButton(1)) 
            target = aimingLocalPosition;

        Vector3 desiredPosition = Vector3.Lerp(placeholderGun.transform.localPosition, target, Time.deltaTime * 10);

        placeholderGun.transform.localPosition = desiredPosition;
    }


}
