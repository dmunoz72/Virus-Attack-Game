using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour {

    PhotonView PV;
    GameObject controller;

    public GameObject[] playerPrefabs;
    GameObject playerToSpawn;

    CharSelectController charselectcontroller;
   
    void Awake(){
        PV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start(){ 
        if (PV.IsMine){
          CreateController();
        }
    }
    
    void CreateController(){
    // Instantiate player controller
    Transform spawnPoint = SpawnManager.Instance.GetSpawnpoint();
    playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
    controller = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, spawnPoint.rotation, 0, new object[] { PV.ViewID });
    }

    public void Die(){
        PhotonNetwork.Destroy(controller);
        CreateController(); // respawing
    }
}