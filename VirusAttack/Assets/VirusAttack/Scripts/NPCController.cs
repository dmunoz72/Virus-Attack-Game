using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour{
	
	[Header("NPC Settings")]
	public float fireRate = 0.1f;
	public int damageDone = 10;
	public float range = 100f;
	public Camera fpsCam;
	
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if(Input.GetKeyDown(KeyCode.H)) {

			StartCoroutine(ShootGun());   
		}
    }
	
	IEnumerator ShootGun(){
		
		RaycastHit hit;
		if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)){
			
			Debug.Log(hit.transform.name);
			PlayerManager target = hit.transform.GetComponent<PlayerManager>();
			
			if(target != null){
				Debug.Log("target == tank");
				target.takeDamage(damageDone);
			}
		}
		
		yield return new WaitForSeconds(fireRate); // fires as fast as fireRate setting
		
	}
	
}
