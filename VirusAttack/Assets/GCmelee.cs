using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class GCmelee : MonoBehaviourPunCallbacks
{
    BoxCollider newcollider;
    PhotonView view;
    private GCcontroller _gcController;
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
        if (attacking = Input.GetKeyDown(KeyCode.F))
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
            if (other.gameObject.tag == "virus")
            {
                if (IsHitting == true)
                {
                    other.gameObject.GetPhotonView().RPC("RPC_TakeDamage", RpcTarget.All, 100f);
                    print("Glasscannon HIT VIRUS");
                    IsHitting = false;
                }
            }
            //}
        }

        //}
    }

    // The function below cues the animation and hesitates the extension of the hitbox to match the 
    // length of the model whilst extended. afterwards it returns the hitbox to its original size and waits
    // a moment before resetting the animation to false, so it can be reused in later occurences.
    IEnumerator waiter()
    {
        animator.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(1.10f);
        newcollider.size = new Vector3(3.435222f, 6.167736f, 2.5f);
        yield return new WaitForSeconds(.90f);
        newcollider.size = new Vector3(3.435222f, 6.167736f, 1.23087f);
        yield return new WaitForSeconds(.001f);
        animator.SetBool("IsAttacking", false);
        yield return new WaitForSeconds(5);
    }
}