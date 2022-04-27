using UnityEngine;

public class FirewallScriptWizard : MonoBehaviour {
    // Start is called before the first frame update
   public GameObject firewall;
   public long firewallCooldown = 0;
   public long speed = 1;
   //public Rigidbody rb;
   long frameCounter = 0;
   public long counterLimit = 60;
   bool isActivated = false;
   GameObject firewallInstance;
   
    void Start(){
        
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.F) && !isActivated && firewallCooldown == 0){
            isActivated = true;
            //spawns 2 units in front of player
            Vector3 spawnPoint = transform.localPosition + 2 * transform.forward;
            firewallInstance = Instantiate(firewall, spawnPoint, transform.localRotation);
        }

        if(isActivated){
            frameCounter++;
            firewallInstance.transform.Translate(Vector3.forward * speed * Time.deltaTime);

            if(frameCounter == counterLimit){
                isActivated = false; 
                Destroy(firewallInstance);
                frameCounter = 0;
            }
        }   
    }
}