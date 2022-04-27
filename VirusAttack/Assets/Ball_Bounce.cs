using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Bounce : MonoBehaviour
{
    //SphereCollider collider;
    public float pushForce = 50f;
    public float radius = 50f;


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "wizard" | other.gameObject.tag == "tank")
        {
            print("collided with trackball");
        }

        // the if statement below checks if the gameobject being collided with any characters.
        if (other.gameObject.tag == "wizard" | other.gameObject.tag == "tank")
        {

            Vector3 explosionPos = transform.position; //- transform.position;
                                                       //Rigidbody otherRB = other.GetComponent<Rigidbody>();
            print("Collisoisn");
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.AddExplosionForce(pushForce, explosionPos, radius, 3.0F);
            }

        }
    }

            //Rigidbody otherRB = other.GetComponent<Rigidbody>();
           // otherRB.AddExplosionForce(pushForce, explosionPos, radius, 2.0F);
            //otherRB.AddForce(transform.up * pushForce, ForceMode.Impulse);
            //otherRB.AddForce(explosionPos * pushForce);
            // otherRB.AddExplosionForce(pushForce, other.contacts[0].point, 10);
            //print("Bounce Detected");

            // otherRB.velocity = Vector3.Reflect(otherRB.velocity, other.contacts[0].normal);


            //collider.AddForce(Vector2.left * pushForce, ForceMode2D.Impulse);

            //                float bounce = 60f; //amount of force to apply
            //              collider.AddForce(other.contacts[0].normal * bounce);
            //            isBouncing = true;
            //          Invoke("StopBounce", 0.3f);
        
        /*  else if (other.gameObject.tag == "glasscannon" | other.gameObject.tag == "medic")
          {
              float bounce = 60f; //amount of force to apply
              collider.AddForce(other.contacts[0].normal * bounce);
              isBouncing = true;
              Invoke("StopBounce", 0.3f);

          }
          else if (other.gameObject.tag == "virus")
          {
              float bounce = 60f; //amount of force to apply
              collider.AddForce(other.contacts[0].normal * bounce);
              isBouncing = true;
              Invoke("StopBounce", 0.3f);
          }*/

    


}
