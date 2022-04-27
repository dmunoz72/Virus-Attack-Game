using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class BinaryPit : MonoBehaviour
{
    public float ScrollX = 0.5f;
    public float ScrollY = 0.5f; // axis to scroll on


    // Update is called once per frame
    void Update()
    {
        float OffsetX = Time.time * ScrollX;
        float OffsetY = Time.time * ScrollY;
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(OffsetX, OffsetY);
    }

    [PunRPC]
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "wizard" | other.gameObject.tag == "tank")
        {
            other.gameObject.GetPhotonView().RPC("RPC_TakeDamage", RpcTarget.All, 1000f);
            print("BINARY PIT DEATH!!");
        }
        if (other.gameObject.tag == "virus" | other.gameObject.tag == "medic")
        {
            other.gameObject.GetPhotonView().RPC("RPC_TakeDamage", RpcTarget.All, 1000f);
            print("BINARY PIT DEATH!!");
        }
        else if(other.gameObject.tag == "glasscannon") {
            other.gameObject.GetPhotonView().RPC("RPC_TakeDamage", RpcTarget.All, 1000f);
            print("BINARY PIT DEATH!!");
        }
    }
}
