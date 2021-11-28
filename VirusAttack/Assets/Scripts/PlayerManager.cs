using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour {

    PhotonView PV;
    GameObject controller;
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
        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
        Debug.Log("Instantiated Player Controller");
        if(Random.Range(0,3) == 0){
            controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "TankT"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
        }
        else{
            controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "tank"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
        }
    }   

    public void Die(){
        PhotonNetwork.Destroy(controller);
        CreateController(); // respawing
    }
}
