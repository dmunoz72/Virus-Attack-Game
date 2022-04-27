using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.IO;
//using UnityEngine.SceneManagement;


public class RoomManager : MonoBehaviourPunCallbacks{
    public static RoomManager Instance;
  
    void Awake(){
            if (Instance)
            { //Checks if another RoomManager exists
                Destroy(gameObject); // if so, destroys itself and returns
                return;
            }
        
        DontDestroyOnLoad(gameObject); //it's the only RoomManager
        Instance = this; // sets itself to be the instance
    }

    public override void OnEnable(){
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable(){
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode){

        if (scene.name == "MotherboardLevel" ||	scene.name == "MouseMapFrame")
        {
          Debug.Log("PLAYER MANAGER 1");
          PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }
    }

}