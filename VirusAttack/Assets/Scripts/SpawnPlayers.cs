using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour {
	
  public GameObject playerPrefab;
  
  public float xCoord;
  public float yCoord;
  public float zCoord;
  
  private void Start(){
	  Vector2 spawnPosition = new Vector3(xCoord, yCoord, zCoord);
	  
	  PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
	  
	  
  }
}
