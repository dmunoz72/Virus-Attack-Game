using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class VirusMelee : MonoBehaviourPunCallbacks
{
    BoxCollider newcollider;
    PhotonView view;
    private VirusController _virusController;
    private Animator animator;
    private bool attacking;
    private bool IsHitting;
    //public float damage;

    void Start()
    {
        animator = GetComponent<Animator>();
        newcollider = GetComponent<BoxCollider>();
        view = GetComponent<PhotonView>();
    }



    void Update()
    {
        if (attacking = Input.GetKeyDown(KeyCode.Y))
        {
            IsHitting = true;
            StartCoroutine(waiter());
        }
    }


    // This function checks if the objects it is colliding with is a hero character.  
    // If this is the case we trigger the attack and deal damage over the network to 
    // whichever collider made contact and to the appropriate gameobject.tag that was hit.
    [PunRPC]
    void OnTriggerEnter(Collider other)
    {
        // if (other != null)
        //{
        if (view.IsMine) // check to see if character controlled is your own view
        {
            // the if statement below checks if the gameobject being collided with is either the wizard or tank
            if (other.gameObject.tag == "wizard" | other.gameObject.tag == "tank")
            {
                if (IsHitting == true)
                {
                    other.gameObject.GetPhotonView().RPC("RPC_TakeDamage", RpcTarget.All, 100f);
                    print("AND HIT");
                    IsHitting = false;
                }
            }
            else if (other.gameObject.tag == "glasscannon" | other.gameObject.tag == "medic")
            {
                if (IsHitting == true)
                {
                    other.gameObject.GetPhotonView().RPC("RPC_TakeDamage", RpcTarget.All, 100f);
                    print("AND HIT");
                    IsHitting = false;
                }
            }
            else if (other.gameObject.tag == "Capacitor")
            {
                if (IsHitting == true)
                {
                    other.gameObject.GetPhotonView().RPC("RPCap_TakeDamage", RpcTarget.All, 1000f);
                    print("CAPACITOR HIT");
                    IsHitting = false;
                }
            }
            //}
        }

        //}
    }
    /*
     void OnTriggerExit(Collider other)
      {
        if (other.gameObject.tag == "wizard" | other.gameObject.tag == "tank" ){
            print("EXIT");
         }

     }
    */

    // The function below cues the animation and hesitates the extension of the hitbox to match the 
    // length of the model whilst extended. afterwards it returns the hitbox to its original size and waits
    // a moment before resetting the animation to false, so it can be reused in later occurences.
    IEnumerator waiter()
    {
        animator.SetBool("MeleeAttack", true);
        yield return new WaitForSeconds(1.10f);
        newcollider.size = new Vector3(1.495432f, 1.678417f, 10.401478f);
        yield return new WaitForSeconds(.90f);
        newcollider.size = new Vector3(1.495432f, 1.678417f, 5.917166f);
        yield return new WaitForSeconds(.001f);
        animator.SetBool("MeleeAttack", false);
        yield return new WaitForSeconds(5);
    }
}