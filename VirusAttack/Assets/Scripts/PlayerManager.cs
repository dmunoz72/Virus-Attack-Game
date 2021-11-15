using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour {

    PhotonView PV;

    void Awake(){
        PV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start(){
        if(PV.IsMine){
            CreateController();
        }
    }

    void CreateController(){
        // Instantiate player controller
        Vector3 spawn = new Vector3(0.0f, 10.0f, 0.0f);
        Debug.Log("Instantiated Player Controller");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "tank"), spawn, Quaternion.identity);
    }        
}
