using UnityEngine;

public class lightingBoltScriptWizard : MonoBehaviour{
    public GameObject lightningBolt;
    public long lightningBCooldown = 0;
    public long speed = 1;

    long frameCounter = 0;
    public long counterLimit = 120;
    bool isActivated = false;
    GameObject lightningBoltInstance;

    void Start(){
        
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.F) && !isActivated && lightningBCooldown == 0){
            isActivated = true;
            //spawns 2 units in front of player
            Vector3 spawnPoint = transform.localPosition + 1 * transform.forward;
            lightningBoltInstance = Instantiate(lightningBolt, spawnPoint, transform.localRotation);
        }

        if(isActivated){
            frameCounter++;
            lightningBoltInstance.transform.Translate(Vector3.forward * speed * Time.deltaTime);

            if(frameCounter == counterLimit){
                isActivated = false; 
                Destroy(lightningBoltInstance);
                frameCounter = 0;
            }
        }   
        
    }
}
