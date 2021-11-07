using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int playerHealth = 1000;
	public TextMeshProUGUI playerHealthText;
	public TextMeshProUGUI ammoText;
	public GunController gunValues;
	public static bool isDead;
	
    void Start(){
		gunValues = FindObjectOfType<GunController>();
		ammoText.text = gunValues._currentAmmoInMag.ToString() + " | " + gunValues._ammoInReserve.ToString();
		playerHealthText.text = "+" + playerHealth;
        isDead = false;
    }

    // Update is called once per frame
    void Update() {

		
		ammoText.text = gunValues._currentAmmoInMag.ToString() + " | " + gunValues._ammoInReserve.ToString();
        //if (isDead){
			
			
			
		//}
    }
	
	public void takeDamage(int damageAmount){
		Debug.Log("IN takeDamage Function");
		playerHealth -= damageAmount;
	
		if(playerHealth >= 0){
			playerHealthText.text = "+" + playerHealth;
			
		}
		
		else{
			isDead = true;
		}
	}
	
}
