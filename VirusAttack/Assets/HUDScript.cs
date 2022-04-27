using UnityEngine;
using Photon.Pun;
using TMPro;

public class HUDScript : MonoBehaviour {
    private int currentAmmo;
    public int currentHealth;
    private int reservedAmmoCapacity;
    private int ammoInReserve;
    private int magSize;

    GameObject BasicPlayerValues;
    GameObject BasicGunValues;
    [SerializeField] TextMeshProUGUI playerHealthText; // Player Health Text Element

    public bool hasGun;

	[SerializeField] TextMeshProUGUI ammoText; // Ammo Text Element
	[SerializeField] GameObject UserInterfaceCanvas; // The main canvas, aka what this script is attached to
    
    void Awake(){
        BasicPlayerValues = GameObject.Find("BasicPlayerController");
        BasicGunValues = GameObject.Find("BasicGunController");
        if(!hasGun){
            ammoText.enabled = false;
        }
        else {
            magSize = BasicGunValues.GetComponent<BasicGunController>().magSize;
            reservedAmmoCapacity = BasicGunValues.GetComponent<BasicGunController>().reservedAmmoCapacity;
            ammoText.text = magSize.ToString() + " | " + reservedAmmoCapacity.ToString();
        }
	}

    
    void Start() { 
        currentHealth = BasicPlayerValues.GetComponent<BasicPlayerController>().currentHealth;
		playerHealthText.text = "+" + currentHealth; // health on screen is = to models currentHealth(init:1000)
    }
    void Update() {
        currentHealth = BasicPlayerValues.GetComponent<BasicPlayerController>().currentHealth;
		playerHealthText.text = "+" + currentHealth;

        if(hasGun) {
            currentAmmo = BasicGunValues.GetComponent<BasicGunController>().currentAmmo;
		    ammoText.text = currentAmmo.ToString() + " | " + ammoInReserve.ToString();
        }
    }

}